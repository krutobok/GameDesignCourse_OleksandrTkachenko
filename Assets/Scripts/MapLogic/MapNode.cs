using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MapNode
{
    public string locationName;
    public string sceneName;
    //public Vector2 position; // для відображення на карті
    public List<MapEdge> connections = new List<MapEdge>();
}

[System.Serializable]
public class MapEdge
{
    public string targetLocation;
    public int walkTime;
    public int busTime;
    public int carTime;
}

