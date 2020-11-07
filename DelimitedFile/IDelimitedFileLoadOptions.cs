namespace Sheleski.DelimitedFile
{
    internal interface IDelimitedFileLoadOptions : IDelimitedFileOptions
    {
        int BufferSize { get; }
    }
}
