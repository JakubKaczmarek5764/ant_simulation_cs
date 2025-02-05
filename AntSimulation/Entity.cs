namespace AntSimulation;

public class Entity
{
    public (double x, double y) Pos { get; protected set; }

    public Entity((double x, double y) pos)
    {
        Pos = pos;
    }
}