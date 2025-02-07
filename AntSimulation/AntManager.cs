using System;
using System.Collections.Generic;
using System.Numerics;

namespace AntSimulation;

public class AntManager : Manager
{
    public List<Ant> Ants = new List<Ant>();
    private static readonly Random Random = new Random();
    private static AntManager _instance;
    private FoodManager foodManager = FoodManager.Instance;
    private PheromoneManager pheromoneManager = PheromoneManager.Instance;
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

    public void CreateAnts(int antCount, Vector2 pos, double speed = 2, double foodDetectionRadius = 100, double pheromoneDetectionRadius = 200)
    {
        for (int i = 0; i < antCount; i++)
        {
            Vector2 randomVelocity =
                new Vector2((float)Random.NextDouble() * 20 - 10, (float)Random.NextDouble() * 20 - 10);
            Ants.Add(new Ant(pos, randomVelocity, foodDetectionRadius, pheromoneDetectionRadius));
        }
    }
    public void NextFrame()
    {
        foreach (var ant in Ants)
        {
            if (ant.ChasedFoodIndex == -1 && !foodManager.IsEmpty())
            {
                TryToChaseFood(ant);
            }
            if (ant.ChasedFoodIndex == -1 && !pheromoneManager.IsEmpty())
            {
                TryToChasePheromone(ant);
            }
            ant.Move();
            if (ant.ChasedFoodIndex != -1 && foodManager.IsNull(ant.ChasedFoodIndex)) ant.ChasedFoodIndex = -1;
            else if (ant.ChasedPheromoneIndex != -1 && pheromoneManager.HasDecayed(ant.ChasedPheromoneIndex)) ant.ChasedPheromoneIndex = -1;
            foodManager.Clear();
            pheromoneManager.Clear();

        }
    }
    private void TryToChaseFood(Ant ant)
    {
        (double dist, int foodIndex, Vector2 foodPos) = foodManager.FindClosestFood(ant.Pos);
        if (dist < ant.FoodDetectionRadius) return;
        ant.ChasedFoodIndex = foodIndex;
        ant.Destination = foodPos;
        ant.ChasedPheromoneIndex = -1;
    }
    private void TryToChasePheromone(Ant ant)
    {
        (double dist, int pheromoneIndex, Vector2 pheromonePos) = pheromoneManager.FindClosestPheromone(ant.Pos);
        if (dist < ant.PheromoneDetectionRadius) return;
        ant.ChasedPheromoneIndex = pheromoneIndex;
        ant.Destination = pheromonePos;
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