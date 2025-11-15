using UnityEngine;

public class SingleHanger : MonoBehaviour
{
    [Header("Точки розміщення")]
    public Transform point1;
    public Transform point2;

    [Header("Налаштування")]
    public ItemTemplate[] posibleItems;
    public float spacing = 0.01f;
    public int id;


}

