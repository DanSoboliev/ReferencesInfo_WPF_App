using ReferencesInfo.Data;
using ReferencesInfo.Interfaces;

namespace ReferencesInfo.IO {
    public class BinarySerializationController : 
        GenericBinarySerializationController<DataSet>, IFileIoController {
        public void Load(DataSet dataSet, string fileName) {
            DataSet newDataSet = Load(fileName);
            if (newDataSet == null)
                return;
            foreach (var el in newDataSet.Books) {
                dataSet.Books.Add(el);
            }
            foreach (var el in newDataSet.Links) {
                dataSet.Links.Add(el);
            }
        }
    }
}
