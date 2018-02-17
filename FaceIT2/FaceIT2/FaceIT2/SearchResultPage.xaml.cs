using System;
using FaceIT2.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Android.Widget;
using FaceIT2.Models;

namespace FaceIT2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResultPage : ContentPage
    {
        ProfileViewModel user = new ProfileViewModel();
        HttpClient client = new HttpClient();
       
        private const string Url = "HTTP://faceitbeta.azurewebsites.net/api/UserAccount/UserProfile/";
        private const string Url2 = "HTTP://faceitbeta.azurewebsites.net/api/UserAccount/UpdateNotification/";
        private NotifiUserViewModel notifyUser;

        public SearchResultPage(NotifiUserViewModel notifyUser)
        {
            this.notifyUser = notifyUser;
                   InitializeComponent();
        }
        

        protected async override void OnAppearing()
        {
            try
            {
                var user1 = new UserViewModel
                {
                    UploaderID = notifyUser.SearchedId,
                };
               
                var json = JsonConvert.SerializeObject(user1);
                HttpContent content = new StringContent(json);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(Url, content);


                if (response.IsSuccessStatusCode)
                {
                    string Message = "Uploading Profile...";
                    Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();

                    var get1 = response.Content.ReadAsStringAsync().Result;
                    var json2 = JsonConvert.DeserializeObject<ProfileViewModel>(get1);
                    user = json2;

                    var x = user.Link;
                    
                    if (x == null)
                    {
                        x = "https://www.lakehorn.com/wp-content/uploads/2016/05/User-icon.png";
                    }
                    Uri uri = new Uri(x);


                    Toast.MakeText(Android.App.Application.Context, "ID: "+user.UserId.ToString(), ToastLength.Long).Show();
                    profile.Source = ImageSource.FromUri(uri);

                    firstNameLabel.Text = user.FirstName;
                    lastNameLabel.Text = user.LastName;
                    descriptionLabel.Text = user.Description;
                    emailLabel.Text = user.Email;
                    addressLabel.Text = user.Address;
                    genderLabel.Text = user.Gender;
                    mobileNumberLabel.Text = user.MobileNumber;

                    try
                    {
                        var res = new NotifyUserBindingModel
                        {
                            SearcherId = notifyUser.SearcherId,
                            SearchedId = user.UserId,
                        };

                        var json3 = JsonConvert.SerializeObject(res);
                        HttpContent content2 = new StringContent(json3);

                        Toast.MakeText(Android.App.Application.Context, "Sending1-ID: " + res.SearchedId.ToString(), ToastLength.Long).Show();
                        Toast.MakeText(Android.App.Application.Context, "Sending2-ID: " + res.SearcherId.ToString(), ToastLength.Long).Show();


                        content2.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        var response2 = await client.PostAsync(Url2, content2);

                        string s = response2.StatusCode.ToString();
                        Toast.MakeText(Android.App.Application.Context, s, ToastLength.Long).Show();

                        if (response2.IsSuccessStatusCode)
                        {
                            string Message2 = "Notification Send..";
                            Toast.MakeText(Android.App.Application.Context, Message2, ToastLength.Long).Show();

                            var get2 = response2.Content.ReadAsStringAsync().Result;
                            var json4 = JsonConvert.DeserializeObject<NotifyUserBindingModel>(get2);
                            var temp = json4;

                            Toast.MakeText(Android.App.Application.Context, "Searched-ID: " + temp.SearchedId.ToString(), ToastLength.Long).Show();
                            Toast.MakeText(Android.App.Application.Context, "SearcherId-ID: " + temp.SearcherId.ToString(), ToastLength.Long).Show();

                            base.OnAppearing();
                        }
                    }
                    catch (Exception)
                    {
                        string Mes = "Error Occured During Notifying..!";
                        Toast.MakeText(Android.App.Application.Context, Mes, ToastLength.Long).Show();
                       // await Navigation.PushAsync(new MainPage());
                    }
                    
                    
                }

            }
            catch (Exception)
            {
                string Message = "Error Occured";
                Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                //await Navigation.PushAsync(new MainPage());
            }
            
        }


        private void Report_Btn2(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ReportUserPage());
        }

        private async void SignOut_Btn4(object sender, EventArgs e)
        {
             await Navigation.PushAsync(new MainPage());
        }
    }
}