using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InzApp.Models;
using System.IO;

namespace InzApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        
        public HomePage()
        {
            InitializeComponent();
            CreateDataBase();
        }
        void CreateDataBase()
        {
            if(File.Exists(App.FilePath)) return;
            //here we create build in database

            List<Medicament> mlist = new List<Medicament>();
          

            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
               //inserting ours list od medicament to database
                conn.CreateTable<Medicament>();
                foreach (Medicament item in mlist)
                {
                    conn.Insert(item);
                }
         

            }
        }
        void CameraButtonClick(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new CameraPage());
        }
       
    }
}