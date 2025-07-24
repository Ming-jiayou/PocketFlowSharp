namespace PocketFlowSharpGallery.Models
{
    public class LLMConfig
    {
        public int Id { get; set; }
        public string Provider { get; set; }
        public string EndPoint { get; set; }
        public string ModelName { get; set; }
        public string ApiKey { get; set; }
    }
}