using System;
using System.ComponentModel;


namespace AntSimulation
{
    public class Ant : Entity
    {
        public (double x, double y) Velocity { get; set; }
        private static readonly Random Random = new Random();

        public Ant((double x, double y) pos, (double x, double y) velocity) : base(pos)
        {
            Velocity = velocity;
        }

        public void Move()
        {
            Pos = (Pos.x + Velocity.x, Pos.y + Velocity.y);
        }
        
    }
}
