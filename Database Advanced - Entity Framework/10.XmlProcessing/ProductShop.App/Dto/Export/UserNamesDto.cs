namespace ProductShop.App.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("user")]
    public class UserNamesDto
    {
        [XmlAttribute("first-name")]
        public string FirstName { get; set; }

        [XmlAttribute("last-name")]
        public string LastName { get; set; }

        [XmlArray("sold-products")]
        public ProductDto[] SoldProducts { get; set; }
    }
}
