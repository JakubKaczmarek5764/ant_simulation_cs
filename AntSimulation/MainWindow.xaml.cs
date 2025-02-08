using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace AntSimulation
{
    public partial class MainWindow : Window
    {
        private Renderer _renderer;
        private DispatcherTimer timer;
        private int antCount = 10; 
        private int width = 1920;
        private int height = 1080;
        private AntManager antManager;
        private FoodManager foodManager;
        private PheromoneManager pheromoneManager;
        public MainWindow()
        {
            InitializeComponent();
            InitializeSimulation();
        }
        private void ForceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ForceLabel != null)
            {
                GlobalVariables.MaxForce = (float)e.NewValue; // Update the global variable
                ForceLabel.Text = $"Max Force: {GlobalVariables.MaxForce}"; // Update the label to reflect the new value
            }
            
        }
        private void SimulationCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            Point clickPosition = e.GetPosition(SimulationCanvas);
            foodManager.CreateFood(25, new Vector2((float)clickPosition.X, (float)clickPosition.Y), 50);
        
        }
        private void InitializeSimulation()
        {
            
            _renderer = new Renderer(width, height);
            SimulationCanvas.Children.Add(_renderer);
            antManager = AntManager.Instance;
            foodManager = FoodManager.Instance;
            pheromoneManager = PheromoneManager.Instance;
            antManager.CreateAnts(antCount, new Vector2(width / 2, height / 2));
            // Timer to update simulation
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000 / 60);
            timer.Tick += UpdateSimulation;
            timer.Start();
        }

        private void UpdateSimulation(object sender, EventArgs e)
        {
            // Move ants
            antManager.NextFrame();
            
            pheromoneManager.DecayPheromones();
            // Redraw ants
            _renderer.Update(antManager.Ants, foodManager.Foods, pheromoneManager.PheromonesLists);
        }
    }

        public class Renderer : System.Windows.Controls.Image
        {
            private WriteableBitmap bitmap;
            private int width, height;
            private int[] pixels;

            public Renderer(int width, int height)
            {
                this.width = width;
                this.height = height;
                bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
                Source = bitmap;
                pixels = new int[width * height];
            }

            public void Update(List<Ant?> ants, List<Food?> foods, List<List<Pheromone?>> pheromones)
            {
                Array.Clear(pixels, 0, pixels.Length);
                foreach (var food in foods)
                {
                    DrawPoint(food.Pos, 2, Colors.Red);
                }

                for (int i = 0; i < pheromones.Count; i++)
                {
                    int color = i == 0 ? Colors.Blue : Colors.Green;
                    foreach (var pheromone in pheromones[i])
                    {
                        DrawPoint(pheromone.Pos, 2, color);
                    }
                }
                
                foreach (var ant in ants)
                {
                    DrawPoint(ant.Pos, 3, ant.HasFood? Colors.Yellow : Colors.White);
                }
                bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);
            }
            public void DrawPoint(Vector2 pos, int radius, int color)
            {
                int x = (int)pos.X;
                int y = (int)pos.Y;

                for (int dx = -radius; dx <= radius; dx++)
                {
                    for (int dy = -radius; dy <= radius; dy++)
                    {
                        if (dx * dx + dy * dy <= radius * radius)
                        {
                            int nx = x + dx;
                            int ny = y + dy;

                            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                            {
                                pixels[ny * width + nx] = color;
                            }
                        }
                    }
                }
            }
        }
    
}
