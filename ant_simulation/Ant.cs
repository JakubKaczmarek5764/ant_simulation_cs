using System;
using System.ComponentModel;


namespace AntSimulation
{
    public class Ant
    {
        public double X { get; set; }
        public double Y { get; set; }
        private static readonly Random Random = new Random();

        public Ant(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void Move()
        {
            X += Random.Next(-5, 6); // Move randomly in X
            Y += Random.Next(-5, 6); // Move randomly in Y
        }
    }
}
