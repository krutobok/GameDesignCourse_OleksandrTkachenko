using UnityEngine;
using Holistic3D.Inventory;

[CreateAssetMenu(fileName = "ItemTemplate", menuName = "Scriptable Objects/ItemTemplate")]
public class ItemTemplate : ScriptableObject
{
    public int id;
    public string baseName;
    public Sprite baseImage;
    public string[] possibleBrands;
    public int basePrice;   
    public int weightMin;
    public int weightMax;
    public ItemType itemType;
    public Material[] possibleColors;
    public GameObject itemPrefab;
}
