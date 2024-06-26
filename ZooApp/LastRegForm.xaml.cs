﻿using System;
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

namespace ZooApp
{
    /// <summary>
    /// Логика взаимодействия для LastRegForm.xaml
    /// </summary>
    public partial class LastRegForm : Window
    {
        public LastRegForm()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            FirstRegForm firstRegForm = (FirstRegForm)sender;
            firstRegForm.Show();
            this.Close();

        }

        private void RegUser_Click_1(object sender, RoutedEventArgs e)
        {
            MainManager mainManager = new MainManager();
            mainManager.Show();
            this.Close();
        }
    }
}
