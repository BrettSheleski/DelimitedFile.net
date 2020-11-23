# Sheleski.DelimitedFile.Mvc

Helpers for working with delimited files using the [Sheleski.DelimitedFile](https://github.com/BrettSheleski/DelimitedFile.net) package in ASP.NET MVC.

## CsvFileResult

Create a file download of a CSV file to the user from a CSV file easily.

```C#
public CsvFileResult GetPeopleCSV()
{
    IEnumerable<Person> people = GetPeopleSomehow();

    CsvFile csv = people.ToCsv();

    return new CsvFileResult(csv, "people.csv")
}
```
