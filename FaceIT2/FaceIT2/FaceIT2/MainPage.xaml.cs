using System;
using Xamarin.Forms;

namespace FaceIT2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            image1.Source = ImageSource.FromResource("FaceIT2.Images.background.jpg");
        }
        
      
        async void Button_SignUp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage2());
        }

        async void Button_SignIn(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new SignInPage());
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
