# Sheleski.DelimitedFile.MvcCore
Helpers for working with delimited files using the Sheleski.DelimitedFile package in ASP.NET MVCCore.

## CsvFileResult
Create a file download of a CSV file to the user from a CSV file easily.

```
public CsvFileResult GetPeopleCSV()
{
    IEnumerable<Person> people = GetPeopleSomehow();

    CsvFile csv = people.ToCsvFile();

    return new CsvFileResult(csv, "people.csv")
}
```