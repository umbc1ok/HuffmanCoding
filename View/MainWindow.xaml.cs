using Logic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        public Huffman f;

        public MainWindow()
        {
            InitializeComponent();
            f = new Huffman();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string text = InputBox.Text;
            List<bool> x = f.EncodeAString(text);
            OutputBox.Text = f.ConvertBytesToString(f.ConvertBoolsToBytes(x));
        }
        private void Decode(object sender, RoutedEventArgs e)
        {
            
            string text = OutputBox.Text;
            List<bool> textInBools = f.ConvertBytesToBools(f.ConvertStringToBytes(text));
            InputBox.Text = f.Decode(textInBools);
            
        }
        private void PopOut(object sender, RoutedEventArgs e)
        {
            PopoutWindow popout = new PopoutWindow(this);
            popout.ShowDialog();
        }
        private void SaveText(object sender, RoutedEventArgs e)
        {
            string filePath = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
                // Save the file
            }
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(InputBox.Text);
            }
        }
        private void SaveCode(object sender, RoutedEventArgs e)
        {
            string filePath = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
                // Save the file
            }
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(OutputBox.Text);
            }
        }
    }
}
