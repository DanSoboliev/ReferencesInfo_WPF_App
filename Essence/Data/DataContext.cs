using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.Extensions;
using ReferencesInfo.Entities;
using ReferencesInfo.Interfaces;

namespace ReferencesInfo.Data {
    public partial class DataContext {
        readonly DataSet dataSet = new DataSet();
        IFileIoController fileIoController;
        public static string fileName = "ReferencesInfo";
        private string directory;

        public ObservableCollection<Book> Books {
            get { return dataSet.Books; }
        }
        public ObservableCollection<Link> Links {
            get { return dataSet.Links; }
        }
        public IFileIoController FileIoController {
            get { return fileIoController; }
            set { fileIoController = value; }
        }
        public string Directory {
            get => directory;
            set {
                directory = value;
                if (!System.IO.Directory.Exists(directory)) {
                    System.IO.Directory.CreateDirectory(directory);
                }
            }
        }
        public string Path => System.IO.Path.Combine(directory, fileName + fileIoController.FileExtension);

        //Конструктор
        public DataContext(IFileIoController fileIoController) {
            this.fileIoController = fileIoController;
        }

        //Save і Load
        public void Save() {
            fileIoController.Save(dataSet, Path);
        }
        public void Load() {
            fileIoController.Load(dataSet, Path);
        }

        //ToString
        public override string ToString() {
            return string.Concat("Інформація про об'єкти ПО" +
                " \"Тематичні посилання на джерела інформації\"\n",
                Books.ToLineList("Книги"),
                Links.ToLineList("Посилання"));
        }

        //Тестові дані
        public void CreateTestingData() {
            CreateBooks();
            CreateLinks();
        }

        //Видалення
        public void Clear() {
            Links.Clear();
            Books.Clear();
        }
    }
}
