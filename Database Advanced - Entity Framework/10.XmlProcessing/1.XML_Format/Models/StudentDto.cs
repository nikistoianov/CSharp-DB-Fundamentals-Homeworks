using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace _1.XML_Format.Models
{
    [XmlType("student")]
    public class StudentDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("gender")]
        public string Gender { get; set; }

        [XmlElement("birthDate")]
        public string BirthDate { get; set; }

        [XmlElement("phoneNumber")]
        public int PhoneNumber { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }

        [XmlElement("university")]
        public string University { get; set; }

        [XmlElement("specialty")]
        public string Specialty { get; set; }

        [XmlElement("facultyNumber")]
        public int FacultyNumber { get; set; }

        [XmlArray("exams")]
        public ExamDto[] Exams { get; set; }
    }
}
