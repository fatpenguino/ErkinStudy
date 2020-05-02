namespace ErkinStudy.Web.Models
{
    public class LandingViewModel
    {
        public long FolderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public long Price { get; set; }
        public long? TeacherId { get; set; }
        public LandingMedia Media { get; set; }
    }

    public class LandingMedia
    {
        public string Path { get; set; }
        public MediaType Type { get; set; }
    }

    public enum MediaType
    {
        Image,
        Video
    }


    public class LandingPageJson
    {
        public string Text { get; set; }
        public string MediaPath { get; set; }
        public int MediaType { get; set; }
    }
}
