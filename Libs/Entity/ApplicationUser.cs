using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Avt { get; set; }
        public string? Gender { get; set; }
        public string? Certification { get; set; }
        public int? CategoryId { get; set; }
        public int? TopicId { get; set; }
        public string? University { get; set; }
        public string? Major { get; set; }
        public string? Address { get; set; }
        public int? FileId { get; set; }
    }
}
