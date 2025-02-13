using System;
using System.ComponentModel;
using System.Numerics;


namespace AntSimulation
{
    public class Ant : Entity
    {
        public Vector2 Velocity { get; set; }
        public int ChasedFoodIndex { get; set; }
        public int ChasedFoodId { get; set; }
        public Vector2 Destination { get; set; }
        public bool HasFood { get; set; }
        public int WanderingCounter { get; set; }
        private static readonly Random Random = new Random();
        private Vector2 AntHill = GlobalVariables.AntHill;
        public Ant(Vector2 pos, Vector2 velocity) : base(pos)
        {
            ChasedFoodIndex = -1;
            Velocity = velocity;

        }

        public void Move()
        {
            if (HasFood && Vector2.DistanceSquared(AntHill, Pos) < GlobalVariables.FoodDropRadiusSquared)
            {
                // ant reached anthill
                HasFood = false;
                ChasedFoodIndex = -1;
                Destination = Vector2.Zero;
            }
            if (Destination == Vector2.Zero)
            {
                Velocity = Wander(Pos, Velocity);

            }
            else Velocity += SteerTowards(Pos, Velocity, Destination);
            Vector2 nextPos = Pos + Velocity;
            if (nextPos.X < 0) Velocity = new Vector2(Velocity.X * -1, Velocity.Y);
            if (nextPos.X > GlobalVariables.AreaWidth) Velocity = new Vector2(Velocity.X * -1, Velocity.Y);
            if (nextPos.Y < 0) Velocity = new Vector2(Velocity.X, Velocity.Y * -1);
            if (nextPos.Y > GlobalVariables.AreaHeight) Velocity = new Vector2(Velocity.X, Velocity.Y * -1);
            Pos += Velocity;

            
        }

        public Vector2 Wander(Vector2 position, Vector2 velocity)
        {
            return Turn(Random.Next(-GlobalVariables.AntTurnAngle, GlobalVariables.AntTurnAngle));
        }

        private Vector2 SteerTowards(Vector2 position, Vector2 velocity, Vector2 target)
        {
            // Step 1: Calculate the direction vector (corrected)
            Vector2 direction = target - position; // Fixed: should point towards the target

            // Step 2: Check if the target is already reached (avoid unnecessary calculations)
            float distanceSquared = direction.LengthSquared();
            if (distanceSquared < 0.01f) // Lowered threshold for better precision
                return Vector2.Zero; // No steering needed

            // Step 3: Normalize only if necessary
            if (distanceSquared > 1e-6f) // Avoid division by zero
                direction = Vector2.Normalize(direction);

            // Step 4: Calculate desired velocity in the target direction
            Vector2 desiredVelocity = direction * GlobalVariables.MaxSpeed;

            // Step 5: Calculate the steering force
            Vector2 steeringForce = desiredVelocity - velocity;

            // Step 6: Clamp steering force to maxForce limit (use squared length for efficiency)
            float steeringForceSquared = steeringForce.LengthSquared();
            float maxForceSquared = GlobalVariables.MaxForce * GlobalVariables.MaxForce;

            if (steeringForceSquared > maxForceSquared)
                steeringForce = (steeringForce / MathF.Sqrt(steeringForceSquared)) * GlobalVariables.MaxForce; // Normalize & scale

            return steeringForce;
        }
        public Vector2 Turn(float degrees)
        {
            float radians = MathF.PI * degrees / 180f;
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);
    
            return new Vector2(
                Velocity.X * cos - Velocity.Y * sin,
                Velocity.X * sin + Velocity.Y * cos
            );
        }
    }
}
