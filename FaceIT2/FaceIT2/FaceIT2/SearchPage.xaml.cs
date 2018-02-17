using System;
using FaceIT2.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FaceIT2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        private UserViewModel user1;

        
        public SearchPage(UserViewModel user1)
        {
            this.user1 = user1;
            InitializeComponent();

        }

        private async void FaceSearch_Btn(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new FaceSearchPage(user1));
        }

        private async void LocationSearch_Btn(object sender, EventArgs e)
        {
          await Navigation.PushAsync(new LocationSearchPage());
        }

        private async void NameSearch_Btn(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new NameSearchPage(user1));
        }

      

        private async void ToolBar_Btn3(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new MainPage());

        }

    }
}