using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "Game/MapData")]
public class MapData : ScriptableObject
{
    public List<MapNode> nodes;
}
