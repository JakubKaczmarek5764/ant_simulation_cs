using System;
using System.Collections.Generic;
using System.Numerics;

namespace AntSimulation;

public class FoodManager : Manager
{
    public List<Food> Foods = new List<Food>();
    private static readonly Random Random = new Random();
    private static FoodManager _instance;
    public static FoodManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new FoodManager();
            }
            return _instance;
        }
    }
    public (double, int, Vector2) FindClosestFood(Vector2 antPos)
    {
        double minDistance = int.MaxValue;
        int closestFoodIndex = -1;
        for (int i = 0; i < Foods.Count; i++)
        {
            Food food = Foods[i];
            double curDist = Vector2.Distance(food.Pos, antPos);
            if (curDist < minDistance)
            {
                minDistance = curDist;
                closestFoodIndex = i;
            }
        }
        return (minDistance, closestFoodIndex, Foods[closestFoodIndex].Pos);
    }
    public void CreateFood(int foodCount, (double x, double y) pos, int radius = 0)
    {
        for (int i = 0; i < foodCount; i++)
        {
            Foods.Add(new Food(new Vector2((float)(Random.NextDouble() * radius * 2 + pos.x - radius), (float)(Random.NextDouble() * radius * 2 + pos.y - radius))));
        }
        Console.WriteLine(Foods.Count);
    }
    public bool IsNull(int foodIndex)
    {
        return Foods[foodIndex] == null;
    }

    public void Clear()
    {
        Foods.RemoveAll(x => x == null);
    }
    public bool IsEmpty()
    {
        return Foods.Count == 0;
    }
}