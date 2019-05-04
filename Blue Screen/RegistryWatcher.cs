using System;
using System.Collections.ObjectModel;
using System.Management;
using Microsoft.Win32;

namespace Blue_Screen
{
    class RegistryWatcher : ManagementEventWatcher, IDisposable
    {

        static ReadOnlyCollection<RegistryKey> supportedHives = null;

        public static ReadOnlyCollection<RegistryKey> SupportedHives
        {
            get
            {
                if (supportedHives == null)
                {
                    RegistryKey[] hives = new RegistryKey[]
                    {
                        Registry.LocalMachine,
                        Registry.Users,
                        Registry.CurrentConfig
                    };
                    supportedHives = Array.AsReadOnly<RegistryKey>(hives);
                }
                return supportedHives;
            }
        }

        public RegistryKey Hive { get; private set; }
        public string KeyPath { get; private set; }
        public RegistryKey KeyToMonitor { get; private set; }

        public event EventHandler<RegistryKeyChangeEventArgs> RegistryKeyChangeEvent;

        public RegistryWatcher(RegistryKey hive, string keyPath)
        {
            this.Hive = hive;
            this.KeyPath = keyPath;
            this.KeyToMonitor = hive.OpenSubKey(keyPath);

            if (KeyToMonitor != null)
            { 
                string queryString = string.Format(@"SELECT * FROM RegistryKeyChangeEvent  
                   WHERE Hive = '{0}' AND KeyPath = '{1}' ", this.Hive.Name, this.KeyPath);

                WqlEventQuery query = new WqlEventQuery();
                query.QueryString = queryString;
                query.EventClassName = "RegistryKeyChangeEvent";
                query.WithinInterval = new TimeSpan(0, 0, 0, 1);
                this.Query = query;

                this.EventArrived += new EventArrivedEventHandler(RegistryWatcher_EventArrived);
            }
            else
            {
                string message = string.Format(
                    @"The registry key {0}\{1} does not exist",
                    hive.Name,
                    keyPath);
                throw new ArgumentException(message);
            }
        }

        void RegistryWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            if (RegistryKeyChangeEvent != null)
            {
                RegistryKeyChangeEventArgs args = new RegistryKeyChangeEventArgs(e.NewEvent);
                RegistryKeyChangeEvent(sender, args);
            }
        }

        public new void Dispose()
        {
            base.Dispose();
            if (this.KeyToMonitor != null)
            {
                this.KeyToMonitor.Dispose();
            }
        }
    }
}