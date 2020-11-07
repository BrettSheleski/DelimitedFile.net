namespace Sheleski.DelimitedFile
{
    internal interface IDelimitedFileOptions
    {
        string LineEnding { get; }
        char? TextQualifier { get; }
        bool FirstRowAsHeaders { get; }
        char Delimiter { get; }
    }
}
