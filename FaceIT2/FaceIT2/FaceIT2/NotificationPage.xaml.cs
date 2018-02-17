using Android.Widget;
using FaceIT2.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FaceIT2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {
        
        private const string Url = "http://faceitbeta.azurewebsites.net/api/UserAccount/Notification";

        UserViewModel model1 = new UserViewModel();
        private HttpClient _client = new HttpClient();
        NotificationViewModel model = new NotificationViewModel();
        private ObservableCollection<NotificationViewModel> _posts;
        private UserViewModel user1;


        public NotificationPage(UserViewModel user1)
        {
            InitializeComponent();
            this.user1 = user1;
        }

        private async void Notification_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as NotificationViewModel;
            
            var notifyUser = new NotifiUserViewModel
            {
                SearcherId = user1.UploaderID,
                SearchedId = item.SearcherId,
            };

            await Navigation.PushAsync(new SearchResultPage(notifyUser));
        }


        protected override async void OnAppearing()
        {

            try
            {

                var send = JsonConvert.SerializeObject(user1);
                HttpContent content = new StringContent(send, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var content2 = await _client.PostAsync(Url, content);

                
              
                string s = content2.StatusCode.ToString();
                
                Toast.MakeText(Android.App.Application.Context, s, ToastLength.Long).Show();
                
                if (content2.IsSuccessStatusCode)
                {
                    var get1 = content2.Content.ReadAsStringAsync().Result;
                    var posts = JsonConvert.DeserializeObject<List<NotificationViewModel>>(get1);
                   
                    _posts = new ObservableCollection<NotificationViewModel>(posts);
                    

                    string Message = "Loading... ";
                    Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                    listview.ItemsSource = _posts;

                    base.OnAppearing();

                }
            }
            catch (Exception)
            {
                string Message = "Error Occured!! ";
                Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();

            }
            
        }


        private async void listview_Refreshing(object sender, EventArgs e)
        {
            try
            {
                var send = JsonConvert.SerializeObject(user1);
                HttpContent content = new StringContent(send, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var content2 = await _client.PostAsync(Url, content);

                var get1 = content2.Content.ReadAsStringAsync().Result;
                var posts = JsonConvert.DeserializeObject<List<NotificationViewModel>>(get1);
                _posts = new ObservableCollection<NotificationViewModel>(posts);

                string s = content2.StatusCode.ToString();

                if (s == "BadRequest")
                {
                    Toast.MakeText(Android.App.Application.Context, "Invalid Login Credentials", ToastLength.Long).Show();

                }

                if (content2.IsSuccessStatusCode)
                {

                    string Message = "Loading... ";
                    Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                    listview.ItemsSource = _posts;
                    listview.IsRefreshing = false;

                }
            }
            catch (Exception)
            {
                string Message = "Error Occured!! ";
                Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();

            }

           

        }

         private void listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
           
            var model = e.SelectedItem as NotificationViewModel;
            var userId = model.SearcherId;
            model1.UploaderID = userId;
            
           // await Navigation.PushAsync(new SearchResultPage(model1));

        }


        private void ToolBar_Btn2(object sender, EventArgs e)
        {
           // await Navigation.PushAsync(new SearchPage());
        }

        private void ToolBar_Btn3(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());

        }
        
    }
}


