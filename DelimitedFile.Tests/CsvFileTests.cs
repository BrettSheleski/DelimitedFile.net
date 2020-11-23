using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using System.Xml.Linq;

namespace Sheleski.DelimitedFile.Tests
{
    [TestClass]
    public class CsvFileTests
    {
        [TestMethod]
        public async Task CsvFile_Load_Works()
        {
            // Setup
            WebRequest request = HttpWebRequest.Create("https://raw.githubusercontent.com/forxer/languages-list/master/src/Languages.csv");
            using (WebResponse response = await request.GetResponseAsync())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {

                // Act
                var csvFile = CsvFile.Load(reader, CsvFileLoadOptions.WithHeaders);

                // Verify

                Assert.IsNotNull(csvFile.Headers);

                // put headers and values in-memory to avoid crazy shit due to iterating over the collections multiple times

                List<string> headers = csvFile.Headers.ToList();

                var headersAgain = csvFile.Headers.ToList();

                List<List<string>> values = csvFile.Values.Select(x => x.ToList()).ToList();

                var valuesAgain = csvFile.Values.Select(x => x.ToList()).ToList();

                Assert.IsTrue(headers.Count == 5);
                Assert.IsTrue(values.All(x => x.Count == 5));
                Assert.IsTrue(values.Count == 185);
            }
        }

        [TestMethod]
        public void CsvFile_Save_Works()
        {
            // Setup
            string[] headers = new string[] { "First Name", "Last Name" };
            string[][] values = new string[][]
            {
                new string[]{"Joe", "Blow"},
                new string[]{"Dickhead", "McGee"}
            };

            CsvFile csv = new CsvFile
            {
                Headers = headers,
                Values = values
            };
            string csvString;

            using (var writer = new StringWriter())
            {
                // Act
                csv.Write(writer);

                csvString = writer.ToString();
            }


            // Verify
            Assert.IsTrue(csvString.Length > 0);
        }

        [TestMethod]
        public async Task CSvFile_Load_LargeFileFromWeb()
        {
            // Setup
            ulong rowCount = 0;
            const ulong expectedRowCount = 10000000;
            string url = "https://raw.githubusercontent.com/Schlumberger/hackathon/master/backend/dataset/data-large.csv";

            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            using (var response = await request.GetResponseAsync())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                CsvFile csv = CsvFile.Load(reader);

                // Act
                foreach (var line in csv.Values)
                {
                    ++rowCount;
                }
            }

            // Verify
            Assert.AreEqual(expectedRowCount, rowCount);
        }
    }
}
