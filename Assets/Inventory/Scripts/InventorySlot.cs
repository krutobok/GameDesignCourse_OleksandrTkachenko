using UnityEngine;

namespace Holistic3D.Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        public ItemInstance item;      
        private int weight;
        private ItemButtonSettings itemButton;


        public int Weight
        {
            get => weight;
            set
            {
                weight = value < 0 ? 1 : value;
                if (itemButton != null)
                {
                    itemButton.UpdateQuantityDisplay(weight);

                }
            }
        }

        public InventorySlot(ItemInstance i)
        {
            item = i;
            itemButton = InventoryPanelManager.Instance.CreateInventoryButton(i, this);
            Weight = i.weight;     
        }
        public bool isEmpty()
        {
            return item == null || weight == 0;
        }
        public void ClearSlot()
        {
            item = null;
            Weight = 0;
            if(itemButton != null)
            {
                InventoryPanelManager.Instance.DestroyInventoryButton(itemButton.gameObject);               
            }
        }
        public void SetItem(ItemInstance newItem, int newWeight)
        {
            item = newItem;
            Weight = newWeight;
        }
    }
}

