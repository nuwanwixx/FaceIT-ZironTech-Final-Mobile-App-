using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FaceIT2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : MasterDetailPage
    {
        public MasterPage()
        {
            InitializeComponent();
            InitializeComponent();
            new NavigationPage(new ProfilePage());
            IsPresent = true;

        }



        private void Notofication_Btn(object sender, EventArgs e)
        {
            new NavigationPage(new NotificationPage());
            IsPresent = false;
        }

        private void EditProfile_Btn(object sender, EventArgs e)
        {
            new NavigationPage(new EditProfile());
            IsPresent = false;
        }

        private void Settings_Btn(object sender, EventArgs e)
        {
            new NavigationPage(new SettingsPage());
            IsPresent = false;
        }

        private void SignOut_Btn(object sender, EventArgs e)
        {
            new NavigationPage(new SignOutPage());
            isPresent = false;
        }
    }
}