using System;
using System.Text;
using FaceIT2.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using FaceIT2.Models;
using Newtonsoft.Json;
using System.Net.Http;
using Android.Widget;
using System.Net.Http.Headers;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;

namespace FaceIT2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPage : ContentPage
    {
        Geocoder geoCoder; //create a new instance ow geocoder class
        private UserViewModel user1;
        private const string Url = "HTTP://faceitbeta.azurewebsites.net/api/UserAccount/EditProfile";
        private const string Url1 = "HTTP://faceitbeta.azurewebsites.net/api/Location/UpdateLocation";
        HttpClient client = new HttpClient();
        UserViewModel profile = new UserViewModel();
       
        public EditPage(UserViewModel user1)
        {
            this.user1 = user1;
            geoCoder = new Geocoder();
            InitializeComponent();
        }

        private async void Button_Submit(object sender, EventArgs e)
        {
            
            var fname = NameEntry.Text;
            var lname = LNameEntry.Text;
            var contactNo = ContactNumber.Text;
            var gender = GenderEntry.Text;
            var address = AddressEntry;
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            var hasNumber = new Regex(@"[0-9]+");
            
            var a = 0;
       
            if (fname != null)
            {

                a++;
            }

            else
            {
                ErrorLabel3.Text = "First Name Should Not be Empty";
            }



            if (lname != null)
            {
                a++;
            }

            else
            {
                ErrorLabel9.Text = "Last name should not be Empty";
            }
            
            if (address != null)
            {
                a++;
            }

            else
            {
                ErrorLabel8.Text = "Address Should not be Empty";
            }

            if (contactNo != null)
            {
                if (contactNo.Length <= 10 &
                    hasNumber.IsMatch(contactNo) &
                    !hasLowerChar.IsMatch(contactNo) &
                    !hasUpperChar.IsMatch(contactNo) &
                    !hasSymbols.IsMatch(contactNo)
                    )
                {
                    a++;
                }

                else
                {
                    ErrorLabel5.Text = "The Number You Entered is Not Valid";

                }
            }

            else
            {
                ErrorLabel5.Text = "Contact Number Should Not be Empty";
            }

            if (gender != null)
            {
                if (gender.Contains("Male") || gender.Contains("Female"))
                {
                    a++;
                }

                else
                {
                    ErrorLabel6.Text = "Enter Male or Female";
                }
            }
            else
            {
                ErrorLabel6.Text = "Gender Should Not Be Empty";
            }


            if (DescriptionEntry.Text != null)
            {
                a++;
            }
            else
            {
                ErrorLabel7.Text = "Description should Not Be Empty";
            }

            if (a == 6)
            {
                var user = new RegisterBindingModel
                {
                    UserId = user1.UploaderID,
                    FirstName = NameEntry.Text,
                    LastName = LNameEntry.Text, 
                    Address = AddressEntry.Text,
                    Gender = GenderEntry.Text,
                    MobileNumber = ContactNumber.Text,
                    Description = DescriptionEntry.Text,
                    //LocationPermission = true,
                };
                try
                {
                    var json = JsonConvert.SerializeObject(user);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync(Url, content);

                    string s = response.StatusCode.ToString();

                   
                    Toast.MakeText(Android.App.Application.Context, s , ToastLength.Long).Show();

                   

                    if (response.IsSuccessStatusCode)
                    {
                        string Message = "Registration Successfull";
                        Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();

                        var get1 = response.Content.ReadAsStringAsync().Result;
                        
                        var json2 = JsonConvert.DeserializeObject<UserViewModel>(get1);
                        profile = json2;
                        await Navigation.PushAsync(new ProfilePage(profile));
                    }

                }
                catch (Exception)
                {
                    string Message = "Registration Failed";
                    Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                    await Navigation.PushAsync(new MainPage());
                }
                
            }

        }

        private async void Button_UpdateLocation(object sender, EventArgs e)
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
                 UserID = user1.UploaderID,
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