using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace switch_api.Models
{
    public class Vlan
    {
        public int Id { get; set; }

        public string Name { get; set; }
        [JsonIgnore]
        public List<PortVlan> Ports { get; set; }

    }
}
