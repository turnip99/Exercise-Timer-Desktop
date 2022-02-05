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

namespace Exercise_Timer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Window NewWindow;
        public MainWindow()
        {
            InitializeComponent();
            ButtonManage.Click += ButtonManage_Click;
            ButtonGet.Click += ButtonGet_Click;
            ButtonTimer.Click += ButtonTimer_Click;
        }

        private void ButtonManage_Click(object sender, RoutedEventArgs e)
        {
            NewWindow = new ManageWindow();
            NewWindow.ShowDialog();
        }

        private void ButtonGet_Click(object sender, RoutedEventArgs e)
        {
            NewWindow = new GenerateWindow();
            NewWindow.ShowDialog();
        }

        private void ButtonTimer_Click(object sender, RoutedEventArgs e)
        {
            NewWindow = new TimerWindow();
            NewWindow.ShowDialog();
        }
    }
}
