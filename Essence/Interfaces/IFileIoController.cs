using ReferencesInfo.Data;

namespace ReferencesInfo.Interfaces {
    public interface IFileIoController {
        string FileExtension { get; }

        void Load(DataSet dataSet, string fileName);
        void Save(DataSet dataSet, string fileName);
    }
}
