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
		public int Prgs { get; set; }
	}

	public class DataRetrievalHelper : IObservable<Message>
	{
		private static StationService _stationService;
		private static CargoService _cargoService;
		private List<IObserver<Message>> _observers;

		private static int ProgressCount { get; set; }

		public DataRetrievalHelper()
		{
			_stationService = new StationService(App.Database.sqlite);
			_cargoService = new CargoService(App.Database.sqlite);
			_observers = new List<IObserver<Message>>();
		}

		public async Task GetAndSaveAll()
		{
			var task1 = GetAllStations();
			var task2 = GetAllCargo();

			await Task.WhenAll(task1, task2).ContinueWith((arg) =>
						{
							if (arg.IsCompleted)
							{
								EndTransmission();
							}
						});
		}

		private async Task GetAllStations()
		{
			char[] az = Enumerable.Range('а', 'я' - 'а' + 1).Select(i => (Char)i).ToArray();

			foreach (var c in az)
			{
				await GetAndSaveStationsByName(c);
				ProgressCount++;
				SendMessage((new Message { Prgs = ProgressCount }));
			}
		}

		private async Task GetAllCargo()
		{
			char[] az = Enumerable.Range('а', 'я' - 'а' + 1).Select(i => (Char)i).ToArray();
			foreach (var c in az)
			{
				await GetAndSaveCargoByName(c);
				ProgressCount++;
				SendMessage((new Message { Prgs = ProgressCount }));
			}
		}

		public async Task GetAndSaveStationsByName(char firstLetter)
		{
			var request = HttpConnector.CreateGetConnection(new Uri($"http://tarifgd.ru/tar_online2/getstan.php?buk={firstLetter}&poiskvh=1&pp=*&view=xml"));

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
					}
					Application.Current.Properties.Add("stationSavedByName" + firstLetter, 1);
					await Application.Current.SavePropertiesAsync().ConfigureAwait(false);
				}
			}
		}

		public async Task GetAndSaveCargoByName(char firstLetter)
		{
			var request = HttpConnector.CreateGetConnection(new Uri($"http://tarifgd.ru/tar_online2/getgruz.php?buk={firstLetter}&poiskvh=1&view=xml"));

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
					}
					Application.Current.Properties.Add("cargoSavedByName" + firstLetter, 1);
					await Application.Current.SavePropertiesAsync().ConfigureAwait(false);
				}
			}
		}

		public async Task GetAndSaveStationsByCode(char firstDigit)
		{
			var request = HttpConnector.CreateGetConnection(new Uri($"http://tarifgd.ru/tar_online2/getstan.php?cod={firstDigit}&poiskvh=1&pp=*&view=xml"));

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
					}
					Application.Current.Properties.Add("stationSavedByCode" + firstDigit, 1);
					await Application.Current.SavePropertiesAsync().ConfigureAwait(false);
				}
			}
		}

		public async Task GetAndSaveCargoByCode(char firstDigit)
		{
			var request = HttpConnector.CreateGetConnection(new Uri($"http://tarifgd.ru/tar_online2/getgruz.php?cod={firstDigit}&poiskvh=1&view=xml"));

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
					}
					Application.Current.Properties.Add("cargoSavedByCode" + firstDigit, 1);
					await Application.Current.SavePropertiesAsync().ConfigureAwait(false);
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
