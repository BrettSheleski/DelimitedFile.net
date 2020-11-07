# DelimitedFile.net
Library for parsing and saving CSV and Tab Delimited Files in .NET.

## Usage

The `CsvFile` has a very simple API with `Headers` and `Values` properties.  Use these properties to work with the data in teh CSV file.

```C#
CsvFile csv = new CsvFile();

csv.Headers = new string[]{ "First Name", "Last Name" };
csv.Values = new string[][]
{
  new string[]{"Joe", "Blow"},
  new string[]{"Bonehead", "McGee"}
};
```

### Save the CSV to file

```C#
csv.Save("c:\\temp\\names.csv");
```

### Write the CSV to any arbitrary `TextReader`

```C#
using (var writer = new StringWriter())
{
    csv.Write(writer);

    string csvString = writer.ToString();
}
```
