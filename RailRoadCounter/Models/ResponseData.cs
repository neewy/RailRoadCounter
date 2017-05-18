using System;
using System.Xml.Serialization;

namespace RailRoadCounter.Models
{

    [XmlRoot("xml")]
    public class Response
    {
        [XmlElement("tar_response")]
        public TarResponse TarResponse { get; set; }

    }

    public class TarResponse
    {
        [XmlElement("section")]
        public ResponseData ResponseData { get; set; }
    }
    public class ResponseData
    {
        [XmlElement("itogt")]
        public String itogt
        { get; set; }

        [XmlElement("itogvag")]
        public String itogvag
        { get; set; }

        [XmlElement("rst")]
        public String rst { get; set; }

        [XmlElement("sectnam")]
        public String country { get; set; }

        [XmlElement("skidvag")]
        public String skidvag { get; set; }

        [XmlElement("srokd")]
        public String srokd { get; set; }

        [XmlElement("sumohr")]
        public String sumohr { get; set; }

        [XmlElement("sumprov")]
        public String sumprov { get; set; }

        [XmlElement("sumt")]
        public String sumt { get; set; }

        [XmlElement("sumvag")]
        public String sumvag { get; set; }
    }
}
