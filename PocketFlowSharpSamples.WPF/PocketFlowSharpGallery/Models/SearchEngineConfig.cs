namespace PocketFlowSharpGallery.Models
{
    public class SearchEngineConfig
    {
        public int Id { get; set; }
        public string Provider { get; set; }
        public string EndPoint { get; set; }
        public string ApiKey { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Provider} - {Description}";
        }
    }
} 