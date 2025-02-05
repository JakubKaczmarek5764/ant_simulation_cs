using System;
using System.Collections.Generic;

namespace AntSimulation;

public class AntManager : Manager
{
    public List<Ant> Ants = new List<Ant>();
    private Random _random = new Random();
    public void CreateAnts(int antCount, (double x, double y) pos)
    {
        for (int i = 0; i < antCount; i++)
        {
            (double, double) velocity = (_random.NextDouble() * 20 - 10, _random.NextDouble() * 20 - 10);
            Ants.Add(new Ant(pos, velocity));
        }
    }
    public void NextFrame()
    {
        foreach (var ant in Ants)
        {
            ant.Move();
        }
    }
    
}