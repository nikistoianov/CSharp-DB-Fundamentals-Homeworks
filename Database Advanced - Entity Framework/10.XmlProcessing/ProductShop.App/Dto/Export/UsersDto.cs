namespace ProductShop.App.Dto.Export
{
    using System.Xml.Serialization;

    [XmlRoot("users")]
    public class UsersDto
    {
        [XmlAttribute("count")]
        public int Count { get; set; }

        [XmlArray("user")]
        public UserSoldDto[] Users { get; set; }
    }
}
