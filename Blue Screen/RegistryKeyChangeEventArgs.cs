using System;
using System.Management;

namespace Blue_Screen
{
    class RegistryKeyChangeEventArgs : EventArgs
    {
        public string Hive { get; set; }
        public string KeyPath { get; set; }
        public uint[] SECURITY_DESCRIPTOR { get; set; }
        public DateTime TIME_CREATED { get; set; }

        public RegistryKeyChangeEventArgs(ManagementBaseObject arrivedEvent)
        {
            this.Hive = arrivedEvent.Properties["Hive"].Value as string;
            this.KeyPath = arrivedEvent.Properties["KeyPath"].Value as string;

            this.TIME_CREATED = new DateTime(
                (long)(ulong)arrivedEvent.Properties["TIME_CREATED"].Value,
                DateTimeKind.Utc).AddYears(1600);
        }
    }
}