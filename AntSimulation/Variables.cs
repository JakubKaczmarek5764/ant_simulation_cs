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
    public static float MaxForce = .5f;
    public static readonly float MaxSpeed = 2;
    public static readonly double FoodDetectionRadiusSquared = Math.Pow(100, 2);
    public static readonly double PheromoneDetectionRadiusSquared = Math.Pow(200, 2);
    public static readonly double FoodPickupRadiusSquared = 100;
    public static readonly int PheromoneLifeTime = 300;
    public static readonly int PheromoneCooldown = 10;
    public static readonly double PheromoneCloseEnough = Math.Pow(20, 2);

}