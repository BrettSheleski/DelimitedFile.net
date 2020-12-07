namespace Sheleski.DelimitedFile
{
    public interface IDelimitedFileLoadOptions : IDelimitedFileOptions
    {
        int BufferSize { get; }
    }
}
