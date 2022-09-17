using System;
using System.IO;
using System.Linq;
using System.Text;
using Common.Context.Extensions;
using Common.Context.StringFormatters;
using ReferencesInfo.Data;
using ReferencesInfo.Entities;
using ReferencesInfo.Interfaces;

namespace ReferencesInfo.IO {
    public class TextFileIoController : IFileIoController {
        public string FileExtension { get; } = ".txt";

        //Save і Load
        public void Load(DataSet dataSet, string fileName) {
            if (!File.Exists(fileName))
                return;
            StreamReader sr = new StreamReader(fileName, Encoding.Unicode);
            while (sr.Peek() >= 0) {
                ReadRecordData(sr, dataSet);
            }
            sr.Close();
        }
        public void Save(DataSet dataSet, string fileName) {
            if (File.Exists(fileName)) DeleteFile(fileName);
            File.Create(fileName).Close();
            foreach (var el in dataSet.Books) {
                string obj = CreateBookString(el);
                File.AppendAllText(fileName, obj, Encoding.Unicode);
            }
            foreach (var el in dataSet.Links) {
                string obj = CreateLinkString(el);
                File.AppendAllText(fileName, obj, Encoding.Unicode);
            }
        }

        //DeleteFile
        public static void DeleteFile(string fileName) {
            File.Delete(fileName);
        }

        //CreateObjectString
        private static string CreateBookString(Book book) {
            return string.Format("Книга:\n\tId: {6}\n\tНазва: {0}\n"
                + "\tРік видання: {1}\n\tКількість сторінок: {2}\n"
                + "\tНаявність ілюстрацій: {3}\n\tЗміст: {4}\n"
                + "\tОпис: {5}\n",
                book.Name, book.YearPublication, book.NumberOfPages, book.Illustration,
                CreateAtribute(book.Content), CreateAtribute(book.Description), book.Id);
        }
        private static string CreateLinkString(Link link) {
            return string.Format("Посилання:\n\tId: {4}\n\tТема: {0}\n"
                + "\tКлюч книги: {1}\n\tНомер сторінки: {2}\n"
                + "\tПримітка: {3}\n",
                link.Topic, link.Book.Key, link.PageNumber, CreateAtribute(link.Note), link.Id);
        }

        private static string CreateAtribute(string str) {
            StringFormatter.Current.LineLength = 50;
            str = string.IsNullOrWhiteSpace(str) ? "" : str.ToIndentedLineBlock(0);
            str = str.Insert(0, "<:");
            str += ":>";
            return str;
        } 

        //GetValue
        private static string GetValue(string s) {
            int pos = s.IndexOf(":");
            return s.Substring(pos + 1).Trim();
        }
        private static string GetValue(StreamReader sr, string openTag, string closeTag) {
            StringBuilder sb = new StringBuilder();
            string s = sr.ReadLine();
            int pos = s.IndexOf(":");
            int openTagLength = openTag.Length;
            if (s.IndexOf(openTag) != -1) {
                while (true) {
                    int pos1 = s.IndexOf(closeTag);
                    int pos2 = pos + 2 + openTagLength;
                    if (pos1 != -1) {
                        sb.Append(s.Substring(pos2, pos1 - (pos2)));
                        return sb.ToString();
                    }
                    else {
                        sb.Append(s.Substring(pos2));
                        s = sr.ReadLine();
                    }
                    pos = -(2 + openTagLength);
                }
            }
            else {
                sb.Append(s.Substring(pos + 1).Trim());
                return sb.ToString();
            }
        }

        //ReadData
        private void ReadRecordData(StreamReader sr, DataSet dataSet) {
            string s = sr.ReadLine().Trim();
            if(s == "Книга:") {
                dataSet.Books.Add(ReadBookData(sr));
            }
            else if(s == "Посилання:") {
                dataSet.Links.Add(ReadLinkData(sr, dataSet));
            }
            else {

            }
        }
        private Book ReadBookData(StreamReader sr) {
            Book book = new Book();
            book.Id = Convert.ToInt32(GetValue(sr.ReadLine()));
            book.Name = GetValue(sr.ReadLine());
            book.YearPublication = Convert.ToInt32(GetValue(sr.ReadLine()));
            book.NumberOfPages = Convert.ToInt32(GetValue(sr.ReadLine()));
            string s = GetValue(sr.ReadLine());
            book.Illustration = string.IsNullOrEmpty(s) ? (bool?)null : bool.Parse(s);
            book.Content = GetValue(sr, "<:", ":>");
            book.Description = GetValue(sr, "<:", ":>");
            return book;
        }
        private Link ReadLinkData(StreamReader sr, DataSet dataSet) {
            Link link = new Link();
            link.Id = Convert.ToInt32(GetValue(sr.ReadLine()));
            link.Topic = GetValue(sr.ReadLine());
            string s = GetValue(sr.ReadLine());
            link.Book = dataSet.Books.FirstOrDefault(e => e.Key == s);
            link.PageNumber = Convert.ToInt32(GetValue(sr.ReadLine()));
            link.Note = GetValue(sr, "<:", ":>");
            return link;
        }
    }
}
