namespace GGEdu.Core.Logging
{
    public class InformationLogFormat : MainLogFormat
    {
        public int ResponseStatusCode { get; set; }
        public string ResponseBody { get; set; }
    }
}
