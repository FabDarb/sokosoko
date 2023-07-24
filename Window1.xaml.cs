using Microsoft.Win32.SafeHandles;
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
        public int num = 99;
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
            num = 99;
            check();
            test = 1;
            lblMax.Content = "99";
        }

        private void ramoyen_Checked(object sender, RoutedEventArgs e)
        {
            chois = "2 First steps - Advanced";
            LblChoix.Content = "moyen";
            num = 79;
            check();
            test = 2;
            lblMax.Content = "79";
        }

        private void radifficile_Checked(object sender, RoutedEventArgs e)
        {
            chois = "3 First steps - Expert";
            LblChoix.Content = "difficile";
            num = 29;
            check();
            test = 3;
            lblMax.Content = "29";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            numMonde = int.Parse(txtNb.Text);
            

            this.Close();
        }

        private void Btn_Plus_Click(object sender, RoutedEventArgs e)
        {
            numMonde = int.Parse(txtNb.Text);
            numMonde++;
            if(numMonde > num)
            {
                numMonde--;
            }
            txtNb.Text = numMonde.ToString();
        }

        private void Btn_moin_Click(object sender, RoutedEventArgs e)
        {
            numMonde = int.Parse(txtNb.Text);
            numMonde--;
            if (numMonde < 0)
            {
                numMonde++;
            }
            txtNb.Text = numMonde.ToString();
        }

        void check()
        {
            btnAdd.IsEnabled = true;
            if (numMonde > num || numMonde < 0)
            {
                btnAdd.IsEnabled = false;
            }
        }

        private void txtNb_TextChanged(object sender, TextChangedEventArgs e)
        {
            check();
        }
    }
}
