using AutoMapper;
using FluentAssertions;
using GGEdu.Application.Services.Commons;
using GGEdu.Core.DTOs.Courses.Bookings.Input;
using GGEdu.Core.Entities.Commons.Bookings;
using GGEdu.Core.Entities.Commons.Subscriptions;
using GGEdu.Core.Entities.Students;
using GGEdu.Core.Entities.Teachers;
using GGEdu.Core.Entities.Teachers.Courses;
using GGEdu.Core.Entities.Users;
using GGEdu.Core.Enums;
using GGEdu.Core.Repositories.Commons;
using GGEdu.Core.Repositories.Students;
using GGEdu.Core.Repositories.Teachers;
using GGEdu.Core.Services.Commons;
using GGEdu.Core.Services.Users;
using GGEdu.Core.UnitOfWorks;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;
using Moq;
using System.Linq.Expressions;
using System.Net;

namespace GGEdu.Test
{
    public class BookingServiceTests
    {
        // ── Mocks ────────────────────────────────────────────────────────
        private readonly Mock<IBookingRepository> _bookingRepoMock = new();
        private readonly Mock<ITeacherRepository> _teacherRepoMock = new();
        private readonly Mock<ISubscriptionRepository> _subscriptionRepoMock = new();
        private readonly Mock<IStudentRepository> _studentRepoMock = new();
        private readonly Mock<IAvailabilityCourseSlotRepository> _slotRepoMock = new();
        private readonly Mock<ICurrentUserService> _currentUserMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock = new();

        private readonly BookingService _sut;

        public BookingServiceTests()
        {
            // Localizer: her key için key'in kendisini döndür
            _localizerMock
                .Setup(x => x[It.IsAny<string>()])
                .Returns<string>(key => new LocalizedString(key, key));

            _unitOfWorkMock
                .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));

            _sut = new BookingService(
                _bookingRepoMock.Object,
                _teacherRepoMock.Object,
                _subscriptionRepoMock.Object,
                _studentRepoMock.Object,
                _currentUserMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _localizerMock.Object,
                _slotRepoMock.Object
            );
        }

        #region GetBookingRequestsAsync
        [Fact]
        public async Task GetBookingRequestsAsync_WhenTeacherNotFound_ShouldReturnBadRequest()
        {
            _currentUserMock.Setup(c => c.UserId).Returns(Guid.NewGuid());

            _teacherRepoMock
                .Setup(c => c.GetByAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Teacher?)null);

            var result = await _sut.GetBookingRequestsAsync(BookingStatus.Scheduled, 10, 1, 10);

            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task GetBookingRequestsAsync_WhenNoBookingsFound_ShouldReturnSuccessWithNullData()
        {
            var teacher = FakeTeacher();
            SetUpCurrentTeacher(teacher);

            _bookingRepoMock.Setup(c=>c.GetBookingsByTeacherIdAndByStatusAsnyc(
                teacher.Id, It.IsAny<BookingStatus?>(), It.IsAny<int?>(), 
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((new List<Booking>(), 0));

            var result = await _sut.GetBookingRequestsAsync(BookingStatus.Scheduled, 10,
                 1, 10);

            result.Success.Should().BeTrue();
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task GetBookingRequestsAsync_WhenBookingsExist_ShouldReturnPagedData()
        {
            // Arrange
            var teacher = FakeTeacher();
            SetupCurrentTeacher(teacher);
            var bookings = FakeBookings(teacher.Id, count: 3);

            _bookingRepoMock
                .Setup(x => x.GetBookingsByTeacherIdAndByStatusAsnyc(
                    teacher.Id, BookingStatus.Scheduled, 10, 1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync((bookings, 3));

            // Act
            var result = await _sut.GetBookingRequestsAsync(BookingStatus.Scheduled, 10, 1, 10);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Items.Should().HaveCount(3);
            result.Data.TotalCount.Should().Be(3);
            result.Data.TotalPages.Should().Be(1);
            result.Data.HasNext.Should().BeFalse();
            result.Data.HasPrevious.Should().BeFalse();
        }

        [Fact]
        public async Task GetBookingRequestsAsync_WhenSecondPage_ShouldReturnCorrectPagination()
        {
            // Arrange
            var teacher = FakeTeacher();
            SetupCurrentTeacher(teacher);
            var bookings = FakeBookings(teacher.Id, count: 5);

            _bookingRepoMock
                .Setup(x => x.GetBookingsByTeacherIdAndByStatusAsnyc(
                    teacher.Id, null, null, 2, 5, It.IsAny<CancellationToken>()))
                .ReturnsAsync((bookings, 12)); // toplam 12, sayfa boyutu 5

            // Act
            var result = await _sut.GetBookingRequestsAsync(null, null, 2, 5);

            // Assert
            result.Data!.Page.Should().Be(2);
            result.Data.TotalPages.Should().Be(3);
            result.Data.HasNext.Should().BeTrue();
            result.Data.HasPrevious.Should().BeTrue();
        }

        #endregion 

        // ════════════════════════════════════════════════════════════════
        // DecideBookingRequestAsync
        // ════════════════════════════════════════════════════════════════

        [Fact]
        public async Task DecideBookingRequestAsync_WhenTeacherNotFound_ShouldReturnBadRequest()
        {
            // Arrange
            _currentUserMock.Setup(x => x.UserId).Returns(Guid.NewGuid());
            _teacherRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Teacher?)null);

            var input = new DecideBookRequestInputDto { Id = Guid.NewGuid(), StudentId = Guid.NewGuid(), IsAccepted = true };

            // Act
            var result = await _sut.DecideBookingRequestAsync(input);

            // Assert
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().BeFalse();
        }

        [Fact]
        public async Task DecideBookingRequestAsync_WhenBookingNotFound_ShouldReturnBadRequest()
        {
            // Arrange
            var teacher = FakeTeacher();
            SetupCurrentTeacher(teacher);
            _bookingRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Booking, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Booking?)null);

            var input = new DecideBookRequestInputDto { Id = Guid.NewGuid(), StudentId = Guid.NewGuid(), IsAccepted = true };

            // Act
            var result = await _sut.DecideBookingRequestAsync(input);

            // Assert
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DecideBookingRequestAsync_WhenSlotNotFound_ShouldReturnBadRequest()
        {
            // Arrange
            var teacher = FakeTeacher();
            SetupCurrentTeacher(teacher);
            var booking = FakeBooking(teacher.Id, BookingStatus.Pending);

            _bookingRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Booking, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);
            _slotRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<AvailabilityCourseSlot, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((AvailabilityCourseSlot?)null);

            var input = new DecideBookRequestInputDto { Id = booking.Id, StudentId = booking.StudentId, IsAccepted = true };

            // Act
            var result = await _sut.DecideBookingRequestAsync(input);

            // Assert
            result.Success.Should().BeFalse();
        }

        [Theory]
        [InlineData(true, BookingStatus.Scheduled, AvailabilityCourseSlotStatus.Booked)]
        [InlineData(false, BookingStatus.Rejected, AvailabilityCourseSlotStatus.Available)]
        public async Task DecideBookingRequestAsync_ShouldSetCorrectStatuses(
            bool isAccepted,
            BookingStatus expectedBookingStatus,
            AvailabilityCourseSlotStatus expectedSlotStatus)
        {
            // Arrange
            var teacher = FakeTeacher();
            SetupCurrentTeacher(teacher);
            var booking = FakeBooking(teacher.Id, BookingStatus.Pending);
            var slot = FakeSlot(teacher.Id, AvailabilityCourseSlotStatus.Pending);
            slot.Id = booking.AvailabilityCourseSlotId;

            _bookingRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Booking, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);
            _slotRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<AvailabilityCourseSlot, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(slot);

            var input = new DecideBookRequestInputDto
            {
                Id = booking.Id,
                StudentId = booking.StudentId,
                IsAccepted = isAccepted
            };

            // Act
            var result = await _sut.DecideBookingRequestAsync(input);

            // Assert
            result.Success.Should().BeTrue();
            booking.Status.Should().Be(expectedBookingStatus);
            slot.Status.Should().Be(expectedSlotStatus);
            booking.DecisionDate.Should().NotBeNull();
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // ════════════════════════════════════════════════════════════════
        // CancelBookingRequestAsTeacherAsync
        // ════════════════════════════════════════════════════════════════

        [Fact]
        public async Task CancelBookingRequestAsTeacherAsync_WhenTeacherNotFound_ShouldReturnBadRequest()
        {
            // Arrange
            _currentUserMock.Setup(x => x.UserId).Returns(Guid.NewGuid());
            _teacherRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Teacher?)null);

            // Act
            var result = await _sut.CancelBookingRequestAsTeacherAsync(Guid.NewGuid());

            // Assert
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CancelBookingRequestAsTeacherAsync_WhenBookingNotFound_ShouldReturnBadRequest()
        {
            // Arrange
            var teacher = FakeTeacher();
            SetupCurrentTeacher(teacher);
            _bookingRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Booking, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Booking?)null);

            // Act
            var result = await _sut.CancelBookingRequestAsTeacherAsync(Guid.NewGuid());

            // Assert
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task CancelBookingRequestAsTeacherAsync_WhenSlotNotAvailable_ShouldReturnBadRequest()
        {
            // Arrange
            var teacher = FakeTeacher();
            SetupCurrentTeacher(teacher);
            var booking = FakeBooking(teacher.Id, BookingStatus.Scheduled);

            _bookingRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Booking, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);
            _slotRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<AvailabilityCourseSlot, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((AvailabilityCourseSlot?)null);

            // Act
            var result = await _sut.CancelBookingRequestAsTeacherAsync(booking.StudentId);

            // Assert
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task CancelBookingRequestAsTeacherAsync_WhenSuccess_ShouldSetCancelledStatusAndFreeSlot()
        {
            // Arrange
            var teacher = FakeTeacher();
            SetupCurrentTeacher(teacher);
            var booking = FakeBooking(teacher.Id, BookingStatus.Scheduled);
            var slot = FakeSlot(teacher.Id, AvailabilityCourseSlotStatus.Pending);

            _bookingRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Booking, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);
            _slotRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<AvailabilityCourseSlot, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(slot);

            // Act
            var result = await _sut.CancelBookingRequestAsTeacherAsync(booking.StudentId);

            // Assert
            result.Success.Should().BeTrue();
            booking.Status.Should().Be(BookingStatus.Cancelled);
            booking.CancelledDate.Should().NotBeNull();
            slot.Status.Should().Be(AvailabilityCourseSlotStatus.Available);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // ════════════════════════════════════════════════════════════════
        // CancelBookingRequestAsStudentAsync
        // ════════════════════════════════════════════════════════════════

        [Fact]
        public async Task CancelBookingRequestAsStudentAsync_WhenStudentNotFound_ShouldReturnBadRequest()
        {
            // Arrange
            _currentUserMock.Setup(x => x.UserId).Returns(Guid.NewGuid());
            _studentRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Student, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Student?)null);

            // Act
            var result = await _sut.CancelBookingRequestAsStudentAsync(Guid.NewGuid());

            // Assert
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CancelBookingRequestAsStudentAsync_WhenSuccess_ShouldSetCancelledStatusAndFreeSlot()
        {
            // Arrange
            var student = FakeStudent();
            SetupCurrentStudent(student);
            var teacherId = Guid.NewGuid();
            var booking = FakeBooking(teacherId, BookingStatus.Pending);
            booking.StudentId = student.Id;
            var slot = FakeSlot(teacherId, AvailabilityCourseSlotStatus.Pending);

            _bookingRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Booking, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);
            _slotRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<AvailabilityCourseSlot, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(slot);

            // Act
            var result = await _sut.CancelBookingRequestAsStudentAsync(teacherId);

            // Assert
            result.Success.Should().BeTrue();
            booking.Status.Should().Be(BookingStatus.Cancelled);
            slot.Status.Should().Be(AvailabilityCourseSlotStatus.Available);
        }

        // ════════════════════════════════════════════════════════════════
        // SendBookingRequestAsync
        // ════════════════════════════════════════════════════════════════

        [Fact]
        public async Task SendBookingRequestAsync_WhenStudentNotFound_ShouldReturnBadRequest()
        {
            // Arrange
            _currentUserMock.Setup(x => x.UserId).Returns(Guid.NewGuid());
            _studentRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Student, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Student?)null);

            // Act
            var result = await _sut.SendBookingRequestAsync(new BookingInputDto());

            // Assert
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task SendBookingRequestAsync_WhenNotSubscribed_ShouldReturnBadRequest()
        {
            // Arrange
            var student = FakeStudent();
            SetupCurrentStudent(student);
            _subscriptionRepoMock
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Subscription, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.SendBookingRequestAsync(new BookingInputDto { TeacherId = Guid.NewGuid() });

            // Assert
            result.Success.Should().BeFalse();
            result.Data.Should().BeFalse();
        }

        [Fact]
        public async Task SendBookingRequestAsync_WhenSlotNotAvailable_ShouldReturnBadRequest()
        {
            // Arrange
            var student = FakeStudent();
            SetupCurrentStudent(student);
            _subscriptionRepoMock
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Subscription, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _slotRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<AvailabilityCourseSlot, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((AvailabilityCourseSlot?)null);

            // Act
            var result = await _sut.SendBookingRequestAsync(new BookingInputDto { TeacherId = Guid.NewGuid() });

            // Assert
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task SendBookingRequestAsync_WhenSuccess_ShouldCreateBookingAndSetSlotToPending()
        {
            // Arrange
            var student = FakeStudent();
            var teacherId = Guid.NewGuid();
            var slotId = Guid.NewGuid();
            SetupCurrentStudent(student);

            var slot = FakeSlot(teacherId, AvailabilityCourseSlotStatus.Available);
            slot.Id = slotId;

            _subscriptionRepoMock
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Subscription, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _slotRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<AvailabilityCourseSlot, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(slot);
            _bookingRepoMock
                .Setup(x => x.CreateAsync(It.IsAny<Booking>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var input = new BookingInputDto
            {
                TeacherId = teacherId,
                AvailabilityCourseSlotId = slotId,
            };

            // Act
            var result = await _sut.SendBookingRequestAsync(input);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().BeTrue();
            slot.Status.Should().Be(AvailabilityCourseSlotStatus.Pending);
            _bookingRepoMock.Verify(
                x => x.CreateAsync(
                    It.Is<Booking>(b =>
                        b.StudentId == student.Id &&
                        b.TeacherId == teacherId &&
                        b.Status == BookingStatus.Pending),
                    false,
                    It.IsAny<CancellationToken>()),
                Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // ════════════════════════════════════════════════════════════════
        // Fake Helpers
        // ════════════════════════════════════════════════════════════════

        private Teacher FakeTeacher() => new()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
        };

        private Student FakeStudent() => new()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
        };

        private Booking FakeBooking(Guid teacherId, BookingStatus status) => new()
        {
            Id = Guid.NewGuid(),
            TeacherId = teacherId,
            StudentId = Guid.NewGuid(),
            AvailabilityCourseSlotId = Guid.NewGuid(),
            Status = status,
        };

        private AvailabilityCourseSlot FakeSlot(Guid teacherId, AvailabilityCourseSlotStatus status) => new()
        {
            Id = Guid.NewGuid(),
            TeacherId = teacherId,
            Status = status,
        };

        private List<Booking> FakeBookings(Guid teacherId, int count) =>
            Enumerable.Range(1, count).Select(i => new Booking
            {
                Id = Guid.NewGuid(),
                TeacherId = teacherId,
                StudentId = Guid.NewGuid(),
                Status = BookingStatus.Scheduled,
                AvailabilityCourseSlotId = Guid.NewGuid(),
                Student = new Student
                {
                    User = new User
                    {
                        FirstName = $"Student{i}",
                        LastName = "Test",
                        Email = $"student{i}@test.com",
                        Photo = ""
                    }
                },
                AvailabilityCourseSlot = new AvailabilityCourseSlot
                {
                    StartAtUtc = DateTime.UtcNow.AddDays(i),
                    EndAtUtc = DateTime.UtcNow.AddDays(i).AddHours(1),
                    TeacherCourseId = Guid.NewGuid(),
                    TeacherCourse = new TeacherCourse
                    {
                        Course = new Course
                        {
                            Id = Guid.NewGuid(),
                            Name = "English",
                            Code = "en"
                        }
                    }
                }
            }).ToList();

        private void SetupCurrentTeacher(Teacher teacher)
        {
            _currentUserMock.Setup(x => x.UserId).Returns(teacher.UserId);
            _teacherRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(teacher);
        }

        private void SetupCurrentStudent(Student student)
        {
            _currentUserMock.Setup(x => x.UserId).Returns(student.UserId);
            _studentRepoMock
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Student, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(student);
        }
    
        private void SetUpCurrentTeacher(Teacher teacher)
        {
            _currentUserMock.Setup(c => c.UserId).Returns(teacher.UserId);
            _teacherRepoMock
                .Setup(c => c.GetByAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(teacher);
        }
    }
}
