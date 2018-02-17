using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FaceIT2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using FaceIT2.ViewModels;
using Android.Widget;

namespace FaceIT2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RenewPasswordPage : ContentPage
    {
        private const string Url = "http://faceitbeta.azurewebsites.net/api/UserAccount/Code";

        HttpClient client = new HttpClient();
        UserViewModel user = new UserViewModel(); 
        private ForgotPasswordModel user1;
        
        public RenewPasswordPage(ForgotPasswordModel user1)
        {
            this.user1 = user1;
            InitializeComponent();
        }

        private async void Btn_Loggin(object sender, EventArgs e)
        {
            var password = Password.Text;
            var confirm = Confirm.Text;
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            var a = 0;

            if (password != null)
            {
                if (password.Length >= 6 || password.Length <= 12 & 
                    hasLowerChar.IsMatch(password) &
                    hasUpperChar.IsMatch(password) &
                    hasSymbols.IsMatch(password)

                    )
                {
                    a++;
                }
                else
                {
                    ErrorLabel1.Text = "Password is Not Valid";
                }
            }
            else
            {
                ErrorLabel1.Text = "Password Should Not be Empty";
            }

            if (confirm != null)
            {
                if (confirm == password)
                {
                    a++;
                }
                else
                {
                    ErrorLabel2.Text = "Re Enter Password";
                }
            }

            else
            {
                ErrorLabel2.Text = "Re Enter Password";
            }

            if (a == 2)
            {

                try
                {
                    var json = JsonConvert.SerializeObject(user1);
                    HttpContent content = new StringContent(json);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync(Url, content);

                    var get1 = response.Content.ReadAsStringAsync().Result;
                    var json2 = JsonConvert.DeserializeObject<UserViewModel>(get1);
                    user = json2;

                    string s = response.StatusCode.ToString();

                    if (s == "BadRequest")
                    {
                        Toast.MakeText(Android.App.Application.Context, "Invalid Login Credentials", ToastLength.Long).Show();

                    }
                    if (response.IsSuccessStatusCode)
                    {

                        string Message = "Password Reset Succesfull";
                        Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();

                        await Navigation.PushAsync(new MainPage());
                    }
                }

                catch (Exception)
                {
                    string Message = "Error Occured!";
                    Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                    // await Navigation.PushAsync(new MainPage());
                }

            }

        }
    
    }
}




