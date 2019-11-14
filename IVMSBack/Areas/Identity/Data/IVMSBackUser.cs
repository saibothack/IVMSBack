using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IVMSBack.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the IVMSBackUser class
    public class IVMSBackUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        [ScaffoldColumn(false)]
        public string UserCreate { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? DateCreate { get; set; }
        [ScaffoldColumn(false)]
        public string UserModified { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? DateModified { get; set; }
        [ScaffoldColumn(false)]
        public string UserEnd { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? DateEnd { get; set; }
        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public string RoleID { get; set; }

    }
}
