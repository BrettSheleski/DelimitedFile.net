using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Sheleski.DelimitedFile.Tests
{
    [TestClass]
    public class CsvFile_EnumerableExtensions
    {
        [TestMethod]
        public void Enumerable_Headers_Works()
        {
            // Setup
            var people = new Person[]
            {
                new Person{FirstName = "Elvis", LastName = "Presley", BirthDate= new DateTime(1935, 1, 8), PrivateInfo="Dead"},
                new Person{FirstName = "Abraham ", LastName = "Lincoln", BirthDate= new DateTime(1809, 2, 12), PrivateInfo="Dead"},
            };
            string[] expectedHeaders = new string[] { "First Name", "Last Name", "Birth Date", "Death Date", "Children", "SomethingNull" };

            // Act
            var csvFile = people.ToCsvFile();
            var actualHeaders = csvFile.Headers.ToArray();

            // Verify
            Assert.AreEqual(expectedHeaders.Length, actualHeaders.Length);

            for (int i = 0; i < expectedHeaders.Length; ++i)
            {
                Assert.AreEqual(expectedHeaders[i], actualHeaders[i]);
            }
        }

        [TestMethod]
        public void Enumerable_Values_Works()
        {
            // Setup
            var people = new Person[]
            {
                new Person{FirstName = "Elvis", LastName = "Presley", BirthDate= new DateTime(1935, 1, 8), PrivateInfo="Dead"},
                new Person{FirstName = "Abraham ", LastName = "Lincoln", BirthDate= new DateTime(1809, 2, 12), PrivateInfo="Dead", DeathDate = new DateTime(1865, 4, 15)},
            };
            string[][] expectedValues = people.Select(p => new string[] { p.FirstName, p.LastName, p.BirthDate.ToString(), p.DeathDate?.ToString(), p.Children.ToString(), null }).ToArray();

            // Act
            var csvFile = people.ToCsvFile();
            string[][] actualValues = csvFile.Values.Select(row => row.ToArray()).ToArray();

            // Verify
            Assert.AreEqual(expectedValues.Length, actualValues.Length);

            for (int i = 0; i < expectedValues.Length; ++i)
            {
                Assert.AreEqual(expectedValues[i].Length, actualValues[i].Length);

                for (int j = 0; j < expectedValues[i].Length; ++j)
                {
                    Assert.AreEqual(expectedValues[i][j], actualValues[i][j]);
                }
            }
        }

        [TestMethod]
        public void Enumerable_CsvString()
        {
            // Setup
            var people = new Person[]
            {
                new Person{FirstName = "Elvis", LastName = "Presley", BirthDate = new DateTime(1935, 1, 8), PrivateInfo = "Dead"},
                new Person{FirstName = "Abraham", LastName = "Lincoln", BirthDate = new DateTime(1809, 2, 12), PrivateInfo = "Dead"},
            };
            string expectedCsvString = @"First Name,Last Name,Birth Date,Death Date,Children,SomethingNull
Elvis,Presley,1/8/1935 12:00:00 AM,,System.Collections.Generic.List`1[Sheleski.DelimitedFile.Tests.CsvFile_EnumerableExtensions+Person],
Abraham,Lincoln,2/12/1809 12:00:00 AM,,System.Collections.Generic.List`1[Sheleski.DelimitedFile.Tests.CsvFile_EnumerableExtensions+Person],";

            string actualCsvString;

            // Act
            var csvFile = people.ToCsvFile();
            using (var writer = new StringWriter())
            {
                csvFile.Write(writer, CsvFileOptions.WithHeaders.WithLineEnding("\r\n"));

                actualCsvString = writer.ToString();
            }

            // Verify
            Assert.AreEqual(expectedCsvString, actualCsvString);
        }

        [TestMethod]
        public void CustomMapping()
        {
            // Setup
            var people = new Person[]
            {
                new Person{FirstName = "Elvis", LastName = "Presley", BirthDate= new DateTime(1935, 1, 8), PrivateInfo="Dead"},
                new Person{FirstName = "Abraham", LastName = "Lincoln", BirthDate= new DateTime(1809, 2, 12), PrivateInfo="Dead", DeathDate = new DateTime(1865, 4, 15 )},
            };
            string[] expectedHeaders = new string[] { "B-Day", "LastName", "F Name", "DeathDate", "Row Index" };
            string expectedCsvString = @"B-Day,LastName,F Name,DeathDate,Row Index
1/8/1935,Presley,Elvis,,0
2/12/1809,Lincoln,Abraham,4/15/1865 12:00:00 AM,1";
            string actualCsvString;

            DelimitedFileObjectMapping<Person> mapping = new DelimitedFileObjectMapping<Person>();
            mapping.AddProperty(x => x.BirthDate).WithValue(d => d.ToShortDateString()).WithHeader("B-Day");
            mapping.AddProperty(x => x.LastName);
            mapping.AddProperty(x => x.FirstName).WithHeader("F Name");
            mapping.AddProperty(x => x.DeathDate);

            mapping.Add("Row Index", (x, i) => i.ToString());

            // Act
            var csvFile = people.ToCsvFile(mapping);
            string[] actualHeaders = csvFile.Headers.ToArray();

            using (var writer = new StringWriter())
            {
                csvFile.Write(writer, CsvFileOptions.WithHeaders.WithLineEnding("\r\n"));

                actualCsvString = writer.ToString();
            }


            // Verify
            Assert.AreEqual(expectedHeaders.Length, actualHeaders.Length);
            for (int i = 0; i < expectedHeaders.Length; ++i)
            {
                Assert.AreEqual(expectedHeaders[i], actualHeaders[i]);
            }

            Assert.AreEqual(expectedCsvString, actualCsvString);

        }

        [TestMethod]
        public void DelimitedFileToEnumerablePerson()
        {
            // setup
            DelimitedFile delimitedFile = new CsvFile();

            delimitedFile.Headers = new string[] { "BirthDay", "LastName", "FirstName", "DeathDate" };
            delimitedFile.Values = new string[][]{
                new string[]{ "1/8/1935", "Presley", "Elvis", null},
                new string[]{ "2/12/1809", "Lincoln", "Abraham", "4/15/1865 12:00:00 AM"}
            };

            // act
            List<Person> people = delimitedFile.Select<Person>().ToList();

            // verify
            Assert.AreEqual(2, people.Count);
        }

        class Person
        {
            [Display(Order = 1, Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Order = 2, Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Order = 3, Name = "Birth Date")]
            public DateTime BirthDate { get; set; }

            [Display(Order = 4, Name = "Death Date")]
            public DateTime? DeathDate { get; set; }

            [Display(Order = -1)]
            public string PrivateInfo { get; set; }

            [Display(Order = 4)]
            public List<Person> Children { get; } = new List<Person>();

            [Display(Order = 5)]
            public object SomethingNull { get; } = null;
        }
    }
}
