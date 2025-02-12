using System.Collections.Generic;
using System.Numerics;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace AntSimulation;

public class PheromoneManager : Manager
{
    public Grid Grid = new Grid();
    private static PheromoneManager _instance;
    public static PheromoneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PheromoneManager();
            }
            return _instance;
        }
    }
    private PheromoneManager()
    {

    }
    public void AddIntensity(Vector2 pos, int type, int intensity)
    {
        Grid.AddIntensity(pos, type, intensity);
    }
    public double GetAverageIntensity(Vector2 pos, int type, double radius)
    {
        return Grid.GetAverageIntensity(pos, type, radius);
    }
    public double GetIntensity(int cellX, int cellY, int type)
    {
        return Grid.GetIntensity(cellX, cellY, type);
    }
    public void Decay()
    {
        Grid.Decay();
    }
    
}