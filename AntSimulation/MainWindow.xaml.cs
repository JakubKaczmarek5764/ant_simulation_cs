using System;
using System.Collections.Generic;
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
        private AntRenderer antRenderer;
        private DispatcherTimer timer;
        private int antCount = 100000; 
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
        private void SimulationCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            Point clickPosition = e.GetPosition(SimulationCanvas);
            foodManager.CreateFood(25, (clickPosition.X, clickPosition.Y), 50);
        
        }
        private void InitializeSimulation()
        {
            
            antRenderer = new AntRenderer(width, height);
            SimulationCanvas.Children.Add(antRenderer);
            antManager = new AntManager();
            foodManager = new FoodManager();
            pheromoneManager = new PheromoneManager();
            antManager.CreateAnts(antCount, (width / 2, height / 2));
            
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

            // Redraw ants
            antRenderer.Update(antManager.Ants, foodManager.Foods, pheromoneManager.Pheromones);
        }
    }

        public class AntRenderer : System.Windows.Controls.Image
        {
            private WriteableBitmap bitmap;
            private int width, height;
            private int[] pixels;

            public AntRenderer(int width, int height)
            {
                this.width = width;
                this.height = height;
                bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
                Source = bitmap;
                pixels = new int[width * height];
            }

            public void Update(List<Ant> ants, List<Food> foods, List<Pheromone> pheromones)
            {
                Array.Clear(pixels, 0, pixels.Length);
                
                foreach (var food in foods)
                {
                    DrawPoint(food.Pos, 2, Colors.Red);
                }
                foreach (var ant in ants)
                {
                    DrawPoint(ant.Pos, 3, Colors.White);
                }
                
                foreach (var pheromone in pheromones)
                {
                    DrawPoint(pheromone.Pos, 2, Colors.Blue);
                }
                bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);
            }
            public void DrawPoint((double x, double y) pos, int radius, int color)
            {
                int x = (int)pos.x;
                int y = (int)pos.y;

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
