
using InzApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InzApp
{
    public partial class App : Application
    {
        public static string FilePath;
        public App()
        {
            InitializeComponent();


            MainPage = new NavigationPage(new HomePage());
        }
        public App(string filepath)
        {
            InitializeComponent();
            FilePath = filepath;
            MainPage = new NavigationPage(new HomePage());


        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
