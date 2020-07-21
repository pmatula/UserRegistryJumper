using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace UserRegistryJumper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private RelayCommand<int?> _commandStart;

        public ObservableCollection<String> LbUsers
        {
            get
            {
                return BusinessLogic.GetUsers(); 
            }
        }
        public ObservableCollection<String> LbRegistry
        {
            get
            {
                return BusinessLogic.GetRegistry();
            }
        }
        public ICommand btJump
        {
            get
            {
                if (_commandStart == null)
                {
                    _commandStart = new RelayCommand<int?>(param => RelayJump());
                }
                return _commandStart;
            }
        }
        public void RelayJump()
        {
            BusinessLogic.DoJump(listBoxUsers.SelectedItem.ToString(), listBoxRegistry.SelectedItem.ToString()); 
        }
    }
}
