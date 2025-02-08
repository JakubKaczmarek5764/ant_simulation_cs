namespace AntSimulation;

public class Colors
{
    public static readonly int Black = unchecked((int)0xFF000000);
    public static readonly int White = unchecked((int)0xFFFFFFFF);
    public static readonly int Red = unchecked((int)0xFFFF0000);
    public static readonly int Green = unchecked((int)0xFF00FF00);
    public static readonly int Blue = unchecked((int)0xFF0000FF);

}

public class GlobalVariables
{
    public static float maxForce = 100;
    public static float maxSpeed = 2;
    public static double FoodDetectionRadiusSquared = 1000;
    public static double PheromoneDetectionRadiusSquared = 200;
    public static double FoodPickupRadiusSquared = 100;
}