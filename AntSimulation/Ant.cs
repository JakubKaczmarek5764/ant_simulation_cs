using System;
using System.ComponentModel;
using System.Numerics;


namespace AntSimulation
{
    public class Ant : Entity
    {
        public Vector2 Velocity { get; set; }
        public int ChasedFoodIndex { get; set; }
        public int ChasedPheromoneIndex { get; set; }
        public Vector2 Destination { get; set; }
        public double FoodDetectionRadius { get; set; }
        public double PheromoneDetectionRadius { get; set; }
        public float MaxSpeed { get; set; }
        public float MaxForce { get; set; }
        private static readonly Random Random = new Random();
        public Ant(Vector2 pos, Vector2 velocity, double foodDetectionRadius, double pheromoneDetectionRadius, float maxSpeed = 2) : base(pos)
        {
            ChasedFoodIndex = -1;
            ChasedPheromoneIndex = -1;
            Velocity = velocity;
            FoodDetectionRadius = foodDetectionRadius;
            PheromoneDetectionRadius = pheromoneDetectionRadius;
            MaxSpeed = maxSpeed;
        }

        public void Move()
        {

            if (Destination == Vector2.Zero)
            {
                Pos += Wander(Pos, Velocity, MaxSpeed);
                
            }
            else Pos += SteerTowards(Pos, Velocity, Destination, MaxSpeed, MaxForce);
        }
        public Vector2 Wander(Vector2 position, Vector2 velocity, float maxSpeed)
        {

            float angle = (float)(Random.NextDouble() * Math.PI * 2); // Random angle (0 to 360 degrees)
            Vector2 randomSteering = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * GlobalVariables.maxForce;

            // Step 2: Apply steering force (without unnecessary normalizations)
            Vector2 newVelocity = velocity + randomSteering;

            // Step 3: Limit velocity to maxSpeed (only normalize if necessary)
            float speed = newVelocity.Length();
            if (speed > maxSpeed)
            {
                newVelocity *= maxSpeed / speed; // Scale down instead of normalizing
            }

            // Step 4: Return new velocity (position should be updated externally)
            return newVelocity;
        }
        private Vector2 SteerTowards(Vector2 position, Vector2 velocity, Vector2 target, float maxSpeed, float maxForce = 0.5f, float deltaTime = 0.016f)
        {
            // Compute desired velocity (direction towards target)
            Vector2 desiredVelocity = target - position;
            if (desiredVelocity.Length() > 0)
            {
                desiredVelocity = Vector2.Normalize(desiredVelocity) * maxSpeed; // Normalize and scale to max speed
            }

            // Compute steering force
            Vector2 steeringForce = desiredVelocity - velocity;
            if (steeringForce.Length() > maxForce)
            {
                steeringForce = Vector2.Normalize(steeringForce) * maxForce; // Limit to max force
            }

            // Apply steering force to velocity
            Vector2 newVelocity = velocity + steeringForce * deltaTime;
            if (newVelocity.Length() > maxSpeed)
            {
                newVelocity = Vector2.Normalize(newVelocity) * maxSpeed; // Clamp to max speed
            }

            // Return the new velocity
            return newVelocity;
        }
    }
    
}
