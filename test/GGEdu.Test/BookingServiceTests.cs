using AutoMapper;
using FluentAssertions;
using GGEdu.Application.Services.Commons;
using GGEdu.Core.Entities.Commons.Bookings;
using GGEdu.Core.Entities.Teachers;
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
        #endregion 




        private void SetUpCurrentTeacher(Teacher teacher)
        {
            _currentUserMock.Setup(c => c.UserId).Returns(teacher.UserId);
            _teacherRepoMock
                .Setup(c => c.GetByAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(teacher);
        }

        private Teacher FakeTeacher() => new()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };
    }
}
