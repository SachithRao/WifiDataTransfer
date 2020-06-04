using System;
namespace WifiDataTransfer
{
    public interface IDataTransferServices
    {
        void ConnectToWifi(string ssid, string password);
        void sendPeerToPeerData(string v);
    }
}
