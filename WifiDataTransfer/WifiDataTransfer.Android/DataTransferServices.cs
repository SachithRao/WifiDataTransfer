using System;
using Android.Content;
using Android.Net.Wifi;
using WifiDataTransfer.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(DataTransferServices))]
namespace WifiDataTransfer.Droid
{
    public class DataTransferServices: IDataTransferServices
    {
        public void ConnectToWifi(string ssid, string password)
        {
            var wifiManager = (WifiManager)Android.App.Application.Context
                        .GetSystemService(Context.WifiService);

            var formattedSsid = $"\"{ssid}\"";
            var formattedPassword = $"\"{password}\"";

            wifiManager.SetWifiEnabled(true);

            var wifiConfig = new WifiConfiguration
            {
                Ssid = formattedSsid,
                PreSharedKey = formattedPassword
            };

            var addNetwork = wifiManager.AddNetwork(wifiConfig);

            //var network = wifiManager.ConfiguredNetworks
            //     .FirstOrDefault(n => n.Ssid == ssid);

            //if (network == null)
            //{
            //    Console.WriteLine($"Cannot connect to network: {ssid}");
            //    return;
            //}

            wifiManager.Disconnect();
            var enableNetwork = wifiManager.EnableNetwork(addNetwork, true);
        }
    }
}
