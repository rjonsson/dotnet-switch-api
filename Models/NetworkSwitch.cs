using System.Collections.Generic;

namespace switch_api.Models
{
    public class NetworkSwitch
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Domain { get; set; }

        public string Ip { get; set; }

        public SyncStatus Status { get; set; }

        public List<Port> Ports { get; set; }

        public enum SyncStatus
        {
            Unknown,
            Collected,
            Failed
        }

    }
}
