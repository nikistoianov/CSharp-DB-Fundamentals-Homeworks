namespace CarDealer.App.Dto.Import
{
    using System.Xml.Serialization;

    [XmlType("car")]
    public class CarDto
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("travelled-distance")]
        public double TravelledDistance { get; set; }
    }
}
