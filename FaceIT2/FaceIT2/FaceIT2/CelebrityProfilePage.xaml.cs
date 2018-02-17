using System;
using FaceIT2.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Android.Widget;

namespace FaceIT2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CelebrityProfilePage : ContentPage
    {
        CelebrityViewModel user = new CelebrityViewModel();
        HttpClient client = new HttpClient();

        private const string Url = "HTTP://faceitbeta.azurewebsites.net/api/UserAccount/CelebrityProfile/";

        private UserViewModel user1;
        
        public CelebrityProfilePage(UserViewModel user1)
        {
            this.user1 = user1;
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            try
            {
                var json = JsonConvert.SerializeObject(user1);
                HttpContent content = new StringContent(json);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(Url, content);


                if (response.IsSuccessStatusCode)
                {
                    string Message = "Uploading Profile...";
                    Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();

                    var get1 = response.Content.ReadAsStringAsync().Result;
            
                    var json2 = JsonConvert.DeserializeObject<CelebrityViewModel>(get1);
                    user = json2;
                    
                    var x = user1.Link;
                    if (x == null)
                    {
                        x = "https://www.lakehorn.com/wp-content/uploads/2016/05/User-icon.png";
                    }
                    Uri uri = new Uri(x);

                    Toast.MakeText(Android.App.Application.Context, "ID Recevied", ToastLength.Long).Show();
                    profile.Source = ImageSource.FromUri(uri);

                    firstNameLabel.Text = user.FirstName;
                    lastNameLabel.Text = user.LastName;
                    descriptionLabel.Text = user.Description;
                    fieldLabel.Text = user.Field;
                    genderLabel.Text = user.Gender;
                    base.OnAppearing();
                }

            }
            catch (Exception)
            {
                string Message = "Error Occured";
                Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                await Navigation.PushAsync(new MainPage());
            }
            
        }

        private async void Notificaion_Btn1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage());

        }

        private async void Search_Btn2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage(user1));
        }

        private async void Suggestion_Btn3(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new SuggestionPage());
        }

        private async void SignOut_Btn4(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}