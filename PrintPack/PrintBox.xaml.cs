﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PrintPack
{
    /// <summary>
    /// Interaction logic for PrintBox.xaml
    /// </summary>
    public partial class PrintBox : Window
    {
        public PrintBox()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                
                comboBox3.Items.Add(i);
            }
        }
    }
}
