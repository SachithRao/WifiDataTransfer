using System;
using UIKit;
using MultipeerConnectivity;
using NetworkExtension;
using WifiDataTransfer.iOS;
using Xamarin.Forms;
using Foundation;
using ObjCRuntime;

[assembly: Dependency(typeof(DataTransferServices))]
namespace WifiDataTransfer.iOS
{
   
    public partial class DataTransferServices: IDataTransferServices
    {
        string serviceTypeString = "example-color";
        MCPeerID myPeerId;
        static MCSession session;
       


        public void ConnectToWifi(string ssid, string password)
        {
            //var wifiManager = new NEHotspotConfigurationManager();
            //var wifiConfig = new NEHotspotConfiguration(ssid, password, false);

            //wifiManager.ApplyConfiguration(wifiConfig, (error) =>
            //{
            //    if (error != null)
            //    {
            //        Console.WriteLine($"Error while connecting to WiFi network {ssid}: {error}");
            //    }
            //});

            initializeMCServices();

        }

        

        public void initializeMCServices()
        {
            myPeerId = new MCPeerID(UIDevice.CurrentDevice.Name);

            MyAdvertiserDelegate myAdvertiserDelegate = new MyAdvertiserDelegate(this);
            MyBrowserDelegate myBrowserDelegate = new MyBrowserDelegate(this);
            MySessionDelegate mySessionDelegate = new MySessionDelegate(this);

            var emptyDict = new NSDictionary();
            MCNearbyServiceAdvertiser serviceAdvertiser = new MCNearbyServiceAdvertiser(myPeerId, emptyDict, serviceTypeString);
            serviceAdvertiser.Delegate = myAdvertiserDelegate;
            serviceAdvertiser.StartAdvertisingPeer();

            MCNearbyServiceBrowser serviceBrowser = new MCNearbyServiceBrowser(myPeerId, serviceTypeString);
            serviceBrowser.Delegate = myBrowserDelegate;
            serviceBrowser.StartBrowsingForPeers();

            session = new MCSession(myPeerId);
            session.Delegate = mySessionDelegate;


            sendPeerToPeerData(myPeerId.DisplayName);
        }

        public void sendPeerToPeerData(string text)
        {
            NSError error;
            var message = NSData.FromString(text);
            session.SendData(message, session.ConnectedPeers, MCSessionSendDataMode.Reliable,out error);
        }

        //MCNearbyServiceAdvertiserDelegate methods
        class MyAdvertiserDelegate: MCNearbyServiceAdvertiserDelegate
        {
            private DataTransferServices dataTransferServices;

            public MyAdvertiserDelegate(DataTransferServices dataTransferServices)
            {
                this.dataTransferServices = dataTransferServices;
            }

            public override void DidReceiveInvitationFromPeer(MCNearbyServiceAdvertiser advertiser, MCPeerID peerID, NSData context, MCNearbyServiceAdvertiserInvitationHandler invitationHandler)
            {
                Console.WriteLine("Did Received Invitation From Peer {0)", peerID);
                invitationHandler(true, DataTransferServices.session);
            }
        }

        //MCNearbyServiceBrowserDelegate methods
        class MyBrowserDelegate : MCNearbyServiceBrowserDelegate
        {
            private DataTransferServices dataTransferServices;

            public MyBrowserDelegate(DataTransferServices dataTransferServices)
            {
                this.dataTransferServices = dataTransferServices;
            }

            public override void FoundPeer(MCNearbyServiceBrowser browser, MCPeerID peerID, NSDictionary info)
            {
                Console.WriteLine("Found peer {0}", peerID);
                Console.WriteLine("Invite peer {0}", peerID);
                browser.InvitePeer(peerID, DataTransferServices.session, null, 10.0);
            }

            public override void LostPeer(MCNearbyServiceBrowser browser, MCPeerID peerID)
            {
                Console.WriteLine("Lost peer {0}", peerID);
            }
        }

        //MCSessionDelegate methods
        class MySessionDelegate : MCSessionDelegate
        {
            private DataTransferServices dataTransferServices;

            public MySessionDelegate(DataTransferServices dataTransferServices)
            {
                this.dataTransferServices = dataTransferServices;
            }

            public override void DidChangeState(MCSession session, MCPeerID peerID, MCSessionState state)
            {
                Console.WriteLine("Peer {0} did changed state: {0}", peerID, state);
            }

            public override void DidFinishReceivingResource(MCSession session, string resourceName, MCPeerID fromPeer, NSUrl localUrl, NSError error)
            {
                Console.WriteLine("DidFinishReceivingResource");
            }

            public override void DidReceiveData(MCSession session, NSData data, MCPeerID peerID)
            {
                Console.WriteLine("DidReceiveData :{0}",data);
            }

            public override void DidReceiveStream(MCSession session, NSInputStream stream, string streamName, MCPeerID peerID)
            {
                Console.WriteLine("DidReceiveStream :{0}", streamName);
            }

            public override void DidStartReceivingResource(MCSession session, string resourceName, MCPeerID fromPeer, NSProgress progress)
            {
                Console.WriteLine("DidStartReceivingResource");
            }
        }

       
    }

   
}
