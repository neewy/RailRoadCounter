using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RailRoadCounter
{
	public class RequestData : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		private Station _departureStation;
		public Station DepartureStation { get { return _departureStation; } set { _departureStation = value; OnPropertyChanged("DepartureStation"); } }
		public Station ArrivalStation { get; set; }

		public Cargo Cargo { get; set; }

		public double DepartureWeight { get; set; }
		public int NumOfWagons { get; set; }
		public int NumOfAxis { get; set; }
		public int NumOfGuardedWagons { get; set; }
		public int NumOfConductors { get; set; }

	}
}
