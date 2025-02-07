using System.Numerics;

namespace AntSimulation;

public class Entity
{
    public Vector2 Pos { get; protected set; }

    public Entity(Vector2 pos)
    {
        Pos = pos;
    }
}