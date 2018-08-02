namespace ProductShop.App.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("category")]
    public class CategoryByProductDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("product-count")]
        public int ProductCount { get; set; }

        [XmlElement("average-price")]
        public decimal AveragePrice { get; set; }

        [XmlElement("total-revenue")]
        public decimal TotalRevenue { get; set; }
    }
}
