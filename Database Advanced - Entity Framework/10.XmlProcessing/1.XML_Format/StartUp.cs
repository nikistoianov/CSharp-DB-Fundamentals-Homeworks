using _1.XML_Format.Models;
using Common.Generators;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace _1.XML_Format
{
    class StartUp
    {
        static void Main()
        {
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });
            GenerateStudentsXml(namespaces);
            GenerateCatalogXml(namespaces);
        }

        #region Students

        private static void GenerateStudentsXml(XmlSerializerNamespaces namespaces)
        {
            var serializer = new XmlSerializer(typeof(StudentDto[]), new XmlRootAttribute("students"));

            var students = GetStudents(3);

            using (TextWriter writer = new StreamWriter("../../../Students.xml"))
            {
                serializer.Serialize(writer, students, namespaces);
            }
        }

        private static StudentDto[] GetStudents(int count)
        {
            var result = new StudentDto[count];

            for (int i = 0; i < count; i++)
            {
                StudentDto student = GetStudent();
                result[i] = student;
            }

            return result;
        }

        private static StudentDto GetStudent()
        {
            var name = TextGenerator.FirstName;
            return new StudentDto()
            {
                Name = name,
                Gender = "Male",
                BirthDate = DateGenerator.PastDate.ToString("yyyy'/'MM'/'dd"),
                PhoneNumber = IntGenerator.GenerateInt(1000000, 9999999),
                Email = EmailGenerator.NewEmail(name),
                FacultyNumber = IntGenerator.GenerateInt(1000000, 9999999),
                Specialty = TextGenerator.Text("C# Web Developer", "Java Web Developer", "JavaScript FrontEnd Developer"),
                University = "SoftUni",
                Exams = GetExams(1)
            };
        }

        private static ExamDto[] GetExams(int count)
        {
            var result = new ExamDto[count];

            for (int i = 0; i < count; i++)
            {
                ExamDto exam = GetExams();
                result[i] = exam;
            }

            return result;
        }

        private static ExamDto GetExams()
        {
            return new ExamDto()
            {
                Name = TextGenerator.Text("C# Web", "DB Fundamentals", "OOP", "JS Core"),
                DateTaken = DateGenerator.RandomDate.ToString("yyyy'/'MM'/'dd"),
                Garde = 6
            };
        }

        #endregion

        private static void GenerateCatalogXml(XmlSerializerNamespaces namespaces)
        {
            // TODO
            //var serializer = new XmlSerializer(typeof(StudentDto[]), new XmlRootAttribute("catalog"));

            //var albums = GetAlbums(3);

            //using (TextWriter writer = new StreamWriter("../../../Catalog.xml"))
            //{
            //    serializer.Serialize(writer, albums, namespaces);
            //}
        }

    }
}
