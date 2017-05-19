using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace RailRoadCounter
{
	public struct Message
	{
		public double Prgs { get; set; }
	}

	public class DataRetrievalHelper : IObservable<Message>
	{
		private static StationService _stationService;
		private static CargoService _cargoService;
		private List<IObserver<Message>> _observers;

		private static double ProgressCount { get; set; }
		private static double Progress { get { return (100.0 / 10567.0) * ProgressCount; } }

		public DataRetrievalHelper()
		{
			_stationService = new StationService(App.Database.sqlite);
			_cargoService = new CargoService(App.Database.sqlite);
			_observers = new List<IObserver<Message>>();
		}

		public async Task InitDb()
		{
			await App.Database.CreateInitialDatabase();

			var task1 = GetStations();
			var task2 = GetCargo();

			await Task.WhenAll(task1, task2).ContinueWith((arg) => {
				if (arg.IsCompleted)
				{
					EndTransmission();
				}
			});

			Application.Current.Properties.Add("firstRun", 1);
			//Сохранение отметки первого запуска в свойствах приложения
			await Application.Current.SavePropertiesAsync().ConfigureAwait(false);
		}

		private async Task GetStations()
		{
			char[] az = Enumerable.Range('а', 'я' - 'а' + 1).Select(i => (Char)i).ToArray();

			foreach (var c in az)
			{
				var request = HttpConnector.CreateGetConnection(new Uri($"http://tarifgd.ru/tar_online2/getstan.php?buk={c}&poiskvh=1&pp=*&view=xml"));

				var response = await HttpConnector.Client.SendAsync(request);
				if (response.IsSuccessStatusCode)
				{

					var xmlResponse = await response.Content.ReadAsStreamAsync();
					var stationSerializer = new XmlSerializer(typeof(StationXml));

					var stationsXml = (StationXml)stationSerializer.Deserialize(xmlResponse);

					if (stationsXml.Stations != null)
					{
						foreach (var station in stationsXml.Stations)
						{
							await _stationService.Save(station);
							ProgressCount++;
							SendMessage((new Message { Prgs = Progress }));
						}
					}
				}
			}
		}

		private async Task GetCargo()
		{
			char[] az = Enumerable.Range('а', 'я' - 'а' + 1).Select(i => (Char)i).ToArray();
			foreach (var c in az)
			{
				var request = HttpConnector.CreateGetConnection(new Uri($"http://tarifgd.ru/tar_online2/getgruz.php?buk={c}&poiskvh=1&view=xml"));

				var response = await HttpConnector.Client.SendAsync(request);
				if (response.IsSuccessStatusCode)
				{

					var xmlResponse = await response.Content.ReadAsStreamAsync();
					var cargoSerializer = new XmlSerializer(typeof(CargoXml));

					var cargoXml = (CargoXml)cargoSerializer.Deserialize(xmlResponse);

					if (cargoXml.CargoList != null)
					{
						foreach (var cargo in cargoXml.CargoList)
						{
							await _cargoService.Save(cargo);
							ProgressCount++;
							SendMessage((new Message { Prgs = Progress }));
						}
					}
				}
			}
		}

		public IDisposable Subscribe(IObserver<Message> observer)
		{
			if (!_observers.Contains(observer))
				_observers.Add(observer);
			return new Unsubscriber(_observers, observer);
		}

		public void SendMessage(Nullable<Message> loc)
		{
			foreach (var observer in _observers)
			{
				if (!loc.HasValue)
					observer.OnError(new MessageUnknownException());
				else
					observer.OnNext(loc.Value);
			}
		}

		public void EndTransmission()
		{
			foreach (var observer in _observers.ToArray())
				if (_observers.Contains(observer))
					observer.OnCompleted();

			_observers.Clear();
		}
	}
}
