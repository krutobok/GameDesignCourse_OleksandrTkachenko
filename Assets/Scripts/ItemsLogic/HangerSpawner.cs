using UnityEngine;


public class HangerSpawner : MonoBehaviour
{
    public Hanger[] hangers;

    public static HangerSpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        foreach (Hanger hanger in hangers)
        {
            foreach (SingleHanger single in hanger.singleHangers)
            {
                GenerateItems(single);
            }
        }
    }

    private void GenerateItems(SingleHanger hanger)
    {
        if (hanger.point1 == null || hanger.point2 == null || hanger.posibleItems.Length == 0) return;

        Vector3 direction = (hanger.point2.position - hanger.point1.position).normalized;
        float totalLength = Vector3.Distance(hanger.point1.position, hanger.point2.position);
        float currentDistance = 0f;

        while (true)
        {
            ItemTemplate itemTemplate = hanger.posibleItems[Random.Range(0, hanger.posibleItems.Length)];
            GameObject prefab = itemTemplate.itemPrefab;
            // Створення тимчасового об’єкта, щоб дізнатись реальні розміри
            GameObject temp = Instantiate(prefab);
            Bounds bounds = GetBoundsRecursive(temp.transform);
            float width = bounds.size.x;
            float height = bounds.size.y;
            Destroy(temp);

            if (currentDistance + width > totalLength)
                break;

            Vector3 basePosition = hanger.point1.position + direction * currentDistance;
            Vector3 spawnPos = basePosition - new Vector3(0f, height / 2f, 0f);

            Quaternion rot = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(0f, 90f, 0f);

            GameObject finalPrefab = Instantiate(prefab, spawnPos, rot, hanger.transform);
            SecondHandItem itemSecondHand = finalPrefab.AddComponent<SecondHandItem>();          

            // Створюємо новий екземпляр речі та ініціалізуємо
            ItemInstance instance = new ItemInstance(itemTemplate);
            //itemSecondHand.Initialize(instance);
            

            currentDistance += width + hanger.spacing;
        }
    }

    private Bounds GetBoundsRecursive(Transform obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(obj.position, Vector3.zero);

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }
        return bounds;
    }
}

