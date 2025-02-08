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

    public void CreateAnts(int antCount, Vector2 pos)
    {
        for (int i = 0; i < antCount; i++)
        {
            Vector2 randomVelocity =
                new Vector2((float)Random.NextDouble() * 20 - 10, (float)Random.NextDouble() * 20 - 10);
            Ants.Add(new Ant(pos, randomVelocity));
        }
    }
    public void NextFrame()
    {
        foreach (var ant in Ants) // Chasing food and pheromones
        {

            if (!ant.HasFood && !foodManager.IsEmpty())
            {
                TryToChaseFood(ant);
            }
            if (!ant.HasFood && !pheromoneManager.IsEmpty(1))
            {
                TryToChasePheromone(ant, 1);
            }

            if (ant.HasFood && !pheromoneManager.IsEmpty(0))
            {
                TryToChasePheromone(ant, 0);
            }
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
            else if (ant.ChasedPheromoneIndex != -1 &&
                     pheromoneManager.HasDecayed(ant.ChasedPheromoneIndex, ant.ChasedPheromoneType))
            {
                ant.ChasedPheromoneIndex = -1;
                ant.ChasedPheromoneType = -1;
                ant.Destination = Vector2.Zero;
            }
        }

        if (_pheromoneCooldown == 0)
        {
            foreach (var ant in Ants) // Leaving pheromones
            {
                pheromoneManager.CreatePheromone(ant.Pos, ant.HasFood ? 1 : 0);
            }
            _pheromoneCooldown = GlobalVariables.PheromoneCooldown;
        }
        foodManager.Clear();
        pheromoneManager.Clear();
        _pheromoneCooldown--;
    }
    private void TryToChaseFood(Ant ant)
    {
        
        (double dist, int foodIndex, Vector2 foodPos) = foodManager.FindClosestFood(ant.Pos);
        
        if (dist > GlobalVariables.FoodDetectionRadiusSquared) return;
        ant.ChasedFoodIndex = foodIndex;
        ant.Destination = foodPos;
        ant.ChasedPheromoneIndex = -1;
        ant.ChasedFoodId = foodManager.GetFoodId(foodIndex);
        
        if (dist > GlobalVariables.FoodPickupRadiusSquared) return;
        foodManager.PickupFood(foodIndex);
        ant.HasFood = true;
        ant.ChasedFoodIndex = -1;
        ant.Destination = Vector2.Zero;
        
    }
    private void TryToChasePheromone(Ant ant, int type)
    {
        (double dist, int pheromoneIndex, Vector2 pheromonePos) = pheromoneManager.FindClosestPheromone(ant.Pos, type);
        if (dist < GlobalVariables.PheromoneDetectionRadiusSquared) return;
        ant.ChasedPheromoneIndex = pheromoneIndex;
        ant.Destination = pheromonePos;
        ant.ChasedPheromoneType = type;
    }
            /*
             * mrowki beda przechowywac wskaznik do elementu w tablicy
             * w tej funkcji bedzie sprawdzane czy element w tablicy nie jest nullem, jezeli jest to bedzie szukany nowy food
             * mrowka jak podniesie jedzenie to bedzie ustawiac element w tablicy na null
             * w food manager bedzie funkcja ktora bedzie czyscic tablice z nulli, ale bedzie sie to dzialo dopiero po calej petli w tej funkcji, na samym koncu
             * w ten sposob bedziemy regularnie czyscic tablice i aktualizowac indexy
             * 
             */
            
            /*
             * jezeli chodzi o feromony to tak samo mrowka bedzie przechowywac wskaznik do elementu w tablicy
             * jezeli wsaznik bedzie nullem to bedzie szukany nowy feromon
             * do feromonow mozna uzywac spatial partitioning albo quadtree jak sie okaze ze mozna
             * 
             */
}