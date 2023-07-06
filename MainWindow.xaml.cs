﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace sokosoko
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //variable
        int grand = 0;
        int large = 0;
        int y;
        int x;
        int t = 0;
        int laterY = 3;
        int laterX = 1;
        char[,] fond;
        bool plus = false;
        int nb_DePoint = 0;
        int resultPlayer = 0;
        int laterGY;
        int laterGX;
        int ou = 0;
        bool wallP = false;
        bool wallC = false;
        bool caseP = false;
        bool caseC = false;
        bool envP = false;
        bool envC = false;
        bool casePEnv = false;
        bool rien = false;
        bool surEnv = false;
        bool rienC = false;
        bool caseCE = false;

        //grille pour toute les images du jeu
        UCImage[,] grilleUI;
        public MainWindow()
        {
            InitializeComponent();
            //les deux tableaux multi
            getFile();
            grilleUI = new UCImage[large, grand];
            
            //remp();
            Create();
        }
        public void Create()
        {
            //création de la gride
            for(int i = 0; i < large; i++)
            {
                griid.RowDefinitions.Add(new RowDefinition());
            }
            for(int i = 0; i < grand; i++)
            {
                griid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            //mettre les images dans chaques éléments de la gride en dépendant de se que c'est (wiki sokoban)
            for (int i = 0; i < large; i++)
            {
                for (int e = 0; e < grand; e++)
                {
                    UCImage img = new UCImage();
                    switch (fond[i, e])
                    {
                        case '#':
                            img.walls();
                            break;
                        case '$':
                            img.crates();
                            break;
                        case '.':
                            img.env();
                            nb_DePoint++;
                            break;
                        case '@':
                            img.playerStart();
                            y = i;
                            x = e;
                            break;
                        case '*':
                            img.caseONJu();
                            break;
                        default:
                            img.groud();
                            break;
                    }
                    Grid.SetRow(img, i);
                    Grid.SetColumn(img, e);
                    griid.Children.Add(img);
                    grilleUI[i,e] = img;
                }
            }
        }
        public void refresh()
        {
            // tou afficher les modifications
            for(int i = 0; i < large; i++)
            {
                for(int e  = 0; e < grand; e++)
                {
                    UCImage img = grilleUI[i,e];
                    switch (fond[i, e])
                    {
                        case '#':
                            img.walls();
                            break;
                        case '$':
                            img.crates();
                            break;
                        case '.':
                            img.env();
                            break;
                        case '@':
                            img.pos(ou);
                            break;
                        case '*':
                            img.caseONJu();
                            break;
                        case '+':
                            img.playerONEnv(ou);
                            break;
                        default:
                            img.groud();
                            break;
                    }
                }
            }
        }
        public void remp()
        {
            fond[0, 0] = '#';
            fond[1, 0] = '#';
            fond[2, 0] = '#';
            fond[3, 0] = '#';
            fond[4, 0] = '#';
            fond[5, 0] = '#';
            fond[6, 0] = '#';

            fond[0, 1] = '#';
            fond[1, 1] = ' ';
            fond[2, 1] = ' ';
            fond[3, 1] = '@';
            fond[4, 1] = ' ';
            fond[5, 1] = ' ';
            fond[6, 1] = '#';

            fond[0, 2] = '#';
            fond[1, 2] = ' ';
            fond[2, 2] = '#';
            fond[3, 2] = ' ';
            fond[4, 2] = ' ';
            fond[5, 2] = ' ';
            fond[6, 2] = '#';

            fond[0, 3] = '#';
            fond[1, 3] = '.';
            fond[2, 3] = '$';
            fond[3, 3] = ' ';
            fond[4, 3] = '$';
            fond[5, 3] = '.';
            fond[6, 3] = '#';

            fond[0, 4] = '#';
            fond[1, 4] = ' ';
            fond[2, 4] = ' ';
            fond[3, 4] = ' ';
            fond[4, 4] = ' ';
            fond[5, 4] = ' ';
            fond[6, 4] = '#';

            fond[0, 5] = '#';
            fond[1, 5] = ' ';
            fond[2, 5] = ' ';
            fond[3, 5] = ' ';
            fond[4, 5] = ' ';
            fond[5, 5] = ' ';
            fond[6, 5] = '#';

            fond[0, 6] = '#';
            fond[1, 6] = '#';
            fond[2, 6] = '#';
            fond[3, 6] = '#';
            fond[4, 6] = '#';
            fond[5, 6] = '#';
            fond[6, 6] = '#';
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //controle de la touche utiliser
            Debug.WriteLine(e.Key);
            switch(e.Key)
            {
                //pour activer le "debuger"
                case Key.P:
                    Dump();
                    break;
                case Key.R:
                    break;
            }
            // déroulement de changement du tableau de fond
            beforeMove(e.Key);
            move();
            refresh();
            finCode();
        }
        private void move()
        {
            if(caseP || casePEnv)
            {
                foncMoveCase();
                if (wallC || caseC)
                {
                    x = laterX;
                    y = laterY;
                }
                if (envC)
                {
                    moveBoxEnv();
                    caseCE = true;
                    resultPlayer++;
                }
                if (rienC)
                {
                    moveBox();
                    surEnv = false;
                }
            }
            else
            {
                if(wallP)
                {
                    x = laterX;
                    y = laterY;
                }
                if(envP)
                {
                    if (fond[laterY, laterX] == '+')
                    {
                        fond[y, x] = '+';
                        fond[laterY, laterX] = '.';
                    }
                    else
                    {
                        fond[y, x] = '+';
                        fond[laterY, laterX] = ' ';
                    }
                    surEnv = true;
                }
                if(rien) 
                {
                    if(surEnv)
                    {
                        fond[y, x] = '@';
                        fond[laterY, laterX] = '.';
                        surEnv = false;
                    }
                    else
                    {
                        fond[y, x] = '@';
                        fond[laterY, laterX] = ' ';
                    }
                }
            }
        }
        public void beforeMove(Key keys)
        {
            //controle de la touche user
            //mise en place de l'ancien placement du personage
            laterX = x;
            laterY = y;
            
            switch(keys)
            {
                case Key.Up:
                    //déplacement
                    y--;
                    // savoir pour l'img quel direction il a eu
                    ou = 3;
                    // dire si il a fait + ou - en terme de déplacement
                    plus = false;
                    break;
                case Key.Down:
                    y++;
                    ou = 2;
                    plus = true;
                    break;
                case Key.Left:
                    x--;
                    ou = 1;
                    plus = false;
                    break;
                case Key.Right:
                    x++;
                    ou = 0;
                    plus = true;
                    break;
            }
            scan();
        }
        public void scan()
        {
            resetVar();
            //savoir se qu'il y a devant lui (wiki sokoban)
            switch(fond[y,x])
            {
                case '#':
                    wallP = true;
                    break;
                case '.':
                    envP = true;
                    break;
                case '$':
                    caseP = true;
                    break;
                case '*':
                    casePEnv = true;
                    break;
                default:
                    rien = true;
                    break;
            }
        }
        public void Dump()
        {
            for (int row = 0; row < fond.GetLength(0); row++) {
                for (int col = 0; col < fond.GetLength(1); col++)
                {
                    Debug.Write(fond[row,col]);
                }
                Debug.WriteLine("");
                
            }
            Debug.WriteLine(x);
            Debug.WriteLine(y);
        }

        public void foncMoveCase()
        {
            // scan si il c'est déplacer sur l'axe y ou x
            if (y / laterY != 1 || y % laterY != 0)
            {
                //scan si il c'est déplacer en avant ou arrière
                if (plus == true)
                {
                    // garder position avant de la case
                    laterGY = y;
                    laterGX = x;
                    deufoncCase(fond[y + 1, x]);
                }
                else
                {
                    laterGY = y;
                    laterGX = x;
                    deufoncCase(fond[y - 1, x]);
                }
            }
            else if (x / laterX != 1 || x % laterX != 0)
            {
                if (plus == true)
                {
                    laterGX = x;
                    laterGY = y;
                    deufoncCase(fond[y, x + 1]);
                }
                else
                {
                    laterGX = x;
                    laterGY = y;
                    deufoncCase(fond[y, x - 1]);
                }
            }
        }

        public void deufoncCase(char duTab)
        {
            resetVar();
            // savoir se qu'il y a devant la case
            switch(duTab)
            {
                case '#':
                    wallC = true;
                    break;
                case '$':
                    caseC = true;
                    break;
                case '*':
                    caseC = true;
                    break;
                case '.':
                    envC = true;
                    break;
                default:
                    rienC = true;
                    break;
            }
        }
        public void switchcolor(int grandY, int grandX, int p)
        {
            
            
        }

        public void finCode()
        {
            // regarder si c'est win ou pas encore
            if(resultPlayer == nb_DePoint)
            {
                MessageBox.Show("good game");
                // down le program
                Environment.Exit(0);
            }
        }
        public void getFile()
        {
            
            string[] lines = File.ReadAllLines("./1 First steps - Beginner/1 First steps - Beginner_18.txt");
            foreach (var line in lines)
            {
                if (line.Trim()[0] == '#')
                {
                    // taille tableau
                    if(line.Length > grand)
                    {
                         grand = line.Length;
                    }
                    large++;
                }
            }
            fond = new char[large, grand];
            int taileGLigne = 0;
            foreach (var line in lines)
            {
                if (line.Trim()[0] == '#')
                {
                    t = 0;
                    foreach (char c in line)
                    {
                        fond[taileGLigne, t] = c;
                        t++;
                    }
                    taileGLigne++;
                }
            }
        }
        public void resetVar()
        {
            wallP = false;
            wallC = false;
            caseP = false;
            caseC = false;
            envP = false;
            envC = false;
            casePEnv = false;
            rien = false;
            rienC = false;
            caseCE = false;
        }

        //public void difficult()
        //{
        //    string ecrit = "quel difficulté vous voudriez ?";

        //    DialogResult rep = MessageBox.Show(ecrit, )
        //}
        public void moveBox()
        {
            if (y / laterY != 1 || y % laterY != 0)
            {
                //scan si il c'est déplacer en avant ou arrière
                if (plus == true)
                {
                    // garder position avant de la case
                    fond[y + 1, x] = '$';
                    if(envC)
                    {

                    }
                    sup();
                }
                else
                {
                    fond[y - 1, x] = '$';
                    
                    sup();
                }
            }
            else if (x / laterX != 1 || x % laterX != 0)
            {
                if (plus == true)
                {
                    fond[y, x + 1] = '$';
                    
                    sup();
                }
                else
                {
                    fond[y, x - 1] = '$';
                    
                    sup();
                }
            }
        }
        public void sup()
        {
            if(fond[y, x] == '*')
            {
                fond[y, x] = '+';
                resultPlayer--;
                surEnv = true;
            }
            else
            {
                fond[y, x] = '@';
            }
            if (fond[laterY, laterX] == '+')
            {
                fond[laterY, laterX] = '.';
                
            }
            else
            {
                fond[laterY, laterX] = ' ';
                //surEnv = false;
            }
            
        }
        public void moveBoxEnv()
        {
            if (y / laterY != 1 || y % laterY != 0)
            {
                //scan si il c'est déplacer en avant ou arrière
                if (plus == true)
                {
                    fond[y + 1, x] = '*';
                    sup();
                }
                else
                {
                    fond[y - 1, x] = '*';
                    sup();
                }
            }
            else if (x / laterX != 1 || x % laterX != 0)
            {
                if (plus == true)
                {
                    fond[y, x + 1] = '*';
                    sup();
                }
                else
                {
                    fond[y, x - 1] = '*';
                    sup();
                }
            }
        }
    }
}