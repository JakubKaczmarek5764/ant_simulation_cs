using System.Numerics;

namespace AntSimulation;

public class Pheromone : Entity
{
    public int Type {get;}
    public int LifeTime {get; set;}
    public Pheromone(Vector2 pos, int type, int lifeTime) : base(pos)
    {
        Type = type;
        LifeTime = lifeTime;
    }
    public void Decay()
    {
        LifeTime--;
    }
}