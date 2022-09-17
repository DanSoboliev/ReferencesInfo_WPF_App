using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReferencesInfo.Entities;

namespace ReferencesInfo.Data {
    [Serializable]
    public class DataSet {
        public ObservableCollection<Book> Books { get; private set; }
        public ObservableCollection<Link> Links { get; private set; }
        public DataSet() {
            Books = new ObservableCollection<Book>();
            Links = new ObservableCollection<Link>();
        }
    }
}
