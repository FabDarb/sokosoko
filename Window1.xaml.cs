﻿using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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
using System.Windows.Shapes;

namespace sokosoko
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public int numMonde;
        public int test = 1;
        public string lvl = "1 First steps - Beginner_0.txt";
        public string chois = "1 First steps - Beginner";
        public Window1()
        {
            InitializeComponent();
            superG.caseONJu();
            lugi.pos(5);
            numMonde = int.Parse(File.ReadAllText("save.txt"));
            txtNb.Text = numMonde.ToString();
        }

        private void raFacile_Checked(object sender, RoutedEventArgs e)
        {
            chois = "1 First steps - Beginner";
            LblChoix.Content = "Facile";
            
        }

        private void ramoyen_Checked(object sender, RoutedEventArgs e)
        {
            chois = "2 First steps - Advanced";
            LblChoix.Content = "moyen";
            
        }

        private void radifficile_Checked(object sender, RoutedEventArgs e)
        {
            chois = "3 First steps - Expert";
            LblChoix.Content = "difficile";
            
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            numMonde = int.Parse(txtNb.Text);
            if (chois == "1 First steps - Beginner")
            {
                if (numMonde > 99 || numMonde < 0)
                {
                    numMonde = 0;
                    test = 1;
                }
                else
                {
                    test = 1;
                    //lvl = $"1 First steps - Beginner_{numMonde}.txt";
                }
            }
            else if(chois == "2 First steps - Advanced")
            {
                if (numMonde > 79 || numMonde < 0)
                {
                    numMonde = 0;
                    test = 2;
                }
                else
                {
                    test = 2;
                    //lvl = $"2 First steps - Advanced_{numMonde}.txt";
                }
            }
            else
            {
                if (numMonde > 29 || numMonde < 0)
                {
                    numMonde = 0;
                    test = 3;
                }
                else
                {
                    test = 3;
                    //lvl = $"3 First steps -Expert_{numMonde}.txt";
                }
            }

            this.Close();
        }

        private void Btn_Plus_Click(object sender, RoutedEventArgs e)
        {
            numMonde++;
            txtNb.Text = numMonde.ToString();
        }

        private void Btn_moin_Click(object sender, RoutedEventArgs e)
        {
            numMonde--;
            txtNb.Text = numMonde.ToString();
        }
    }
}
