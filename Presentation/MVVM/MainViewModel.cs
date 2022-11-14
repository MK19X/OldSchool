using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSProject.Core;
using OSProject.DAL;

namespace OSProject.Presentation.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand LibraryViewCommand { get; set; }
        public RelayCommand AccountViewCommand { get; set; }
        public LibraryViewModel LibraryVM { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public AccountViewModel AccountVM { get; set; }
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel() {
            HomeVM = new HomeViewModel();
            LibraryVM = new LibraryViewModel();
            AccountVM = new AccountViewModel();
            
            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o => 
            { 
                CurrentView = HomeVM;
            });
            LibraryViewCommand = new RelayCommand(o =>
            {
                CurrentView = LibraryVM;
            });
            AccountViewCommand = new RelayCommand(o =>
            {
                CurrentView = AccountVM;
            });
        }
    }
}
