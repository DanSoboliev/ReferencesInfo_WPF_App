using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ReferencesInfo.IO {
    public class GenericBinarySerializationController<T> where T : class {

        public string FileExtension { get; set; }

        public T Load(string fileName) {
            fileName = Path.ChangeExtension(fileName, FileExtension);
            if (!File.Exists(fileName))
                return null;
            BinaryFormatter bFormatter = new BinaryFormatter();
            using (FileStream fSteam = File.OpenRead(fileName)) {
                return (T)bFormatter.Deserialize(fSteam);
            }
        }

        public void Save(T data, string fileName) {
            fileName = Path.ChangeExtension(fileName, FileExtension);
            BinaryFormatter bFormatter = new BinaryFormatter();
            using (var fStream = File.OpenWrite(fileName)) {
                bFormatter.Serialize(fStream, data);
            }
        }
    }
}
