using System;
using System.Xml.Serialization;
using SQLite;

namespace RailRoadCounter
{

	public class Station
	{
		[PrimaryKey, AutoIncrement]
		[Indexed]
		public int Id { get; set; } = 0;

		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("code")]
		public string Code { get; set; }

		[XmlElement("dornam")]
		public string RoadName { get; set; }
	}

	[XmlRoot("xml")]
	public class StationXml {

		[XmlElement("STAN")]
		public Station[] Stations { get; set; }
	}
}
