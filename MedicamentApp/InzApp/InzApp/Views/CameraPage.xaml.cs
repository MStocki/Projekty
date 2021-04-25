using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using SQLite;
using InzApp.Models;

namespace InzApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        private  List<Medicament> medicaments;
        public CameraPage()
        {
            InitializeComponent();
            //connecting with database and download medicament to the list
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Medicament>();
                 medicaments = conn.Table<Medicament>().ToList();
            }
        }
        async void TakePhoto()
        {

            InternetAccess.IsVisible =false;
            string objectFound = "";
            float prediction=0;
            List<Classification> classificationList = new List<Classification>();
            PredictionResult result = new PredictionResult();
            await CrossMedia.Current.Initialize();
            // asynchronous method of taking pictures with the built-in camera
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                CompressionQuality = 75,
                Name = "medicament.jpg"
            }
            ) ; 
           
            if (file == null)
            {
                return;
            }
            var stream = file.GetStream();
            byte[] bytes = System.IO.File.ReadAllBytes(file.Path);
            var client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 3);
            client.DefaultRequestHeaders.Add("Prediction-Key", "key ");

            string url = "url to custom vision ";

            HttpResponseMessage response;

            var content = new ByteArrayContent(bytes);
            
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            try
            {
            //geting ours prediction from custom vision server using http client
               response = await client.PostAsync(url, content);
               var json = await response.Content.ReadAsStringAsync();
               result = JsonConvert.DeserializeObject<PredictionResult>(json);
               classificationList= result.Predictions;
               objectFound = classificationList[0].TagName.ToString();
               prediction = classificationList[0].Probability;
               prediction *= 100;
            }
            catch (Exception e)
            {
                InternetAccess.Text = "No Internet Access!";
                InternetAccess.IsVisible = true;
            }
           
            Photo.Source = ImageSource.FromStream(() => stream);
            // accuracy of ours prediction must over 75%
            if (objectFound != null && prediction > 75)
            {
                Found.Text = objectFound;
                Medicament foundMedicament = medicaments.Find(x => x.Name == objectFound);
                if (foundMedicament != null)
                {
                    // showing results to screen
                    CompositionLabel.IsVisible = true;
                    DoseLabel.IsVisible = true;
                    IndicationsLabel.IsVisible = true;
                    AbilityLabel.IsVisible = true;
                    Composition.Text = foundMedicament.Composition;
                    Dose.Text = foundMedicament.Dose;
                    Indications.Text = foundMedicament.Indications;
                    if (foundMedicament.AbilityToDrive)
                        AbilityToDrive.Text = "Tak";
                    else AbilityToDrive.Text = "Nie";
                }
                else
                {
                    Found.Text = Found.Text + Environment.NewLine + " Object not found in database!";
                }
            }
            else
            {
                Found.Text = "Object not found!";
                CompositionLabel.IsVisible = false;
                DoseLabel.IsVisible = false;
                IndicationsLabel.IsVisible = false;
                AbilityLabel.IsVisible = false;
                Composition.Text = "";
                Dose.Text = "";
                Indications.Text = "";             
                AbilityToDrive.Text = "";
            }
        }

        void TakePhotoClick(object sender, EventArgs e)
        {
            TakePhoto();   
        }
        public class PredictionResult
        {
        public List<Classification> Predictions { get; set; }
        }

        public class Classification
        {
            public float Probability { get; set; }
            public string TagName { get; set; }

            public Classification(string tagName, float probability)
            {
            TagName = tagName;
            Probability = probability;
            }
        }
    }
}