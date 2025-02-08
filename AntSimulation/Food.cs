using System.Numerics;

namespace AntSimulation;

public class Food : Entity
{
    public int Id { get; set; }
    public Food(Vector2 pos, int id) : base(pos)
    {
        Id = id;
    }
}