using System;

namespace AntSimulation;

public class Colors
{
    public static readonly int Black = unchecked((int)0xFF000000);
    public static readonly int White = unchecked((int)0xFFFFFFFF);
    public static readonly int Red = unchecked((int)0xFFFF0000);
    public static readonly int Green = unchecked((int)0xFF00FF00);
    public static readonly int Blue = unchecked((int)0xFF0000FF);
    public static readonly int Yellow = unchecked((int)0xFFFFFF00);
}

public class GlobalVariables
{
    public static float MaxForce = 1.5f;
    public static readonly float MaxSpeed = 3;
    public static readonly double FoodDetectionRadiusSquared = Math.Pow(100, 2);
    public static readonly double FoodPickupRadiusSquared = Math.Pow(25, 2);
    public static readonly double FoodDropRadiusSquared = Math.Pow(100, 2);
    public static readonly int PheromoneCooldown = 5;
    public static readonly int AreaWidth = 1920;
    public static readonly int AreaHeight = 1080;
    public static readonly int GridWidth = 384;
    public static readonly int GridHeight = 216;
    public static readonly int PheromoneTypesCount = 2;
    public static readonly int PheromoneAddIntensity = 50;
    public static readonly int AntTurnAngle = 30;
    public static readonly int PheromoneDetectionAngle = 30;
    public static readonly int PheromoneDetectionDistance = 20;
    public static readonly int PheromoneAreaSampleRadius = 30;
    public static readonly int PheromoneMaxIntensity = 1000;
}