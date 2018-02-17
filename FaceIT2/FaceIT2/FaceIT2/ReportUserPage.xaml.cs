using Android.Widget;
using FaceIT2.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FaceIT2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportUserPage : ContentPage
    {
        ReportUserBindingModel  detail = new ReportUserBindingModel();
        public ReportUserPage()
        {
            InitializeComponent();
        }

        bool t = false;
        string Message;
        private void Button_Submit(object sender, EventArgs e)
        {
            if (t == false)
            {
                if (Report.Text == null)
                {
                    ErrorLabel.Text = "Descrption Can't Be Null";
                    return;
                }
                detail.ReportDetail = Report.Text;

                 Message = "Submit Success";
                Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();
                
            }
             Message = "Submit Failed! Cannot Submit More than one report!";
            Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Long).Show();

            t = true;

        }
    }
}