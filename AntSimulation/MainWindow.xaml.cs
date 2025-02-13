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
        private int antCount = 3000; 
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
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Label != null)
            {
                GlobalVariables.PheromoneFollwingDecisionThreshold = (float)e.NewValue; 
                Label.Text = $"Threshold: {GlobalVariables.PheromoneFollwingDecisionThreshold}";
            }
            
        }
        private void SimulationCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            Point clickPosition = e.GetPosition(SimulationCanvas);
            foodManager.CreateFood(GlobalVariables.FoodSpawnCount, new Vector2((float)clickPosition.X, (float)clickPosition.Y), GlobalVariables.FoodSpawnRadius);
        
        }
        private void InitializeSimulation()
        {
            
            _renderer = new Renderer(width, height);
            SimulationCanvas.Children.Add(_renderer);
            antManager = AntManager.Instance;
            foodManager = FoodManager.Instance;
            pheromoneManager = PheromoneManager.Instance;
            antManager.CreateAnts(antCount, GlobalVariables.AntHill);
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
            
            pheromoneManager.Decay();
            foodManager.Clear();
            // Redraw ants
            _renderer.Update(antManager.Ants, foodManager.Foods, pheromoneManager.Grid);
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

            public void Update(List<Ant?> ants, List<Food?> foods, Grid pheromones)
            {
                Array.Clear(pixels, 0, pixels.Length);
                for (int i = 0; i < GlobalVariables.GridWidth; i++)
                {
                    for (int j = 0; j < GlobalVariables.GridHeight; j++)
                    {
                        int color = pheromones.GetColor(i, j);
                        int cellWidth = (int)(GlobalVariables.AreaWidth / GlobalVariables.GridWidth);
                        int cellHeight = (int)(GlobalVariables.AreaHeight / GlobalVariables.GridHeight);
                
                        for (int dx = 0; dx < cellWidth; dx++)
                        {
                            for (int dy = 0; dy < cellHeight; dy++)
                            {
                                int px = i * cellWidth + dx;
                                int py = j * cellHeight + dy;
                        
                                if (px >= 0 && px < width && py >= 0 && py < height)
                                {
                                    pixels[py * width + px] = color;
                                }
                            }
                        }
                    }
                }
                foreach (var food in foods)
                {
                    DrawPoint(food.Pos, 2, Colors.Red);
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
