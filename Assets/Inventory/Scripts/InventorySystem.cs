using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Holistic3D.Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        public List<InventorySlot> slots = new List<InventorySlot>();
        public int maxWeight = 5000;
        public int CurrentWeight { get; private set; } = 0;
        public void SetWeight(int newWeight)
        {
            CurrentWeight = Mathf.Max(0, newWeight);
        }
        private void Awake()
        {
            LoadFromJson();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                SaveToJson();
            }
        }
        public void AddWeight(int weight)
        {
            CurrentWeight += weight;
        }
        public PlayerInventorySystem playerInventorySystem;

        public bool AddItem(ItemInstance item)
        {            
            if (item.weight <= 0) return false;
            if (canAddItem(item.weight))
            {
                slots.Add(new InventorySlot(item));
                AddWeight(item.weight);
                return true;
            }            
            return false;
        }
        public void RemoveItem(ItemInstance item)
        {            
            List<InventorySlot> slotsWithItem = slots.Where(s => s.item.uniqueId == item.uniqueId).ToList();
                
            foreach(var slot in slotsWithItem)
            {           
               slot.ClearSlot();
               slots.Remove(slot);
            }            
        }
        
        public void RemoveItemFromSlot(InventorySlot slot)
        {         
           slot.ClearSlot();
           slots.Remove(slot);              
        }
        public void RemoveItemsFromSlot(int slotNumber)
        {
            slots.RemoveAt(slotNumber);
        } 
        public bool isFull()
        {
            return CurrentWeight >= maxWeight;
        }
        public bool canAddItem(int itemWeight)
        {
            return CurrentWeight + itemWeight <= maxWeight;
        }

        public void ClearInventory()
        {
            slots.Clear();
        }

        public void SaveToJson()
        {
            
            InventoryDataWrapper wrapper = new InventoryDataWrapper();
            wrapper.slots = slots;

            string inventoryData = JsonUtility.ToJson(wrapper, true);
            Debug.Log(inventoryData);

            string filePath = Application.persistentDataPath + "/InventoryData.json";
            System.IO.File.WriteAllText(filePath, inventoryData);
            
        }

        public void LoadFromJson()
        {
            string filePath = Application.persistentDataPath + "/InventoryData.json";
         
            if (System.IO.File.Exists(filePath))
            {
                string inventoryData = System.IO.File.ReadAllText(filePath);
                Debug.Log(inventoryData);

                InventoryDataWrapper wrapper = JsonUtility.FromJson<InventoryDataWrapper>(inventoryData);
                List<InventorySlot> slotsData = wrapper.slots;
                Debug.Log(slotsData);
                foreach (var slot in slotsData)
                {                    
                    AddItem(slot.item);
                }
                
            }
        }

    }
    [System.Serializable]
    public class InventoryDataWrapper
    {
        public List<InventorySlot> slots;
    }
}


