using System;
using System.ComponentModel.DataAnnotations;

namespace IVMSBack.Models
{
    public class DefaultData
    {
        [Key]
        public int Id { get; set; }

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

    }
}
