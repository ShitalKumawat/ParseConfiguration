using System.IO;
using System.Xml.Serialization;

namespace Common
{
    public class XmlFileProvider<T> : IProvider<T>
    {
        public string FileName { get; set; }
        public T Read()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T obj = default(T);
            using (FileStream fs = new FileStream(FileName, FileMode.Open))
            {
                obj = (T)serializer.Deserialize(fs);
            }
            return obj;
        }
    }
}
