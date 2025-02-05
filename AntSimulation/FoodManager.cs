using System;
using System.Collections.Generic;

namespace AntSimulation;

public class FoodManager : Manager
{
    private List<Food> foods = new List<Food>();
    private static readonly Random random = new Random();
    public ((double, double), double, int) FindClosestFood((double x, double y) pos)
    {
        double minDistance = int.MaxValue;
        int closestFoodIndex = -1;
        for (int i = 0; i < foods.Count; i++)
        {
            Food food = foods[i];
            double curDist = sq_distance(food.Pos.x, food.Pos.y, pos.x, pos.y);
            if (curDist < minDistance)
            {
                minDistance = curDist;
                closestFoodIndex = i;
            }
        }
        return (foods[closestFoodIndex].Pos, minDistance, closestFoodIndex);
    }
    public void CreateFood(int foodCount, (double x, double y) pos, int radius = 0)
    {
        for (int i = 0; i < foodCount; i++)
        {
            foods.Add(new Food((random.NextDouble() * radius * 2 + pos.x, random.NextDouble() * radius * 2 + pos.y)));
        }
        Console.WriteLine(foods.Count);
    }
    public void DeleteFood(int foodIndex)
    {
        foods.RemoveAt(foodIndex);
    }
    
}