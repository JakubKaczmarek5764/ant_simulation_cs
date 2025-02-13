using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace AntSimulation;

public class AntManager : Manager
{
    public List<Ant?> Ants = new List<Ant?>();
    private static readonly Random Random = new Random();
    private static AntManager _instance;
    private FoodManager foodManager = FoodManager.Instance;
    private PheromoneManager pheromoneManager = PheromoneManager.Instance;
    private int _pheromoneCooldown = GlobalVariables.PheromoneCooldown;
    public static AntManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AntManager();
            }
            return _instance;
        }
    }
    private AntManager()
    {
        
    }
    public void CreateAnts(int antCount, Vector2 pos)
    {
        for (int i = 0; i < antCount; i++)
        {
            Vector2 randomVelocity =
                RandomUnitVector() * GlobalVariables.MaxSpeed;
            Ants.Add(new Ant(pos, randomVelocity));
        }
    }
    public static Vector2 RandomUnitVector()
    {
        double angle = Random.NextDouble() * 2 * Math.PI;
        return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
    }
    public void NextFrame()
    {
        foreach (var ant in Ants) // Chasing food and pheromones
        {
            
            if (!ant.HasFood && !foodManager.IsEmpty())
            {
                TryToChaseFood(ant);
            }

            if (ant.WanderingCounter <= 0 && Random.NextDouble() > GlobalVariables.PheromoneFollwingDecisionThreshold)
            {
                ant.WanderingCounter = GlobalVariables.AntWanderingCounter;
            }
            else if (ant.Destination.Equals(Vector2.Zero) && ant.WanderingCounter <= 0)
            {
                TryToChasePheromone(ant);
            }
            ant.WanderingCounter--;
            ant.Move();
        }
        foreach (var ant in Ants) // Verifying if food has been picked up
        {
            if (ant.ChasedFoodIndex != -1 && (foodManager.IsNull(ant.ChasedFoodIndex) || ant.ChasedFoodId == foodManager.GetFoodId(ant.ChasedFoodIndex)))
            {
                ant.ChasedFoodIndex = -1;
                ant.ChasedFoodId = -1;
                ant.Destination = Vector2.Zero;
            }

        }

        if (_pheromoneCooldown == 0)
        {
            foreach (var ant in Ants) // Leaving pheromones
            {
                pheromoneManager.AddIntensity(ant.Pos, ant.HasFood ? 1 : 0, GlobalVariables.PheromoneAddIntensity);
            }
            _pheromoneCooldown = GlobalVariables.PheromoneCooldown;
        }
        _pheromoneCooldown--;
    }
    private void TryToChaseFood(Ant ant)
    {
        (double dist, int foodIndex, Vector2 foodPos) = foodManager.FindClosestFood(ant.Pos);
        if (dist > GlobalVariables.FoodDetectionRadiusSquared) return;
        ant.ChasedFoodIndex = foodIndex;
        ant.Destination = foodPos;
        ant.ChasedFoodId = foodManager.GetFoodId(foodIndex);
        
        if (dist > GlobalVariables.FoodPickupRadiusSquared) return;
        foodManager.PickupFood(foodIndex);
        ant.HasFood = true;
        ant.ChasedFoodIndex = -1;
        ant.Destination = Vector2.Zero;
        
    }
    private void TryToChasePheromone(Ant ant)
    {
        Vector2 front = ant.Pos + GetDirection(ant.Velocity, 0) * GlobalVariables.PheromoneDetectionDistance;
        Vector2 left = ant.Pos + GetDirection(ant.Velocity, -GlobalVariables.PheromoneDetectionAngle) * GlobalVariables.PheromoneDetectionDistance;
        Vector2 right = ant.Pos + GetDirection(ant.Velocity, GlobalVariables.PheromoneDetectionAngle) * GlobalVariables.PheromoneDetectionDistance;
        
        double frontIntensity = pheromoneManager.GetAverageIntensity(front, ant.HasFood ? 0 : 1, GlobalVariables.PheromoneAreaSampleRadius);
        double leftIntensity = pheromoneManager.GetAverageIntensity(left, ant.HasFood ? 0 : 1, GlobalVariables.PheromoneAreaSampleRadius);
        double rightIntensity = pheromoneManager.GetAverageIntensity(right, ant.HasFood ? 0 : 1, GlobalVariables.PheromoneAreaSampleRadius);
        if (frontIntensity == 0 && leftIntensity == 0 && rightIntensity == 0)
        {
            return;
        }
        if (frontIntensity > leftIntensity && frontIntensity > rightIntensity)
        {
            // Chase front
            
        }
        else if (leftIntensity > frontIntensity && leftIntensity > rightIntensity)
        {
            // Chase left
            ant.Velocity = ant.Turn(-GlobalVariables.AntTurnAngle);
        }
        else if (rightIntensity > frontIntensity && rightIntensity > leftIntensity)
        {
            // Chase right
            ant.Velocity = ant.Turn(GlobalVariables.AntTurnAngle);

        }

    }

    private static Vector2 VelocityTowards(Vector2 pos, Vector2 target, float length)
    {
        return Vector2.Normalize(target - pos) * length;
    }
    

    private static Vector2 GetDirection(Vector2 velocity, float angleDegrees)
    {
        if (velocity.LengthSquared() == 0) return Vector2.UnitX; // Prevent divide by zero

        velocity = Vector2.Normalize(velocity); // Normalize to get direction

        float angleRadians = MathF.PI * angleDegrees / 180f; // Convert to radians
        float cosA = MathF.Cos(angleRadians);
        float sinA = MathF.Sin(angleRadians);

        // Apply 2D rotation matrix:
        return new Vector2(
            velocity.X * cosA - velocity.Y * sinA,
            velocity.X * sinA + velocity.Y * cosA
        );
    }
}