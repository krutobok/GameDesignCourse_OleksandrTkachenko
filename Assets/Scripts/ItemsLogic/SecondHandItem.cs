using UnityEngine;

using UnityEngine;

public class SecondHandItem : MonoBehaviour
{
    public SecondHandItemData itemData;

    // Конструктор з параметрами
    public SecondHandItem(ItemInstance itemInstance, int hangerId, int singleHangerId)
    {
        itemData = new SecondHandItemData(itemInstance, hangerId, singleHangerId);

        if (itemData.item != null)
        {
            name = itemData.item.itemName + "_" + itemData.item.uniqueId;
        }
    }

    // Ініціалізація через окремі параметри (старий варіант)
    public void Initialize(ItemInstance itemInstance, int hangerId, int singleHangerId)
    {
        itemData = new SecondHandItemData(itemInstance, hangerId, singleHangerId);

        name = itemData.item.itemName + "_" + itemData.item.uniqueId;
    }

    // Ініціалізація через готовий SecondHandItemData
    public void Initialize(SecondHandItemData data)
    {
        itemData = data;

        if (itemData.item != null)
        {
            name = itemData.item.itemName + "_" + itemData.item.uniqueId;
        }
    }

    private void Start()
    {
        if (itemData == null || itemData.item == null)
        {
            Debug.LogWarning("ItemData або ItemInstance відсутні");
            return;
        }

        // Знаходимо дочірній об’єкт з тегом "item"
        Transform itemChild = FindObjects.FindChildWithTag(transform, "item");
        if (itemChild != null)
        {
            Renderer renderer = itemChild.GetComponent<Renderer>();
            if (renderer != null && itemData.item.color != null)
            {
                renderer.material = itemData.item.color;
            }
            else
            {
                Debug.LogWarning("Renderer або item.color не знайдено");
            }
        }
        else
        {
            Debug.LogWarning("Дочірній об’єкт з тегом 'item' не знайдено");
        }
    }
}
