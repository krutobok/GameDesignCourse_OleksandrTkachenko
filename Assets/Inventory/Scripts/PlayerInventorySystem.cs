using UnityEngine;
using UnityEngine.InputSystem;

namespace Holistic3D.Inventory
{
    public class PlayerInventorySystem : MonoBehaviour
    {
        public InventorySystem inventorySystem;
        public InputAction dropAction;
        public InputAction inventoryAction;        
        [SerializeField] InventoryPanelManager inventoryPanelManager;

        private void Start()
        {
            dropAction = InputSystem.actions.FindAction("Drop");
            inventoryAction = InputSystem.actions.FindAction("Inventory");
            inventoryPanelManager.SetPanelVisibility(false);
            inventorySystem.playerInventorySystem = this;
        }

        private void Update()
        {
            if (dropAction.WasPressedThisFrame())
            {
                Debug.Log("Drop");
                //DropItem(itemToDrop, 6);
            }else if (inventoryAction.WasPressedThisFrame())
            {
                OpenCloseInventory();
            }
        }

        public void OpenCloseInventory()
        {
            inventoryPanelManager.TogglePanelVisibility();
            if (inventoryPanelManager.IsVisible)
            {
                GameTimeManager.Instance.IsGameTimeRunning = false;          
            }
            else
            {
                GameTimeManager.Instance.IsGameTimeRunning = true;                
            }
        }

        public bool PickUpItem(ItemInstance item)
        {
            if (!inventorySystem.canAddItem(item.weight))
            {
                return inventorySystem.AddItem(item);
            }
            return false;
        }

        /* public void DropItemsFromSlot(int slotNumber)
        {
            inventorySystem.RemoveItemsFromSlot(slotNumber);
        }

        public void DropItem(ItemData itemData, int quantity) {
            int couldntBeDropped = inventorySystem.RemoveItem(itemData, quantity);
            int numberDropped = quantity - couldntBeDropped;
            if (numberDropped > 0)
            {
                if (itemData.groupedPrefab)
                {
                    Instantiate(itemData.prefab, GetDropPosition(), Quaternion.identity).GetComponent<ItemPickup>().quantity = numberDropped;
                }
                else
                {
                    Vector3 location = GetDropPosition();
                    for (int i = 0; i < numberDropped; i++)
                    {
                        Instantiate(itemData.prefab, GetDropPosition(), Quaternion.identity);
                    }
                }
            }
        } */

        public Vector3 GetDropPosition()
        {
            Vector3 playerPosition = transform.position;
            Vector3 forwaedDirection = transform.forward;
            Vector3 upDirection = transform.up;
            float dropDistance = 2f;
            return playerPosition + forwaedDirection * dropDistance + upDirection * dropDistance;
        }
    }
}

