namespace AntSimulation;

public class AntHillCell : Cell
{
    public AntHillCell(int typesCount) : base(typesCount)
    {
        PheromoneIntensities[0] = GlobalVariables.PheromoneMaxIntensity;
        PheromoneIntensities[1] = 0;
    }
    public override void Decay()
    {
        
    }
    
    public override int GetColor()
    {
        return Colors.Lime;
    }
}   