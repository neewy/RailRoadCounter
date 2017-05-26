using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace RailRoadCounter
{
	public partial class InfoPage : ContentPage
	{
		public InfoPage()
		{
			InitializeComponent();
			ToolbarItems.Add(new ToolbarItem
			{
				Icon = "IconCheck.png",
				Command = new Command(() =>
				{
					Device.BeginInvokeOnMainThread(async() =>
					{
						await Navigation.PopModalAsync();
					});

				}),
			});
		}

		void Handle_Clicked(object sender, System.EventArgs e)
		{
			Navigation.PushModalAsync(new CachingPage());
		}
	}
}
