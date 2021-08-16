using System;
using System.Collections.Generic;
using Tizen.Network.WiFi;

namespace WiFiSwitch.Core
{
    public class AccessPointManager
    {
        public event EventHandler AcessPointsLoaded;
        public event EventHandler Connecting;
        public event EventHandler Connected;

        public Dictionary<string, AccessPoint> AccessPoints
        {
            get;
        }

        public Dictionary<string, WiFiConfiguration> Configurations
        {
            get;
        }

        public AccessPointManager()
        {
            AccessPoints = new Dictionary<string, AccessPoint>();
            Configurations = GetConfigurations();

            WiFiManager.ConnectionStateChanged += OnNetworkStateChanged;
        }

        public async void Connect(string ssid)
        {
            Connecting?.Invoke(this, null);

            try
            {
                await WiFiManager.ScanSpecificAPAsync(ssid);

                foreach (var foundAccessPoint in WiFiManager.GetFoundAPs())
                {
                    if (foundAccessPoint.NetworkInformation.Essid == ssid)
                    {
                        await foundAccessPoint.ConnectAsync();
                    }
                }
            }
            catch (Exception)
            {
                Connected?.Invoke(this, null);
            }
        }

        public async void Scan()
        {
            await WiFiManager.ScanAsync();

            UpdateAccessPoints();
            AcessPointsLoaded?.Invoke(this, null);
        }

        private void UpdateAccessPoints()
        {
            foreach (var accessPoint in WiFiManager.GetFoundAPs())
            {
                var ssid = accessPoint.NetworkInformation.Essid;
                var connected = accessPoint.NetworkInformation.ConnectionState == WiFiConnectionState.Connected;

                if (Configurations.ContainsKey(ssid) == false)
                {
                    continue;
                }


                if (AccessPoints.ContainsKey(ssid))
                {
                    AccessPoints[ssid].Connected = connected;
                }
                else
                {
                    AccessPoints.Add(ssid, new AccessPoint()
                    {
                        Connected = connected,
                        Name = ssid
                    });
                }
            }
        }

        private Dictionary<string, WiFiConfiguration> GetConfigurations()
        {
            var configurations = new Dictionary<string, WiFiConfiguration>();

            foreach (var configuration in WiFiManager.GetWiFiConfigurations())
            {
                configurations.Add(configuration.Name, configuration);
            }

            return configurations;
        }


        protected virtual void OnNetworkStateChanged(object sender, ConnectionStateChangedEventArgs args)
        {
            UpdateAccessPoints();

            if (args.State == WiFiConnectionState.Connected || args.State == WiFiConnectionState.Failure)
            {
                Connected?.Invoke(this, null);
            }
            else
            {
                Connecting?.Invoke(this, null);
            }

        }
    }
}
