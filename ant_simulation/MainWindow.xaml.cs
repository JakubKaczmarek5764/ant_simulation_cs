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
using System.Windows.Threading;

namespace AntSimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
public partial class MainWindow : Window
    {
        private List<Ant> ants = new List<Ant>();
        private List<Ellipse> antShapes = new List<Ellipse>();
        private DispatcherTimer timer;
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            InitializeSimulation();
        }

        private void InitializeSimulation()
        {
            int antCount = 100;
            int height = 1080;
            int width = 1920;
            for (int i = 0; i < antCount; i++)
            {
                var ant = new Ant(width / 2, height / 2);
                ants.Add(ant);

                // Create a UI representation (Ellipse) for each ant
                Ellipse ellipse = new Ellipse
                {
                    Width = 5,
                    Height = 5,
                    Fill = Brushes.White
                };
                SimulationCanvas.Children.Add(ellipse);
                antShapes.Add(ellipse);
            }

            // Timer to update simulation
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000 / 60);
            timer.Tick += UpdateSimulation;
            timer.Start();
        }

        private void UpdateSimulation(object sender, EventArgs e)
        {
            for (int i = 0; i < ants.Count; i++)
            {
                ants[i].Move(); // Update ant's position

                // Update UI position
                Canvas.SetLeft(antShapes[i], ants[i].X);
                Canvas.SetTop(antShapes[i], ants[i].Y);
            }
        }
    }
}