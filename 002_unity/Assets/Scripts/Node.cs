using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    int number;
    Vector3Int position;

    int north;
    int east;
    int south;
    int west;

    public Node(int number, Vector3Int position, int north, int east, int south, int west)
    {
        this.number = number;
        this.position = position;
        this.north = north;
        this.east = east;
        this.south = south;
        this.west = west;
    }

    public int Nr
    {
        get { return number; }
    }

    public Vector3Int Position
    {
        get { return position; }
    }

    public int NorthNeighbour
    {
        get { return north; }
        set { north = value; }
    }
    public int EastNeighbour
    {
        get { return east; }
        set { east = value; }
    }
    public int SouthNeighbour
    {
        get { return south; }
        set { south = value; }
    }
    public int WestNeighbour
    {
        get { return west; }
        set { west = value; }
    }
}
