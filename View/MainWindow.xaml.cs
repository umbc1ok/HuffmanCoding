using Logic;
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


namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Huffman f = new Huffman();
            string text = InputBox.Text;
            List<bool> x = f.EncodeAString(text);
            OutputBox.Text = f.ConvertBytesToString(f.ConvertBoolsToBytes(x));
        }
        private void PopOut(object sender, RoutedEventArgs e)
        {
            PopoutWindow popout = new PopoutWindow(this);
            popout.ShowDialog();
        }
    }
}
