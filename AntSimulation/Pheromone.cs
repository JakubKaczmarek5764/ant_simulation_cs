namespace AntSimulation;

public class Pheromone : Entity
{
    private int Type {get;}

    public Pheromone((double x, double y) pos, int type) : base(pos)
    {
        Type = type;
    }
}