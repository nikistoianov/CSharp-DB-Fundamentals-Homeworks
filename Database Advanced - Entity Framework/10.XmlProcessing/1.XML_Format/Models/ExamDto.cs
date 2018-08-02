using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace _1.XML_Format.Models
{
    [XmlType("exam")]
    public class ExamDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("dateTaken")]
        public string DateTaken { get; set; }

        [XmlElement("grade")]
        public double Garde { get; set; }
    }
}
