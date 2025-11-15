using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SecondHandSpawner : MonoBehaviour
{
    public static SecondHandSpawner Instance;
    public string secondHandName = "SecondHand1";
    public int deliveryDay = 1;
    public int unclockDay = 1;

    public Hanger[] hangers;
    public List<SecondHandItem> allItems;
    public int Delivery
    {
        get => Mathf.FloorToInt((GameTimeManager.Instance.day - deliveryDay) / 7f) + 1;
    }
    private void Start()
    {
        LoadFromJson();
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveToJson();
        }
        if (GameTimeManager.Instance.isTimeToItemDisappear())
        {
            RemoveRandomExpensiveItem();
        }
    }

    private void RemoveRandomExpensiveItem()
    {
        // 1. Копія і сортування
        var sortedCopy = allItems
            .Where(elem => elem.itemData.item.uniqueId != BuyPanelManager.Instance.currentItemId)
            .OrderByDescending(elem => elem.itemData.item.sellPrice)
            .ToList();

        // 2. Перші 3 найдорожчі
        var selected = sortedCopy.Take(3).ToList();

        // 3. Додаємо 2 випадкових з решти
        var remaining = sortedCopy.Skip(3).ToList();
        if (remaining.Count > 0)
        {
            var randomTwo = remaining
                .OrderBy(x => UnityEngine.Random.value)
                .Take(Mathf.Min(2, remaining.Count))
                .ToList();
            selected.AddRange(randomTwo);
        }

        // 4. Вибір 1 випадкового з п'ятірки
        var itemToRemove = selected[UnityEngine.Random.Range(0, selected.Count)];

        // 5. Видаляємо з оригінального списку
        allItems.Remove(itemToRemove);

        // 6. Знищення в ігровому світі
        if (itemToRemove != null && itemToRemove.gameObject != null)
        {            
            Destroy(itemToRemove.gameObject);
            Destroy(itemToRemove);
        }
    }

    private void GenerateItems(SingleHanger hanger, int hangerId)
    {
        if (hanger.point1 == null || hanger.point2 == null || hanger.posibleItems.Length == 0) return;

        Vector3 direction = (hanger.point2.position - hanger.point1.position).normalized;
        float totalLength = Vector3.Distance(hanger.point1.position, hanger.point2.position);
        float currentDistance = 0f;

        while (true)
        {
            ItemTemplate itemTemplate = hanger.posibleItems[UnityEngine.Random.Range(0, hanger.posibleItems.Length)];
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
            itemSecondHand.Initialize(instance, hangerId, hanger.id);
            allItems.Add(itemSecondHand);

            currentDistance += width + hanger.spacing;
        }
    }
    private void GenerateAllItems()
    {
        foreach (Hanger hanger in hangers)
        {
            foreach (SingleHanger single in hanger.singleHangers)
            {
                GenerateItems(single, hanger.id);
            }
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

    public void SaveToJson()
    {
        SecondHandDataWrapper wrapper = new SecondHandDataWrapper();
        allItems.RemoveAll(i => i == null);
        wrapper.items = allItems
    .Select(item => item.itemData)
    .ToList();
        wrapper.delivery = Delivery;
        wrapper.day = GameTimeManager.Instance.day;
        wrapper.time = GameTimeManager.Instance.gameTime;

        string secondHandData = JsonUtility.ToJson(wrapper, true);
        Debug.Log(secondHandData);

        string filePath = Application.persistentDataPath + "/" + secondHandName + ".json";
        System.IO.File.WriteAllText(filePath, secondHandData);

    }

    public void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/" + secondHandName + ".json";
        if (System.IO.File.Exists(filePath))
        {
            string secondHandData = System.IO.File.ReadAllText(filePath);
            Debug.Log(secondHandData);

            SecondHandDataWrapper wrapper = JsonUtility.FromJson<SecondHandDataWrapper>(secondHandData);
            if (wrapper.delivery == Delivery)
            {
                List<SecondHandItemData> itemsData = wrapper.items;

                var organizer = new SecondHandItemOrganizer(itemsData);
                foreach (Hanger hanger in hangers)
                {
                    foreach (SingleHanger singleHanger in hanger.singleHangers)
                    {
                        var items = organizer.GetItems(hanger.id, singleHanger.id);
                        GenerateItemsFromList(singleHanger, items);
                    }
                }

                destroySeveralItems(wrapper.day, wrapper.time);
            }
            else
            {
                GenerateAllItems();
                int day = Delivery*7+deliveryDay;
                destroySeveralItems(day, GameTimeManager.Instance.hourseDuration);
            }

        }
        else
        {
            GenerateAllItems();
        }
    }
    private void GenerateItemsFromList(SingleHanger hanger, List<SecondHandItemData> items)
    {
        if (hanger.point1 == null || hanger.point2 == null || hanger.posibleItems.Length == 0) return;

        Vector3 direction = (hanger.point2.position - hanger.point1.position).normalized;
        float totalLength = Vector3.Distance(hanger.point1.position, hanger.point2.position);
        float currentDistance = 0f;

        for (int i = 0; i < items.Count; i++)
        {
            ItemTemplate itemTemplate = hanger.posibleItems[UnityEngine.Random.Range(0, hanger.posibleItems.Length)];
            GameObject prefab = itemTemplate.itemPrefab;
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
            itemSecondHand.Initialize(items[i]);
            allItems.Add(itemSecondHand);
            currentDistance += width + hanger.spacing;
        }
    }
    public void destroySeveralItems(int day, float time)
    {
        int counter = countElapsedTime(day, time);
        for (int i = 0; i < counter; i++)
        {
            RemoveRandomExpensiveItem();
        }
    }
    public int countElapsedTime(int day, float time)
    { 
        int counter = 0;

        int currentDay = GameTimeManager.Instance.day;
        float currentTime = GameTimeManager.Instance.gameTime; // теж у секундах від початку дня
        float hourDuration = GameTimeManager.Instance.hourseDuration;

        int hoursPerDay = 15;

        Debug.Log("Day: " + day + "Time: " + time );
        int startHour = (int)Math.Floor(time / hourDuration);
        int endHour = (int)Math.Floor(currentTime / hourDuration);
        Debug.Log("startHour: " + startHour + "endHour: " + endHour);

       
        int totalHours = (currentDay - day) * hoursPerDay + (endHour - startHour);
        Debug.Log("totalHours" + totalHours);
       
        for (int i = 0; i < totalHours; i++)
        {
            int absoluteHour = (startHour + i) % hoursPerDay;

            if (absoluteHour != 0 && absoluteHour < 12) // лише 1–11 враховуються
            {
                counter++;
            }
        }
        Debug.Log("Second counter" + counter);

        return counter;
    }
}

[System.Serializable]
public class SecondHandDataWrapper
{
    public List<SecondHandItemData> items;
    public int delivery;
    public int day;
    public float time;
}