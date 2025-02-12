using System.Numerics;

public class Entity
{
    public Vector2 Pos { get; protected set; } = new Vector2(0, 0);

    public Entity(Vector2 pos)
    {
        Pos = pos;
    }
}