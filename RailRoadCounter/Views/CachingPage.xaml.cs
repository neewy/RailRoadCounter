using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace RailRoadCounter
{
	public partial class CachingPage : ContentPage, IObserver<Message>
	{

		private IDisposable _disposable;

		public CachingPage()
		{
			InitializeComponent();
			var dbHelper = new DataRetrievalHelper();
			_disposable = dbHelper.Subscribe(this);
			dbHelper.GetAndSaveAll().ConfigureAwait(false);
		}

		public void OnCompleted()
		{
			this._disposable.Dispose();
			Device.BeginInvokeOnMainThread(async () =>
				{
					await Navigation.PopModalAsync();
				});
		}

		public void OnError(Exception error)
		{
			throw new NotImplementedException();
		}

		public void OnNext(Message value)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				CountMessage.Text = $"Обновлено {(int)value.Prgs} записей";
			});
		}

		// блокируем нажатие кнопки назад на Android устройствах
		protected override bool OnBackButtonPressed()
		{
			return true;
		}
	}
}
