using Android.Widget;
using FaceIT2.Models;
using FaceIT2.ViewModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FaceIT2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage2 : ContentPage
    {
        ProfileViewModel profile = new ProfileViewModel();
        
        public SignUpPage2()
        {            
            InitializeComponent();
        }

        private const string Url = "HTTP://faceitbeta.azurewebsites.net/api/UserAccount/Register/";
        HttpClient client = new HttpClient();

        async private void Button_CreateAcount(object sender, EventArgs e)
        {
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;
            var fname = NameEntry.Text;
            var lname = LNameEntry.Text;
            var confirmpassword = ConfirmPassword.Text;
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
            
            if(lname != null)
            {
                a++;
            }
            else
            {
                ErrorLabel9.Text = "Last name should not be Empty";
            }

            if (email != null)
            {
                if (email.Contains("@") & email.Contains(".com"))
                {

                    a++;
                }
                else
                {
                    ErrorLabel.Text = "Email is not valid";
                  
                }
            }

            else
            {
                ErrorLabel.Text = "Email Should Not be Empty";
            }

            if (password != null)
            {

                if (password.Length >= 6 || password.Length <=12 &
                    hasLowerChar.IsMatch(password) &
                    hasUpperChar.IsMatch(password) &
                    hasSymbols.IsMatch(password) )
                {
                    a++;
                }
                else
                {
                    ErrorLabel2.Text = "Password is not valid";
                  
                }

            }
            else
            {
                ErrorLabel2.Text = "Password Should Not be Empty";
            }
            
            if (confirmpassword != null)
            {
                if (confirmpassword == password)
                {
                    a++;
                }

                else
                {
                    ErrorLabel4.Text = "Re Enter Password";
                    confirmpassword = null;
                }
            }
            else
            {
                ErrorLabel4.Text = "Re Enter Password";
            }

            if(address !=null)
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
                    !hasSymbols.IsMatch(contactNo))
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

            if (a == 9)
            {
                UserViewModel imgId = new UserViewModel();
                var user = new RegisterBindingModel
                {
                    FirstName = NameEntry.Text,
                    LastName = LNameEntry.Text,
                    Email = EmailEntry.Text,
                    Password = PasswordEntry.Text,
                    Description = DescriptionEntry.Text,
                    Address = AddressEntry.Text,
                    Gender = GenderEntry.Text,
                    LocationPermission = true,
                    MobileNumber = ContactNumber.Text,    
                };
                try
                {
                    var json = JsonConvert.SerializeObject(user);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                  
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync(Url, content);

                    
                    string s = response.StatusCode.ToString();
                    if (s == "NotFound")
                    {
                        string Message = "Email Already Exists";
                        Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                    }
                    else if (s == "BadRequest")
                    {
                        string Message = "Error!! Try Again";
                        Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                    }
                  
                    if (response.IsSuccessStatusCode)
                    {
                        string Message = "Registration Successfull";
                        Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();

                        var get1 = response.Content.ReadAsStringAsync().Result;
                        
                        var json2 = JsonConvert.DeserializeObject<UserViewModel>(get1);
                        imgId = json2;
                        
                        await Navigation.PushAsync(new SignUpPage(imgId));

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
}