using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace switch_api.Models
{
    public class Port
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public PortModes Mode { get; set; }

        public bool Shutdown { get; set; }

        public List<PortVlan> Vlans { get; set;  }
        

        public int NetworkSwitchId { get; set; }
        [JsonIgnore]
        public NetworkSwitch NetworkSwitch { get; set; }

        public enum PortModes
        {
            Access,
            Trunk
        }
    }
}
