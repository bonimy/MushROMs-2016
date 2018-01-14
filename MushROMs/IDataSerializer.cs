namespace MushROMs
{
    public interface IDataSerializer
    {
        bool IsValidByteData { get; }

        IEditorData EditorData { get; }

        byte[] SerializeData();
    }
}
