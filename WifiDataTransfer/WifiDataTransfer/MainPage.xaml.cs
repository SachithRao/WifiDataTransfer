using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace WifiDataTransfer
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var dataTransferServices = DependencyService.Get<IDataTransferServices>();
            dataTransferServices.ConnectToWifi("SRedmiNote 7 Pro", "Sachith27198$");

            FindPeer.Clicked += async (sender, args) =>
            {
                dataTransferServices.sendPeerToPeerData("iPhone");
            };
        }
    }
}
