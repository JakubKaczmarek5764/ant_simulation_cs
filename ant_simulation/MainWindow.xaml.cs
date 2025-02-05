﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Runtime.InteropServices;

namespace AntSimulation
{
    public partial class MainWindow : Window
    {
        private List<Ant> ants = new List<Ant>();
        private AntRenderer antRenderer;
        private DispatcherTimer timer;
        private int antCount = 50_000; 
        private int width = 1920;
        private int height = 1080;
        public MainWindow()
        {
            InitializeComponent();
            InitializeSimulation();
        }

        private void InitializeSimulation()
        {
            
            antRenderer = new AntRenderer(width, height);
            SimulationCanvas.Children.Add(antRenderer);

            
            for (int i = 0; i < antCount; i++)
            {
                ants.Add(new Ant(width / 2, height / 2));
            }

            // Timer to update simulation
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000 / 60);
            timer.Tick += UpdateSimulation;
            timer.Start();
        }

        private void UpdateSimulation(object sender, EventArgs e)
        {
            // Move ants
            foreach (var ant in ants)
            {
                ant.Move();
            }

            // Redraw ants
            antRenderer.UpdateAnts(ants);
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

        public void UpdateAnts(List<Ant> ants)
        {
            Array.Clear(pixels, 0, pixels.Length);

            foreach (var ant in ants)
            {
                DrawAntAsCircle(ant, 3);
            }

            // Write pixels to bitmap
            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);
        }
        public void DrawAntAsCircle(Ant ant, int radius)
        {
            int x = (int)ant.X;
            int y = (int)ant.Y;
            int color = unchecked((int)0xFFFFFFFF);

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
