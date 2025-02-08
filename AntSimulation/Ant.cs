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
        public int ChasedPheromoneIndex { get; set; }
        public int ChasedPheromoneType { get; set; }
        public Vector2 Destination { get; set; }
        public bool HasFood { get; set; }
        private static readonly Random Random = new Random();

        public Ant(Vector2 pos, Vector2 velocity) : base(pos)
        {
            ChasedFoodIndex = -1;
            ChasedPheromoneIndex = -1;
            Velocity = velocity;

        }

        public void Move()
        {
            if (Destination == Vector2.Zero)
            {
                Velocity = Wander(Pos, Velocity);

            }
            else Velocity = SteerTowards(Pos, Velocity, Destination);
            Pos += Velocity;
            
        }

        public Vector2 Wander(Vector2 position, Vector2 velocity)
        {

            float angle = (float)(Random.NextDouble() * Math.PI * 2); // Random angle (0 to 360 degrees)
            Vector2 randomSteering =
                new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * GlobalVariables.MaxForce;

            // Step 2: Apply steering force (without unnecessary normalizations)
            Vector2 newVelocity = velocity + randomSteering;

            // Step 3: Limit velocity to maxSpeed (only normalize if necessary)
            float speed = newVelocity.Length();
            if (speed > GlobalVariables.MaxSpeed)
            {
                newVelocity *= GlobalVariables.MaxSpeed / speed; // Scale down instead of normalizing
            }

            // Step 4: Return new velocity (position should be updated externally)
            return newVelocity;
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

    }
}
