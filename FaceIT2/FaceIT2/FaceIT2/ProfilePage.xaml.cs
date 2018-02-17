using System;
using System.Net.Http;
using FaceIT2.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Android.Widget;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FaceIT2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class ProfilePage : ContentPage
    {        
        ProfileViewModel user = new ProfileViewModel();
        HttpClient client = new HttpClient();
        private UserViewModel user1;
        private const string Url = "HTTP://faceitbeta.azurewebsites.net/api/UserAccount/UserProfile/";

        public ProfilePage(UserViewModel user1)
        {
                        
            this.user1 = user1;
            InitializeComponent();
        }
        
        private async void ToolBar_Btn1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationPage(user1));

        }

        private async void ToolBar_Btn2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage(user1));
        }

        protected async override void OnAppearing()
        {
            try
            {
                var json = JsonConvert.SerializeObject(user1);
                HttpContent content = new StringContent(json);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(Url,content);


                if (response.IsSuccessStatusCode)
                {
                    string Message = "Uploading Profile...";
                    Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();

                    var get1 = response.Content.ReadAsStringAsync().Result;
                    var json2 = JsonConvert.DeserializeObject<ProfileViewModel>(get1);
                    user = json2;
                    var x = user.Link;
                    
                    Uri uri = new Uri(x);

                    Toast.MakeText(Android.App.Application.Context, "ID : "+user.UserId.ToString(), ToastLength.Long).Show();
                    profile.Source = ImageSource.FromUri(uri);

                    firstNameLabel.Text = user.FirstName;
                    lastNameLabel.Text = user.LastName;
                    descriptionLabel.Text = user.Description;
                    emailLabel.Text = user.Email;
                    addressLabel.Text = user.Address;
                    genderLabel.Text = user.Gender;
                    mobileNumberLabel.Text = user.MobileNumber;

                    base.OnAppearing();
                }

            }
            catch (Exception)
            {
                string Message = "Registration Failed";
                Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                await Navigation.PushAsync(new MainPage());
            }
            
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void ToolBar_Btn3(object sender, EventArgs e)
        {
          
          await Navigation.PushAsync(new EditPage(user1));

        }

        private async void ToolBar_Btn4(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new MainPage());

        }
    }
}