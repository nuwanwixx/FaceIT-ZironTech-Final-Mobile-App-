using FaceIT2.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FaceIT2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgetPasswordPage : ContentPage
    {
        private const string Url = "http://faceitbeta.azurewebsites.net/api/UserAccount/ForgotPassword";
        ForgotPasswordModel user1 = new ForgotPasswordModel();
       
        HttpClient client = new HttpClient();
        public ForgetPasswordPage()
        {
            InitializeComponent();
        }


        private async void Btn_submit(object sender, EventArgs e)
        {
            var email = Email.Text;

            if (email != null)
            {
                if (email.Contains("@") & email.Contains(".com"))
                {
                    try
                    {
                        var send = JsonConvert.SerializeObject(user1);
                        HttpContent content = new StringContent(send);

                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        var content2 = await client.PostAsync(Url, content);

                        string Message = content2.StatusCode.ToString();
                        Android.Widget.Toast.MakeText(Android.App.Application.Context, Message, Android.Widget.ToastLength.Long).Show();

                        if (content2.IsSuccessStatusCode)
                        {
                            await Navigation.PushAsync(new RenewPasswordPage(user1));
                        }

                    }

                    catch (Exception)
                    {
                        string Message = "Error Occured";
                        Android.Widget.Toast.MakeText(Android.App.Application.Context, Message, Android.Widget.ToastLength.Long).Show();
                        
                    }
                }
                else
                {
                    ErrorLabel1.Text = "Email is not valid";
                }
            }

            else
            {
                ErrorLabel1.Text = "Email Should Not be Empty";
            }

        }
        
    }
}



