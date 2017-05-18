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
        public String TonVAT
        { get; set; }

        [XmlElement("itogvag")]
        public String WagonVAT
        { get; set; }

        [XmlElement("rst")]
        public String Distance { get; set; }

        [XmlElement("sectnam")]
        public String Country { get; set; }

        [XmlElement("skidvag")]
        public String DiscountForWagon{ get; set; }

        [XmlElement("srokd")]
        public String DeliveryTime
        { get; set; }

        [XmlElement("sumohr")]
        public String Security { get; set; }

        [XmlElement("sumprov")]
        public String Conductors { get; set; }

        [XmlElement("sumt")]
        public String AmountForTon { get; set; }

        [XmlElement("sumvag")]
        public String AmountForWagon { get; set; }
    }
}
