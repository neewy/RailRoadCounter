using System;
namespace RailRoadCounter
{
	public class DownloadCheker
	{
		public static Boolean IsStationDownloadedByName(string name)
		{
			var firstLetter = name.Trim()[0];
			return App.Current.Properties.ContainsKey($"stationSavedByName{firstLetter}");

		}

		public static Boolean IsCargoDownloadedByName(string name)
		{
			var firstLetter = name.Trim()[0];
			return App.Current.Properties.ContainsKey($"cargoSavedByName{firstLetter}");
		}

		public static Boolean IsStationDownloadedByCode(string code)
		{
			var firstLetter = code.Trim()[0];
			return App.Current.Properties.ContainsKey($"stationSavedByCode{firstLetter}");

		}

		public static Boolean IsCargoDownloadedByCode(string code)
		{
			var firstLetter = code.Trim()[0];
			return App.Current.Properties.ContainsKey($"cargoSavedByCode{firstLetter}");
		}
	}
}
