using System;
using System.Xml.Serialization;

namespace RailRoadCounter
{

	public class Station
	{
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
