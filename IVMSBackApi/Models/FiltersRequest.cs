using Newtonsoft.Json;
using System.Collections.Generic;

namespace IVMSBackApi.Models
{
    public class FiltersRequest
    {
        public List<Filter> filtros { get; set; }
    }
}
