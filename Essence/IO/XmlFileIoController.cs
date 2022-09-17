using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using ReferencesInfo.Data;
using ReferencesInfo.Entities;
using ReferencesInfo.Interfaces;

namespace ReferencesInfo.IO {
    public class XmlFileIoController : IFileIoController {
        public string FileExtension { get; } = ".xml";

        //Write
        private void WriteBooks(IEnumerable<Book> collection, XmlWriter writer) {
            writer.WriteStartElement("BooksData");
            foreach(var inst in collection) {
                writer.WriteStartElement("Book");
                writer.WriteElementString("Id", inst.Id.ToString());
                writer.WriteElementString("Name", inst.Name);
                writer.WriteElementString("YearPublication", inst.YearPublication.ToString());
                writer.WriteElementString("NumberOfPages", inst.NumberOfPages.ToString());
                writer.WriteElementString("Illustration", inst.Illustration.ToString());
                writer.WriteElementString("Content", inst.Content);
                writer.WriteElementString("Description", inst.Description);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        private void WriteLinks(IEnumerable<Link> collection, XmlWriter writer) {
            writer.WriteStartElement("LinksData");
            foreach (var inst in collection) {
                writer.WriteStartElement("Link");
                writer.WriteElementString("Id", inst.Id.ToString());
                writer.WriteElementString("Topic", inst.Topic);
                string bookName = inst.Book == null ? "" : inst.Book.Name;
                writer.WriteElementString("BookName", bookName);
                writer.WriteElementString("PageNumber", inst.PageNumber.ToString());
                writer.WriteElementString("Note", inst.Note);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        private void WriteData(DataSet dataSet, XmlWriter writer) {
            writer.WriteStartDocument();
            writer.WriteStartElement("ReferencesInfo");
            WriteBooks(dataSet.Books, writer);
            WriteLinks(dataSet.Links, writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        //Save
        public void Save(DataSet dataSet, string fileName) {
            fileName = Path.ChangeExtension(fileName, FileExtension);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.Unicode;
            XmlWriter writer = null;
            try {
                writer = XmlWriter.Create(fileName, settings);
                WriteData(dataSet, writer);
            }
            catch {
                throw;
            }
            finally {
                if (writer != null) writer.Close();
            }
        }

        //Read
        private void ReadBook(XmlReader reader, ICollection<Book> collection) { 
            reader.ReadStartElement("Book");
            int id = reader.ReadElementContentAsInt();
            string name = reader.ReadElementContentAsString();
            int yearPublication = reader.ReadElementContentAsInt();
            int numberOfPages = reader.ReadElementContentAsInt();
            string i = reader.ReadElementContentAsString();
            bool? illustration = string.IsNullOrEmpty(i) ? (bool?)null : bool.Parse(i);
            string content = reader.ReadElementContentAsString();
            string description = reader.ReadElementContentAsString();
            Book inst = new Book(name, yearPublication, numberOfPages, illustration, content, description) { Id = id };
            collection.Add(inst);
        }
        private void ReadLink(XmlReader reader, DataSet dataSet) {
            reader.ReadStartElement("Link");
            int id = reader.ReadElementContentAsInt();
            string topic = reader.ReadElementContentAsString();
            string bookName = reader.ReadElementContentAsString();
            Book book = dataSet.Books.FirstOrDefault(e => e.Name == bookName);
            int pageNumber = reader.ReadElementContentAsInt();
            string note = reader.ReadElementContentAsString();
            Link inst = new Link(topic, book, pageNumber) { Id = id, Note = note };
            dataSet.Links.Add(inst);
        }

        //Load
        public void Load(DataSet dataSet, string fileName) {
            fileName = Path.ChangeExtension(fileName, FileExtension);
            if (!File.Exists(fileName)) return;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            using (XmlReader reader = XmlReader.Create(fileName, settings)) {
                while (reader.Read()) {
                    if (reader.NodeType == XmlNodeType.Element) {
                        switch (reader.Name) {
                            case "Book":
                                ReadBook(reader, dataSet.Books);
                                break;
                            case "Link":
                                ReadLink(reader, dataSet);
                                break;
                        }
                    }
                }
            }
        }
    }
}
