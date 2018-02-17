using System;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Android.Widget;
using FaceIT2.ViewModels;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using FaceIT2.Models;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using FaceAPIFunctions;

namespace FaceIT2
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        private const string Url = "HTTP://faceitbeta.azurewebsites.net/api/UserAccount/UserPhoto/";
        private const string Url1 = "HTTP://faceitbeta.azurewebsites.net/api/Location/SetLocation/";
        HttpClient client = new HttpClient();
        Geocoder geoCoder; //create a new instance ow geocoder class
        Face0 face = new Face0();
        private UserViewModel user;

        public SignUpPage(UserViewModel imgId)
        {
            this.user = imgId;
            geoCoder = new Geocoder();

            InitializeComponent();
        }

        private async void Button_UploadPhoto(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No camera", ":(No camera available.", "ok");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                }
                );

            if (file == null)
                return;
            int s = await face.searchFirst(file);

            if (s != 3)
            {
                Toast.MakeText(Android.App.Application.Context, s.ToString(), ToastLength.Long).Show();
                return;
            }
            var id = user.UploaderID;
            
            string s2 = await face.register(file, id);
            if (s2 != "Success")
            {
                Toast.MakeText(Android.App.Application.Context, s2, ToastLength.Long).Show();
                return;
            }
 
            DateTime dTime = DateTime.Now;
            string time = dTime.ToString();
            time = time.Replace(" ", "_") + ".jpg";
            
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=faceitphotos;AccountKey=67nq3VNJlZ0KJArJZU62vjri4pNzqd1MERWFQytw7w7B6cfTv7Gw75iJq4LJgUN7E05Y0+3ixmkOWDyKk4yhtw==");
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("userimages");
 
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(id + "_" + time);
            using (Stream photo = file.GetStream())
            {
                await blockBlob.UploadFromStreamAsync(photo);
            }

            MainImage.Source = ImageSource.FromStream(() =>
            {
                var Stream = file.GetStream();
                file.Dispose();
                return Stream;
            });


            if (id != 0)
            {
                UserPhotoBindingModel photoModel = new UserPhotoBindingModel();

                photoModel.Link = "https://faceitphotos.blob.core.windows.net/userimages/" + id + "_" + time;
                photoModel.UploaderID = id;

                try
                {
                    var json = JsonConvert.SerializeObject(photoModel);
                    HttpContent content = new StringContent(json);

                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync(Url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string Message = "Registration Successfull";
                        Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();

                        var get1 = response.Content.ReadAsStringAsync().Result;
 
                        var json2 = JsonConvert.DeserializeObject<UserViewModel>(get1);
                        user = json2;

                        Toast.MakeText(Android.App.Application.Context, "Uploading Image... ", ToastLength.Long).Show();
                        
                    }
                }
                catch (Exception)
                {
                    string Message = "Registration Failed";
                    Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                    
                }

            }

        }


        private async void Button_Galary(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("oops", "Gallery is not supported!", "ok");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;


            PathLabel.Text = file.Path;
            int s = await face.searchFirst(file);

            if (s != 3)
            {
             
                Toast.MakeText(Android.App.Application.Context, s.ToString(), ToastLength.Long).Show();
                return ;
            }
            
           
            var id = user.UploaderID;

            //
            string s2= await face.register(file, id);
            if (s2 != "Success")
            {
                Toast.MakeText(Android.App.Application.Context, s2, ToastLength.Long).Show();
                return;
            }
          

            DateTime dTime = DateTime.Now;
            string time = dTime.ToString();
            time = time.Replace(" ", "_") + ".jpg";
            
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=faceitphotos;AccountKey=67nq3VNJlZ0KJArJZU62vjri4pNzqd1MERWFQytw7w7B6cfTv7Gw75iJq4LJgUN7E05Y0+3ixmkOWDyKk4yhtw==");
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("userimages");
          
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(id + "_" + time);
            using (Stream photo = file.GetStream())
            {
                await blockBlob.UploadFromStreamAsync(photo);
            }

            MainImage.Source = ImageSource.FromStream(() =>
            {
                var Stream = file.GetStream();
                file.Dispose();
                return Stream;
            });


            if (id != 0)
            {
                UserPhotoBindingModel photoModel = new UserPhotoBindingModel();

                photoModel.Link = "https://faceitphotos.blob.core.windows.net/userimages/" + id + "_" + time;
                photoModel.UploaderID = id;

                try
                {
                    var json = JsonConvert.SerializeObject(photoModel);
                    HttpContent content = new StringContent(json);

                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync(Url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string Message = "Registration Successfull";
                        Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();

                        var get1 = response.Content.ReadAsStringAsync().Result;
          
                        var json2 = JsonConvert.DeserializeObject<UserViewModel>(get1);
                        user = json2;

                        Toast.MakeText(Android.App.Application.Context, "Uploading Image... ", ToastLength.Long).Show();

          
                    }
                }
                catch (Exception)
                {
                    string Message = "Registration Failed";
                    Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                    
                }

            }
            
        }
        
        async private void Button_Next(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage(user));
        }

        private async void Button_SetLocation(object sender, EventArgs e)
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var location = await locator.GetPositionAsync();

            Xamarin.Forms.Maps.Position position = new Xamarin.Forms.Maps.Position(location.Latitude, location.Longitude);
            var position1 = new Position(position.Latitude, position.Longitude);
            var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position1);
            foreach (var address in possibleAddresses)
                txtAddress.Text += address + "\n";
            
            var location2 = new SetLocationBindingModel
            {
                UserID = user.UploaderID,
                Latitude = position.Latitude,
                Longitude = position.Longitude,

            };
            try
            {
                var json = JsonConvert.SerializeObject(location2);
                HttpContent content = new StringContent(json);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(Url1, content);
                if (response.IsSuccessStatusCode)
                {
                    string Message = "Location Set successfully";
                    Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                    
                }

            }
            catch (Exception)
            {
                string Message = "Location Setting failed";
                Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
            }
        }
    }
}
