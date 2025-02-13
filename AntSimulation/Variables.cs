using System;
using System.Numerics;

namespace AntSimulation;

public class Colors
{
    public static readonly int Black = unchecked((int)0xFF000000);
    public static readonly int White = unchecked((int)0xFFFFFFFF);
    public static readonly int Red = unchecked((int)0xFFFF0000);
    public static readonly int Green = unchecked((int)0xFF00FF00);
    public static readonly int Blue = unchecked((int)0xFF0000FF);
    public static readonly int Yellow = unchecked((int)0xFFFFFF00);
    public static readonly int Lime = unchecked((int)0xFFCBF542);
}

public class GlobalVariables
{
    public static readonly float MaxForce = 1.5f;
    public static readonly float MaxSpeed = 5;
    
    public static readonly double FoodDetectionRadiusSquared = Math.Pow(100, 2);
    public static readonly double FoodPickupRadiusSquared = Math.Pow(25, 2);
    public static readonly double FoodDropRadiusSquared = Math.Pow(50, 2);
    public static readonly int FoodSpawnCount = 10;
    public static readonly int FoodSpawnRadius = 50;
    
    
    public static readonly int AreaWidth = 1920;
    public static readonly int AreaHeight = 1080;
    public static readonly int GridWidth = 384;
    public static readonly int GridHeight = 216;
    
    public static readonly int PheromoneCooldown = 3;
    public static readonly int PheromoneTypesCount = 2;
    public static readonly int PheromoneAddIntensity = 100;
    public static readonly int PheromoneDetectionAngle = 30;
    public static readonly int PheromoneDetectionDistance = 70;
    public static readonly int PheromoneAreaSampleRadius = 50;
    public static readonly int PheromoneMaxIntensity = 1000;
    public static float PheromoneFollwingDecisionThreshold = 0.95f;
    
    public static readonly int AntWanderingCounter = 5;
    public static readonly int MaxAntTurnAngle = 20;
    public static Vector2 AntHill = new Vector2(AreaWidth / 8, AreaHeight / 8);
}