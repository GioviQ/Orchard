using System.ComponentModel.DataAnnotations;

namespace Q.SeoTools.ViewModels
{

    public class AdminCreateViewModel
    {
        [Required, StringLength(2048)]
        public string Alias { get; set; }
        [Required, StringLength(2048)]
        public string OriginalUrl { get; set; }
        public bool Permanent { get; set; }
    }
}