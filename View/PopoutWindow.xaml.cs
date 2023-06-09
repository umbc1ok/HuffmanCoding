﻿using Logic;
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
using System.Windows.Shapes;

namespace View
{
    /// <summary>
    /// Logika interakcji dla klasy PopoutWindow.xaml
    /// </summary>
    public partial class PopoutWindow : Window
    {
        string inputText;
        string outputText;
        MainWindow window;
        public PopoutWindow(MainWindow mainWindow)
        {
            window = mainWindow;
            InitializeComponent();
            inputText = mainWindow.InputBox.Text;
            outputText = mainWindow.OutputBox.Text;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Client.HandleAndSendData(inputText, PopoutTextBox.Text);
            this.Close();
        }   

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Server server = new Server();
            window.OutputBox.Text = server.ReceiveAndHandleData(window.f);
            this.Close();
        }
    }
}
