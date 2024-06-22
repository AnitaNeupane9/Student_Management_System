using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitaSMS.Models.Entity
{
    public class Students: BaseEntity
    {
   
        public int CourseId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string StudentProfileurl { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public IFormFile studentPhoto { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public virtual Courses Course { get; set; }
    }
}
