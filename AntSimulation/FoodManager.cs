using System;
using System.Collections.Generic;

namespace AntSimulation;

public class FoodManager : Manager
{
    public List<Food> Foods = new List<Food>();
    private static readonly Random Random = new Random();
    public ((double, double), double, int) FindClosestFood((double x, double y) pos)
    {
        double minDistance = int.MaxValue;
        int closestFoodIndex = -1;
        for (int i = 0; i < Foods.Count; i++)
        {
            Food food = Foods[i];
            double curDist = sq_distance(food.Pos.x, food.Pos.y, pos.x, pos.y);
            if (curDist < minDistance)
            {
                minDistance = curDist;
                closestFoodIndex = i;
            }
        }
        return (Foods[closestFoodIndex].Pos, minDistance, closestFoodIndex);
    }
    public void CreateFood(int foodCount, (double x, double y) pos, int radius = 0)
    {
        for (int i = 0; i < foodCount; i++)
        {
            Foods.Add(new Food((Random.NextDouble() * radius * 2 + pos.x - radius, Random.NextDouble() * radius * 2 + pos.y - radius)));
        }
        Console.WriteLine(Foods.Count);
    }
    public void DeleteFood(int foodIndex)
    {
        Foods.RemoveAt(foodIndex);
    }
    
}