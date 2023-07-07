using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace sokosoko
{
    /// <summary>
    /// Logique d'interaction pour UCImage.xaml
    /// </summary>
    public partial class UCImage : UserControl
    {
        public UCImage()
        {
            InitializeComponent();
        }

        public void changeTaill(int taille)
        {
            UCImageSoko.Width = taille;
            UCImageSoko.Height = taille;
            UCgrid.Width = taille;
            UCgrid.Height = taille;

        }
        public void clearAllImage()
        {
            crate.Visibility = Visibility.Hidden;
            environment.Visibility = Visibility.Hidden;
            ground.Visibility = Visibility.Hidden;
            player_bottom.Visibility = Visibility.Hidden;
            player_left.Visibility = Visibility.Hidden;
            player_right.Visibility = Visibility.Hidden;
            player_top.Visibility = Visibility.Hidden;
            player_Case.Visibility = Visibility.Hidden;
            wall.Visibility = Visibility.Hidden;
            crateJu.Visibility = Visibility.Hidden;
        }
        public void walls()
        {
            clearAllImage();
            wall.Visibility = Visibility.Visible;
        }
        public void crates()
        {
            clearAllImage();
            crate.Visibility = Visibility.Visible;
        }
        public void env()
        {
            clearAllImage();
            ground.Visibility = Visibility.Visible;
            environment.Visibility = Visibility.Visible;
            
        }
        public void groud()
        {
            clearAllImage();
            ground.Visibility = Visibility.Visible;
        }
        public void playerStart() 
        {
            clearAllImage();
            ground.Visibility = Visibility.Visible;
            player_right.Visibility = Visibility.Visible;
        }
        public void playerONEnv(int o) 
        {
            clearAllImage();
            ground.Visibility = Visibility.Visible;
            switch (o)
            {
                case 0:
                    player_right.Visibility = Visibility.Visible;
                    break;
                case 1:
                    player_left.Visibility = Visibility.Visible;
                    break;
                case 2:
                    player_top.Visibility = Visibility.Visible;
                    break;
                case 3:
                    player_bottom.Visibility = Visibility.Visible;
                    break;
                case 4:
                    player_Case.Visibility = Visibility.Visible;
                    break;
            }
            environment.Visibility = Visibility.Visible;
        }
        public void caseONJu()
        {
            clearAllImage();
            crateJu.Visibility = Visibility.Visible;
        }

        public void pos(int o)
        {
            clearAllImage();
            ground.Visibility = Visibility.Visible;
            switch (o)
            {
                case 0:
                    player_right.Visibility = Visibility.Visible;
                    break;
                case 1:
                    player_left.Visibility = Visibility.Visible;
                    break;
                case 2:
                    player_top.Visibility = Visibility.Visible;
                    break;
                case 3:
                    player_bottom.Visibility = Visibility.Visible;
                    break;
                case 4:
                    player_Case.Visibility = Visibility.Visible;
                    break;
                case 5:
                    player_right.Visibility = Visibility.Visible;
                    ground.Visibility = Visibility.Hidden;
                    break;
            }
        }
    }
}
