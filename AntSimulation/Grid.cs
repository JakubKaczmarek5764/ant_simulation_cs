using System;
using System.Collections.Generic;
using System.Numerics;
using Vector = System.Windows.Vector;

namespace AntSimulation;

public class Grid
{
    // zrobic cell jako parent class i zrobic anthill cell ktory bedzie zwracac zawsze max intensity i get color bedzie jako kolor anthilla z globalnych zmiennych czy cos
    private List<List<Cell>> _grid = new List<List<Cell>>();
    private int _width;
    private int _height;
    private int _typesCount;
    private double _cellHeight;
    private double _cellWidth;
    public Grid()
    {
        
        _width = GlobalVariables.GridWidth;
        _height = GlobalVariables.GridHeight;
        _cellWidth = GlobalVariables.AreaWidth / _width;
        _cellHeight = GlobalVariables.AreaHeight / _height;
        _typesCount = GlobalVariables.PheromoneTypesCount;
        for (int i = 0; i < _width; i++)
        {
            _grid.Add(new List<Cell>());
            for (int j = 0; j < _height; j++)
            {
                Vector2 cellCenter = new Vector2((float)(i * _cellHeight + _cellHeight / 2),
                    (float)(j * _cellWidth + _cellWidth / 2));
                if (Vector2.DistanceSquared(cellCenter, GlobalVariables.AntHill) < GlobalVariables.FoodDropRadiusSquared)
                {
                    _grid[i].Add(new AntHillCell(_typesCount));
                }
                else _grid[i].Add(new PheromoneCell(_typesCount));
                
            }
        }

    }

    public void Decay()
    {
        foreach (var row in _grid)
        {
            foreach (var cell in row)
            {
                cell.Decay();
            }
        }
    }

    public double GetAverageIntensity(Vector2 pos, int type, double radius)
    {
        int farLeftCellX = Math.Max((int)((pos.X - radius) / _cellWidth), 0);
        int farRightCellX = Math.Min((int)((pos.X + radius) / _cellWidth), _width - 1);
        int farTopCellY = Math.Max((int)((pos.Y - radius) / _cellHeight), 0);
        int farBottomCellY = Math.Min((int)((pos.Y + radius) / _cellHeight), _height - 1);

        double totalIntensity = 0;
        for (int i = farLeftCellX; i <= farRightCellX; i++)
        {
            for (int j = farTopCellY; j <= farBottomCellY; j++)
            {
                totalIntensity += _grid[i][j].GetIntensity(type);
            }
        }
        return totalIntensity / ((farRightCellX - farLeftCellX + 1) * (farBottomCellY - farTopCellY + 1));
        
    }
    public void AddIntensity(Vector2 pos, int type, int intensity)
    {
        
        int cellX = (int)(pos.X / _cellWidth);
        int cellY = (int)(pos.Y / _cellHeight);
        if (cellX < 0 || cellX >= _width || cellY < 0 || cellY >= _height) return;
        _grid[cellX][cellY].AddIntensity(type, intensity);
    }
    public double GetIntensity(int cellX, int cellY, int type)
    {
        return _grid[cellX][cellY].GetIntensity(type);
    }
    public int GetColor(int cellX, int cellY)
    {
        return _grid[cellX][cellY].GetColor();
    }
}