using System;
using System.Collections.Generic;
using AntSimulation;

namespace AntSimulation;

public class AntManager : Manager
{
    public List<Ant> ants = new List<Ant>();
    private Random random = new Random();
    public void CreateAnts(int antCount, (double x, double y) pos)
    {
        Console.WriteLine("CreateAnts");
        for (int i = 0; i < antCount; i++)
        {
            (double, double) velocity = (random.NextDouble() * 2 - 1, random.NextDouble() * 2 - 1);
            ants.Add(new Ant(pos, velocity));
        }
    }
    public void NextFrame(Object sender, EventArgs e)
    {
        foreach (var ant in ants)
        {
            ant.Move();
        }
    }
    
}