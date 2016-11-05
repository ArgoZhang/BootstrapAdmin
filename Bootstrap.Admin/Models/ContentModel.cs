
namespace Bootstrap.Admin.Models
{
    public class ContentModel : HeaderBarModel
    {
        public ContentModel()
        {
            ShowMenu = true;
        }
        public string Url { get; set; }
    }
}