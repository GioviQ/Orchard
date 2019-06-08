using System.ComponentModel.DataAnnotations;

namespace Q.SeoTools.Models
{
    public class CustomAliasRecord
    {
        public virtual int Id { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual bool Permanent { get; set; }

        [StringLength(2048)]
        public virtual string Alias { get; set; }
        [StringLength(2048)]
        public virtual string OriginalUrl { get; set; }

    }
}