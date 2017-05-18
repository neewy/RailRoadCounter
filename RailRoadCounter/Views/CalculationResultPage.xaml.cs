using RailRoadCounter.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RailRoadCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalculationResultPage : ContentPage
    {
        public ResponseData ResponseData { get; set; }

        public CalculationResultPage(ResponseData responseData)
        {
            ResponseData = responseData;
            InitializeComponent();
            String icon = "IconCheck.png";
            BindingContext = ResponseData;
            ToolbarItems.Add(new ToolbarItem
            {
                Icon = icon,
                Command = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PopModalAsync();
                    });

                }),
            });
        }
    }
}