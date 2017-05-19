using System;
using System.Xml.Serialization;
using SQLite;

namespace RailRoadCounter
{
    public class Cargo
    {

		[PrimaryKey, AutoIncrement]
		[Indexed]
		public int Id { get; set; } = 0;

        [XmlElement("code")]
        public int Code { get; set; }

        [XmlElement("ohr")]
        public string Guarded
        { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("opas")]
        public string Dangerous
        { get; set; }

    }

    [XmlRoot("xml")]
    public class CargoXml
    {
        [XmlElement("GRUZ")]
        public Cargo[] CargoList { get; set; }
    }
}