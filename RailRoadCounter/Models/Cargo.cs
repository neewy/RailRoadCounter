using System;
using System.Xml.Serialization;

namespace RailRoadCounter
{
    public class Cargo
    {
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