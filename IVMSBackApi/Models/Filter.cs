using Newtonsoft.Json;

namespace IVMSBackApi.Models
{
    public class Filter
    {
        [JsonProperty("operator")]
        public string operador { get; set; }
        [JsonProperty("value")]
        public string valor { get; set; }
        [JsonProperty("property")]
        public string propiedad { get; set; }
    }
}
