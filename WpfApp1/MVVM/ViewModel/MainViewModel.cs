using System.Windows.Controls;
using ReferencesInfo.Data;
using ReferencesInfo.Interfaces;
using ReferencesInfo.IO;
using WpfApp1.Core;
using WpfApp1.MVVM.View;

namespace WpfApp1.MVVM.ViewModel {
    class MainViewModel : ObservableObject {
        private DataContext dataContext;
        static string directory = "files";
        private static IFileIoController fileIoController = new XmlFileIoController();

        private bool mrCheck1;
        private bool mrCheck2;
        private bool mrCheck3;
        private bool mrCheck4;
        public bool MRCheck1 {
            get { return mrCheck1; }
            set {
                mrCheck1 = value;
                OnPropertyChanged();
            }
        }
        public bool MRCheck2 {
            get { return mrCheck2; }
            set {
                mrCheck2 = value;
                OnPropertyChanged();
            }
        }
        public bool MRCheck3 {
            get { return mrCheck3; }
            set {
                mrCheck3 = value;
                OnPropertyChanged();
            }
        }
        public bool MRCheck4 {
            get { return mrCheck4; }
            set {
                mrCheck4 = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand BookViewCommand { get; set; }
        public RelayCommand LinkViewCommand { get; set; }
        public RelayCommand SaveLoadViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public BookViewModel BookVM { get; set; }
        public LinkViewModel LinkVM { get; set; }
        public SaveLoadViewModel SaveLoadVM { get; set; }


        private object _currentView;
        public object CurrentView {
            get { return _currentView; }
            set { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(){
            dataContext = new DataContext(fileIoController) {
                Directory = directory
            };
            MRCheck1 = true;

            UserControl HomeV = new HomeView();
            UserControl BookV = new BookView();
            UserControl LinkV = new LinkView();
            UserControl SaveLoadV = new SaveLoadView();

            HomeVM = new HomeViewModel();
            BookVM = new BookViewModel(dataContext);
            LinkVM = new LinkViewModel(dataContext);
            SaveLoadVM = new SaveLoadViewModel(dataContext, HomeViewCommand);

            HomeV.DataContext = this;
            BookV.DataContext = BookVM;
            LinkV.DataContext = LinkVM;
            SaveLoadV.DataContext = SaveLoadVM;

            CurrentView = HomeV;

            HomeViewCommand = new RelayCommand(o => {
                CurrentView = HomeV;
                MRCheck1 = true;
            });
            BookViewCommand = new RelayCommand(o => {
                CurrentView = BookV;
                MRCheck2 = true;
            });
            LinkViewCommand = new RelayCommand(o => {
                CurrentView = LinkV;
                MRCheck3 = true;
            });
            SaveLoadViewCommand = new RelayCommand(o => {
                CurrentView = SaveLoadV;
                SaveLoadVM.LoadList();
                MRCheck4 = true;
            });
        }
    }
}
