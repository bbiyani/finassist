namespace SharedLibs.Models
{
    public class EventGridSettings
    {
        public string Source { get; set; } = "/finassist/document-uploader";
        public string EventType { get; set; } = "document.uploaded";
        public string ContentType { get; set; } = "application/json";
    }
}