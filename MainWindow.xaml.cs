using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        bool touchPoint = false;
        bool moveCase = false;
        bool plus = false;
        bool color = false;
        bool enable = false;
        int nb_DePoint = 0;
        int resultPlayer = 0;
        int laterGY;
        int laterGX;
        int ou = 0;
        bool devantMur = false;
        bool caseON = false;
        bool persoON = false;
        bool devantR = false;

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
            if(touchPoint == true)
            {
                // si le user passe sur un env pour changer l'img
                fond[y, x] = '+';
                if (persoON && fond[laterY, laterX] == '+')
                {
                    if(laterX == x && laterY == y)
                    {
                        fond[laterY, laterX] = '+';
                    }
                    else
                    {
                        fond[laterY, laterX] = '.';
                    }
                    
                }
                else
                {
                    fond[laterY, laterX] = ' ';
                }

                touchPoint = false;

            }
            else if (fond[laterY, laterX] == '+' && touchPoint == false) 
            {
                if(laterY != y || laterX != x)
                {
                    if (y / laterY != 1 || y % laterY != 0)
                    {
                        if (plus == true)
                        {
                            laterGY = y;
                            laterGX = x;
                            comCase(fond[y + 1, x]);
                        }
                        else
                        {
                            laterGY = y;
                            laterGX = x;
                            comCase(fond[y - 1, x]);
                        }
                    }
                    else if (x / laterX != 1 || x % laterX != 0)
                    {
                        if (plus == true)
                        {
                            laterGX = x;
                            laterGY = y;
                            comCase(fond[y, x + 1]);
                        }
                        else
                        {
                            laterGX = x;
                            laterGY = y;
                            comCase(fond[y, x - 1]);
                        }
                    }
                    if (devantMur && devantR == false)
                    {
                        fond[y, x] = '+';
                    }
                    else
                    {
                        fond[laterY, laterX] = '.';
                        fond[y, x] = '@';
                        persoON = false;
                    }
                }
            }
            else
            {
                // déplacement basique
                if (y / laterY != 1 || y % laterY != 0)
                {
                    if (plus == true)
                    {
                        laterGY = y;
                        laterGX = x;
                        comCase(fond[y + 1, x]);
                    }
                    else
                    {
                        laterGY = y;
                        laterGX = x;
                        comCase(fond[y - 1, x]);
                    }
                }
                else if (x / laterX != 1 || x % laterX != 0)
                {
                    if (plus == true)
                    {
                        laterGX = x;
                        laterGY = y;
                        comCase(fond[y, x + 1]);
                    }
                    else
                    {
                        laterGX = x;
                        laterGY = y;
                        comCase(fond[y, x - 1]);
                    }
                }
                fond[laterY, laterX] = ' ';
                if (fond[laterY, laterX] == '+' && devantR == true || fond[y, x] == '*' && devantR == true)
                {
                    fond[y, x] = '+';
                }
                else
                {
                    fond[y, x] = '@';
                }

            }
            if(moveCase == true)
            {
                // si une case doit être déplacer
                foncMoveCase();
                
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
            devantR = false;
            //savoir se qu'il y a devant lui (wiki sokoban)
            switch(fond[y,x])
            {
                case '#':
                    // retour au placement d'avant pour pas bouger
                    x = laterX;
                    y = laterY;
                    break;
                case '.':
                    // si c'est un endroit ou il y a un env
                    touchPoint = true;
                    persoON = true;
                    break;
                case '$':
                    // si il y a une case devant lui
                    moveCase = true;
                    caseON = false;
                    break;
                case '*':
                    // si il y a une case sur un env devant lui
                    // enable c'est pour déplacer et laisser une img d'env après le mouvement de la case
                    if(laterX == x && laterY == y)
                    {
                        enable = true;
                    }
                    caseON = true;
                    moveCase = true;
                    break;
                default:
                    devantR = true;
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
                    switchcolor(y + 1, x, 1);
                }
                else
                {
                    laterGY = y;
                    laterGX = x;
                    deufoncCase(fond[y - 1, x]);
                    switchcolor(y - 1, x, 1);
                }
            }
            else if (x / laterX != 1 || x % laterX != 0)
            {
                if (plus == true)
                {
                    laterGX = x;
                    laterGY = y;
                    deufoncCase(fond[y, x + 1]);
                    switchcolor(y, x + 1, 0);
                }
                else
                {
                    laterGX = x;
                    laterGY = y;
                    deufoncCase(fond[y, x - 1]);
                    switchcolor(y, x - 1, 0);
                }
            }
            moveCase = false;
            if (fond[laterY, laterX] != '.' && laterX != x && laterY != y)
            {
                fond[laterY, laterX] = ' ';
            }
            if(enable == true && fond[y, x] == '+')
            {
               enable = false;
            }
            else if (caseON == true)
            {
                
            }
            else if(fond[laterY, laterX] == '+')
            {

            }
            else
            {
                fond[y, x] = '@';
            }
        }

        public void deufoncCase(char duTab)
        {
            // savoir se qu'il y a devant la case
            switch(duTab)
            {
                case '#':
                    x = laterX;
                    y = laterY;
                    devantMur = true;
                    break;
                case '$':
                    x = laterX;
                    y = laterY;
                    devantMur = true;
                    break;
                case '*':
                    x = laterX;
                    y = laterY;
                    devantMur = true;
                    break;
                case '.':
                    // color c'est pour changer futurement l'affichage de la case sur l'env
                    color = true;
                    devantMur = false;
                    break;
                default: 
                    color = false;
                    devantMur = false;
                    break;
            }
        }
        public void switchcolor(int grandY, int grandX, int p)
        {
            if(color == true)
            {
                // changer l'affichage de la case si il est sur l'env
                fond[grandY, grandX] = '*';
                if (laterGX != grandX || laterGY != grandY)
                {
                    // si il est sur l'env il gagne un "point"
                    resultPlayer++;
                }
                if (caseON == true)
                {
                    resultPlayer--;
                    fond[y, x] = '+';
                }
                
            }
            else if (enable == false)
            {
                // si la case sort de l'env changer l'affichage
                if (caseON == true)
                {
                    resultPlayer--;
                }
                if (fond[laterY, laterX] == '+') 
                {
                    fond[y, x] = '+';
                }
                else
                {
                    if (fond[y,x] == '+')
                    {
                        fond[y, x] = '+';
                    }
                    else
                    {
                        fond[y, x] = '@';
                    }
                    
                }
                fond[grandY, grandX] = '$';
                // il perd un point
                
                
            }
            else
            {
                // déplacement normal
                fond[grandY, grandX] = '$';
                fond[y, x] = '+';
            }
            if (devantMur == true)
            {
                if (caseON == true)
                {
                    fond[laterGY, laterGX] = '*';
                    //if(persoON == false)
                    //{
                    //    fond[y, x] = '@';
                    //}
                    resultPlayer++;
                }
                else
                {
                    fond[laterGY, laterGX] = '$';
                    if(persoON || fond[laterY, laterX] == '+')
                    {
                        fond[laterY, laterX] = '+';
                    }
                    else
                    {
                        fond[laterY, laterX] = '@';
                    }
                    
                }

            }
            
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
        public void comCase(char duTab)
        {
            switch (duTab)
            {
                case '#':
                    devantMur = true;
                    break;
                case '$':
                    devantMur = true;
                    break;
                case '*':
                    devantMur = true;
                    break;
                case '.':
                    color = true;
                    devantMur = false;
                    break;
                default:
                    color = false;
                    devantMur = false;
                    break;
            }
        }

        //public void difficult()
        //{
        //    string ecrit = "quel difficulté vous voudriez ?";

        //    DialogResult rep = MessageBox.Show(ecrit, )
        //}
    }
}
