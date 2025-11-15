using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Holistic3D.Inventory
{
    public class InventoryPanelManager : MonoBehaviour
    {
        [SerializeField] private GameObject itemButtonPrefab;
        [SerializeField] private GameObject itemContainer;
        [SerializeField] private Button ListForSaleButton;
        [SerializeField] private TMPro.TMP_InputField SalePriceField;
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private List<ItemTemplate> allTemplatesItems;
        [SerializeField] private ItemRotator itemRotator;

        private Dictionary<GameObject, ItemType> inventoryItemMap = new Dictionary<GameObject, ItemType>();
        private Dictionary<int, GameObject> previewItemObjects;
        public static InventoryPanelManager Instance { get; private set; }

        private ItemType activeItemTypeTab;
        public bool IsVisible
        {
            get {
                CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
                if (canvasGroup.alpha == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            previewItemObjects = new Dictionary<int, GameObject>();
            foreach (var template in allTemplatesItems)
            {
                GameObject obj = GameObject.Find("Preview_" + template.id);
                if (obj != null)
                {
                    previewItemObjects[template.id] = obj;
                    obj.SetActive(false);
                }
            }
        }

        public void SetPanelVisibility(bool isVisible)
        {
            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
            canvasGroup.alpha = isVisible ? 1 : 0;
            canvasGroup.blocksRaycasts = isVisible;
            canvasGroup.interactable = isVisible;
        }
        public void TogglePanelVisibility()
        {
            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
            if (canvasGroup.alpha == 1)
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            }
            else
            {
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
            }

        }

        public ItemButtonSettings CreateInventoryButton(ItemInstance item, InventorySlot slot)
        {
            GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
            ItemButtonSettings itemButtonSettings = itemButton.GetComponent<ItemButtonSettings>();
            itemButtonSettings.Init(item);
            inventoryItemMap.Add(itemButton, item.template.itemType);
            itemButton.GetComponent<Button>().onClick.AddListener(() => {
                ShowItem(item.template.id, slot);
                UpdateUIListForSale(item.status == ItemStatus.Bought);
            });

            if (item.template.itemType != activeItemTypeTab)
            {
                itemButton.gameObject.SetActive(false);
            }
            return itemButtonSettings;
        }
        public void DestroyInventoryButton(GameObject invButton)
        {
            inventoryItemMap.Remove(invButton);
            Destroy(invButton);

        }
        public void FilterItemsByType(ItemType type)
        {
            activeItemTypeTab = type;
            foreach (var kvp in inventoryItemMap)
            {
                GameObject itemButton = kvp.Key;
                ItemType itemType = kvp.Value;
                itemButton.gameObject.SetActive(itemType == type);
            }
        }
        public void ShowItem(int itemId, InventorySlot slot)
        {
            ListForSaleButton.onClick.RemoveAllListeners();
            ListForSaleButton.onClick.AddListener(() => {
                SaleManager.Instance.ListItemForSale(slot.item, int.Parse(SalePriceField.text), GameTimeManager.Instance.day);
                UpdateUIListForSale(false);
            });
            foreach (var obj in previewItemObjects.Values)
            {
                obj.SetActive(false);
            }
            if (previewItemObjects.TryGetValue(itemId, out GameObject itemToShow))
            {
                itemRotator.SetTarget(itemToShow.transform);
                itemToShow.SetActive(true);
                Renderer renderer = itemToShow.GetComponent<Renderer>();
                if (renderer != null && slot.item.color != null)
                {
                    renderer.material = slot.item.color;
                }
                else
                {
                    Debug.LogWarning("Renderer або item.color не знайдено");
                }
            }
        }
        public void UpdateUIListForSale(bool canListForSale)
        {
            ListForSaleButton.gameObject.SetActive(canListForSale);
            SalePriceField.gameObject.SetActive(canListForSale);
        }
        public void saveInventory(){
            inventorySystem.SaveToJson();
        }

    }
}

