using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FaceIT2.ViewModels;
using FaceIT2.Models;
using System.Net.Http.Headers;
using Android.Widget;

namespace FaceIT2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        private const string Url = "HTTP://faceitbeta.azurewebsites.net/api/UserAccount/Login/";
        
        UserViewModel user = new UserViewModel();
        HttpClient client = new HttpClient();

        public SignInPage()
        {
            InitializeComponent();            
        }

       async void Button_Login(object sender, EventArgs e)
        {
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;
            var a = 0;
            if (email.Contains("@") & email.Contains(".com"))
            {
                a++;                
            }

            else {
                ErrorLabel.Text = "Email is not valid";
            }
                       
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            

            if (password != null)
            {
                if (password.Length >= 6  || password.Length <=12 &
                    hasLowerChar.IsMatch(password) &
                    hasUpperChar.IsMatch(password) &
                    hasSymbols.IsMatch(password) )
                {
                    a++;
                }
                else
                {
                    ErrorLabel2.Text = "Password is Not Valid";
                }
            }
            else
            {
                ErrorLabel2.Text = "Password Should Not be Empty";
            }
            if (a==2)

            {
                var model = new LoginBindingModel
                {

                    Email = EmailEntry.Text,
                    Password = PasswordEntry.Text,

                };

                try
                {
                    var json = JsonConvert.SerializeObject(model);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
             
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync(Url, content);

                    var get1 = response.Content.ReadAsStringAsync().Result;
             
                    var json2 = JsonConvert.DeserializeObject<UserViewModel>(get1);
                    user = json2;

                    string s = response.StatusCode.ToString();

                    if (s == "BadRequest")
                    {
                        Toast.MakeText(Android.App.Application.Context,"Invalid Login Credentials", ToastLength.Long).Show();

                    }
                    

                    if (response.IsSuccessStatusCode)
                    {
                        
                        string Message = "Login Successfull ";
                        Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                        
                        await Navigation.PushAsync(new ProfilePage(user));
                    }
                   
                }
                catch (Exception)
                {
                         
                    string Message = "Login Failed! Connect to Internet ";
                    Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                    await Navigation.PushAsync(new MainPage());
                }
                
            }

        }


        async private void Button_ForgetPassword(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgetPasswordPage());
        }


    }
}