using System;
using System.Collections.Generic;
using System.Numerics;

namespace AntSimulation;

public class FoodManager : Manager
{
    public List<Food?> Foods = new List<Food?>();
    private static readonly Random Random = new Random();
    private static FoodManager _instance;
    private int _nextFoodId;
    public int FoodCount { get; private set; }
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
    private FoodManager()
    {
        
    }
    public (double, int, Vector2) FindClosestFood(Vector2 antPos)
    {
        double minDistance = int.MaxValue;
        int closestFoodIndex = -1;
        for (int i = 0; i < Foods.Count; i++)
        {
            if (Foods[i] == null) continue;
            Food food = Foods[i];
            double curDist = Vector2.DistanceSquared(food.Pos, antPos);
            if (curDist < minDistance)
            {
                minDistance = curDist;
                closestFoodIndex = i;
            }
        }
        return (minDistance, closestFoodIndex, Foods[closestFoodIndex].Pos);
    }
    public void CreateFood(int foodCount, Vector2 pos, int radius = 0)
    {
        for (int i = 0; i < foodCount; i++)
        {
            Foods.Add(new Food(new Vector2((float)(Random.NextDouble() * radius * 2 + pos.X - radius), (float)(Random.NextDouble() * radius * 2 + pos.Y - radius)), _nextFoodId));
            _nextFoodId++;
            FoodCount++;
        }
    }
    public bool IsNull(int foodIndex)
    {
        return foodIndex + 1 > Foods.Count || Foods[foodIndex] == null;
    }

    public void Clear()
    {
        Foods.RemoveAll(x => x == null);
    }
    public bool IsEmpty()
    {
        return FoodCount == 0;
    }
    public void PickupFood(int foodIndex)
    {
        Foods[foodIndex] = null;
        FoodCount--;
        
    }
    public void PrintFoods()
    {
        foreach (var food in Foods)
        {
            Console.WriteLine(food);
        }
    }
    public int GetFoodId(int foodIndex)
    {
        return Foods[foodIndex].Id;
    }
}