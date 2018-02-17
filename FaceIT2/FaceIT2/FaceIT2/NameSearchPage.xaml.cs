using Android.Widget;
using FaceIT2.Models;
using FaceIT2.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FaceIT2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NameSearchPage : ContentPage
    {
        private const string Url = "HTTP://faceitbeta.azurewebsites.net/api/UserAccount/SearchName";
        HttpClient client = new HttpClient();
        SearchKeyBindingModel name = new SearchKeyBindingModel(); 
        UserViewModel user = new UserViewModel();
        private ObservableCollection<ProfileViewModel> data;
        private UserViewModel user1;
        
        public NameSearchPage(UserViewModel user1)
        {
            this.user1 = user1;
            InitializeComponent();
        }

        private async void SearchList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as ProfileViewModel;
            user.UploaderID = item.UserId;

            var notifyUser = new NotifiUserViewModel
            {
                SearcherId = user1.UploaderID,
                SearchedId = item.UserId,
            };
        
            await Navigation.PushAsync(new SearchResultPage(notifyUser));
             
         }

        private async void Search_Btn(object sender, EventArgs e)
        {
            name.Keyword= Search.Text;
            try
            {
                var json = JsonConvert.SerializeObject(name);
                HttpContent content = new StringContent(json);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(Url, content);

                string s = response.StatusCode.ToString();
                Toast.MakeText(Android.App.Application.Context, s, ToastLength.Long).Show();

                if (response.IsSuccessStatusCode)
                {
                    string Message = "Searching Profiles....";
                    Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();

                    var get1 = response.Content.ReadAsStringAsync().Result;

                    var json2 = JsonConvert.DeserializeObject<List<ProfileViewModel>>(get1);

                    data = new ObservableCollection<ProfileViewModel>(json2);
                    SearchList.ItemsSource = data;
                }
            }
            catch (Exception)
            {

                string Message = "Registration Failed";
                Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
            }
        }            
    }

}
    