using System.Text.Json.Serialization;


namespace switch_api.Models
{
    public class PortVlan
    {

        public int PortId { get; set; }
        [JsonIgnore]
        public Port Port { get; set; }

        public int VlanId { get; set; }

        public Vlan Vlan { get; set; }
    }
}
