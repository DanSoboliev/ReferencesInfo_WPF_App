using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using ReferencesInfo.Data;
using ReferencesInfo.IO;
using WpfApp1.Core;

namespace WpfApp1.MVVM.ViewModel {
    class SaveLoadViewModel : ObservableObject {
        DataContext dataContext;
        private static BinarySerializationController bsIoController = new BinarySerializationController();
        private static XmlFileIoController xmlFileIoController = new XmlFileIoController();
        private static TextFileIoController textFileIoController = new TextFileIoController();

        public RelayCommand Exit { get; set; }
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public int ItemsSelectedIndex { get; set; }
        public int ExtentionsSelectedIndex { get; set; }

        public List<string> items;
        public List<string> Items { 
            get { return items;  } 
            set { 
                items = value;
                OnPropertyChanged();
            }
        }
        public List<string> FilesExtention { get; set; }
        public string FileNameSave { get; set; }
        public SaveLoadViewModel(DataContext dataContext, RelayCommand exit) {
            Exit = exit;
            this.dataContext = dataContext;
            FilesExtention = new List<string>();
            FilesExtention.Add(".txt");
            FilesExtention.Add(".xml");
            LoadList();
            LoadCommand = new RelayCommand(o => {
                if (ItemsSelectedIndex<0) return;
                string fileName = Items[ItemsSelectedIndex];

                string Extension = Path.GetExtension(fileName);
                switch (Extension) {
                    case @".xml":
                        dataContext.FileIoController = xmlFileIoController;
                        break;
                    case @".txt":
                        dataContext.FileIoController = textFileIoController;
                        break;
                    default:
                        MessageBox.Show("Файл з таким розширенням не підтримується");
                        return;
                }
                string FileNameLoad = Path.GetFileName(fileName).Replace(Extension, "");
                DataContext.fileName = FileNameLoad;
                LoadHelp();
            });
            SaveCommand = new RelayCommand(o => {
                DataContext.fileName = FileNameSave;
                string fileExtention = FilesExtention[ExtentionsSelectedIndex];
                switch (fileExtention) {
                    case @".txt":
                        dataContext.FileIoController = textFileIoController;
                        bsIoController.FileExtension = ".txt";
                        break;
                    case @".xml":
                        dataContext.FileIoController = xmlFileIoController;
                        bsIoController.FileExtension = ".xml";
                        break;
                    default:
                        MessageBox.Show("Виникла помилка");
                        return;
                }
                dataContext.Save();
                LoadList();
                MessageBox.Show("Дані успішно збережено");
            });
        }

        public void LoadList() {
            Items = (Directory.GetFiles("files")).ToList();
        }
        private void LoadHelp() {
            try {
                dataContext.Clear();
                dataContext.Load();
                MessageBox.Show("Дані успішно завантажено");
            }
            catch (Exception ex) {
                dataContext.Clear();
                MessageBox.Show("Виникла помилка");
                return;
            }
        }

    }
}
