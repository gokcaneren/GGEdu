namespace GGEdu.Core.Logging.Configurations
{
    public class ElasticSearch
    {
        public string Url { get; set; }
        public string Index { get; set; }
        public string LogLevel { get; set; }
        public string FailureFile { get; set; }
    }
}
