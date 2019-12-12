using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 pos;

    public int gCost;
    public int hCost;

    public int gridX;
    public int gridY;

    public Node parent;
    public Node(bool _walkable, Vector3 _pos, int _gridX, int _gridY){
        walkable = _walkable;
        pos = _pos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost{
        get{
            return gCost + hCost;
        }
    }
}
