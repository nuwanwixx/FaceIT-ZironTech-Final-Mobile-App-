using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Net.Http;
using System.Collections.ObjectModel;
using FaceIT2.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Android.Widget;
//using Geolocator.Plugin;

namespace FaceIT2
{
    public partial class LocationSearchPage : ContentPage
    {
        public class userdetail
        {
            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }

        }

        private const string Url = "HTTP://faceitbeta.azurewebsites.net/api/Location/SearchLocation";
        
        HttpClient client = new HttpClient();
        private ObservableCollection<userdetail> _userdetail;
        Xamarin.Forms.Maps.Geocoder geoCoder; //create a new instance ow geocoder class
        Xamarin.Forms.Maps.Geocoder geocode;

        public LocationSearchPage()
        {
            geoCoder = new Xamarin.Forms.Maps.Geocoder();
            geocode = new Xamarin.Forms.Maps.Geocoder();
            InitializeComponent();
        }
        
        private async void btnSearchLocation_Clicked(object sender, EventArgs e)
        {

            var address = Place.Text;
            var approximateLocations1 = await geoCoder.GetPositionsForAddressAsync(address);
            foreach (var position in approximateLocations1)
            {
                var position2 = position.Latitude + ", " + position.Longitude + "\n";
                var maxlat = position.Latitude + 1;
                var maxlon = position.Longitude + 1;
                var minlat = position.Latitude - 1;
                var minlon = position.Longitude - 1;
                txtLat.Text = "Latitude: " + position.Latitude.ToString();
                txtLong.Text = "Longitude: " + position.Longitude.ToString();
                txtAddress.Text = "Address:" + address;
                MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude),
              Distance.FromMiles(1)));



                var pin = new Pin
                {
                    Position = new Position(position.Latitude, position.Longitude),
                    Label = "Searched Location",
                    Address = (Place.Text)


                };

                MainMap.Pins.Add(pin);

                Place.Text = null;


                var user = new SearchUserBindingModel
                {
                    MaxLatitude = maxlat,
                    MaxLongitude = maxlon,
                    MinLatitude = minlat,
                    MinLongitude = minlon,

                };
                try
                {
                    var json = JsonConvert.SerializeObject(user);
                    HttpContent content = new StringContent(json);

                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync(Url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string Message = "Location Searched Successfull";
                        Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();

                        var get1 = response.Content.ReadAsStringAsync().Result;
        
                        var json2 = JsonConvert.DeserializeObject<List<userdetail>>(get1);
                        _userdetail = new ObservableCollection<userdetail>(json2);
                        MainListView.ItemsSource = _userdetail;
                    }

                }
                catch (Exception)
                {
                    string Message = "Location Searching Failed";
                    Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();

                }
                
            }

        }

    }
}

