using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Browser;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;

namespace Sporty.Charts
{
    public partial class MainPage : UserControl
    {
        private readonly string username;

        public MainPage(string username)
        {
            this.username = username;
            InitializeComponent();
            Loaded += Page_Loaded;

            HtmlPage.RegisterScriptableObject("SportyReport", this);
        }

        [ScriptableMember]
        public int Month { get; set; }

        [ScriptableMember]
        public int Year { get; set; }

        [ScriptableMember]
        public void Update()
        {
            string attributes = String.Empty;
            if (Month != 0 && Year != 0)
                attributes = String.Format("/{0}/{1}", Month, Year);
            //mvc.OpenReadCompleted += new OpenReadCompletedEventHandler(mvc_OpenReadCompleted);
            ////mvc.Credentials = CredentialCache.DefaultCredentials;
            //var uri = Application.Current.Host.Source.AbsoluteUri + @"/MonthlyData";
            ////http://localhost:52197/Report/MonthlyData
            ////http://sportsplaner.org/Report/MonthlyData
            //mvc.OpenReadAsync(new Uri(String.Concat("http://localhost:52197/Report/MonthlyData", attributes)));

            WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
            var client = new WebClient();
            client.DownloadStringCompleted += client_DownloadStringCompleted;
            //http://localhost:52197/Report/user
            string uri = HtmlPage.Document.DocumentUri.OriginalString;
            client.DownloadStringAsync(new Uri(String.Concat(uri, "/", username, "/MonthlyData", attributes)));
        }

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var json = new DataContractJsonSerializer(typeof (List<ExercisesPerDay>));

            byte[] byteArray = Encoding.UTF8.GetBytes(e.Result);
            var stream = new MemoryStream(byteArray);

            var cats = (List<ExercisesPerDay>) json.ReadObject(stream);
            DurationPerMonth.DataContext = cats;
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        public void mvc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            var json = new DataContractJsonSerializer(typeof (List<ExercisesPerDay>));
            //string result = null;
            //using (var sr = new StreamReader(e.Result))
            //{
            //    result = sr.ReadToEnd();
            //}
            //var cats = JsonConvert.DeserializeObject<ExerciseView>(result);
            var cats = (List<ExercisesPerDay>) json.ReadObject(e.Result);
            DurationPerMonth.DataContext = cats;
        }
    }

    public class ExercisesPerDay
    {
        public int Day { get; set; }
        public int Duration { get; set; }
    }
}