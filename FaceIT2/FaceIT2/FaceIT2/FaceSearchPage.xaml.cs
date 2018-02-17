using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FaceAPIFunctions;
using Android.Widget;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.ObjectModel;
using FaceIT2.ViewModels;
using System.Net.Http.Headers;

namespace FaceIT2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FaceSearchPage : ContentPage
    {

        private const string Url = "HTTP://faceitbeta.azurewebsites.net/api/UserAccount/SearchFace";
        private const string Url2 = "HTTP://faceitbeta.azurewebsites.net/api/Celebrity/SearchFace";
        UserViewModel user = new UserViewModel();
        UserViewModel celebuser = new UserViewModel();

        HttpClient client = new HttpClient();
        private ObservableCollection<ProfileViewModel> data;
        private ObservableCollection<ProfileViewModel> celebdata;
        Face0 face = new Face0();
        MediaFile file;
        private UserViewModel user1;


        public FaceSearchPage(UserViewModel user1)
        {
            this.user1 = user1;
            InitializeComponent();
            file = null;

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

        private async void CelebSearchList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as ProfileViewModel;
            celebuser.UploaderID = item.UserId;

            var notifyUser = new UserViewModel
            {
                UploaderID = user1.UploaderID,
                Link = item.Link,
            };

            await Navigation.PushAsync(new CelebrityProfilePage(notifyUser));


        }


        private async void Button_UploadPhoto(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No camera", ":(No camera available.", "ok");
                return;
            }

                file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                }
                );

            if (file == null)
                return;

            PathLabel.Text = file.AlbumPath;

            if (file != null)
            {
                int message = await face.search(file);


                if (message == 0 && message == -1)
                {
                    string Message = "Can not Identify Any User";
                    Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                }
                else
                {
                    user.UploaderID = message;
                    try
                    {
                        var json = JsonConvert.SerializeObject(user);
                        HttpContent content = new StringContent(json);

                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        var response = await client.PostAsync(Url, content);

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


            MainImage.Source = ImageSource.FromStream(() =>
            {
                var Stream = file.GetStream();
                file.Dispose();
                return Stream;
            });
        }
  

        private async void Button_Galary(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("oops", "Gallery is not supported!", "ok");
                return;
            }

             file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            PathLabel.Text = "Photo Path" + file.Path;

            if (file != null)
            {
                int message = await face.search(file);


                if (message == 0 || message == -1)
                {
                    string Message = "Can not Identify Any User";
                    Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                }
                else
                {
                    user.UploaderID = message;
                    try
                    {
                        var json = JsonConvert.SerializeObject(user);
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
                        try
                        {
                            var celebjson = JsonConvert.SerializeObject(celebuser);
                            HttpContent celebcontent = new StringContent(celebjson);

                           celebcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                            var celebresponse = await client.PostAsync(Url2, celebcontent);

                            string s1 = celebresponse.StatusCode.ToString();
                            Toast.MakeText(Android.App.Application.Context, s1, ToastLength.Long).Show();

                            if (celebresponse.IsSuccessStatusCode)
                            {
                                string Message = "Searching Profiles....";
                                Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();

                                var celebget1 = celebresponse.Content.ReadAsStringAsync().Result;
                                var celebjson2 = JsonConvert.DeserializeObject<List<ProfileViewModel>>(celebget1);

                                celebdata = new ObservableCollection<ProfileViewModel>(celebjson2);
                                CelebSearchList.ItemsSource = celebdata;

                            }

                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                    catch (Exception)
                    {
                        string Message = "Registration Failed";
                        Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                    }

                }

            }

            MainImage.Source = ImageSource.FromStream(() =>
            {
                var Stream = file.GetStream();
                file.Dispose();
                return Stream;
            });

        }

        
    }
    }
