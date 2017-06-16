﻿using Plugin.Connectivity;

namespace PortableApp.Data
{
    class Connectivity
    {
        public static bool checkWiFiConnection()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                foreach (var band in CrossConnectivity.Current.ConnectionTypes)
                {
                    if (band.ToString() == "WiFi")
                        return true;
                }
                return false;
            }
            else
            { 
                return false;
            }
        }
    }
}
