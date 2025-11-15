using UnityEngine;
using Holistic3D.Inventory;

public class GameMoneyManager : MonoBehaviour
{
    public static GameMoneyManager Instance;
    [SerializeField] private InventorySystem inventorySystem;
    public int currentMoney { get; private set; } = 5000;
    
    public bool canBuy(int cost)
    {
        return currentMoney >= cost;
    }
    public bool Buy(ItemInstance item)
    {
        
        if (canBuy(item.price))
        {
            
            bool result = inventorySystem.AddItem(item);      
            if (result)
            {
                currentMoney -= item.price;
                item.status = ItemStatus.Bought;
                return true;
            }          
        }
        return false;
    }
    public void Sell(int cost)
    {
        currentMoney += cost;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        LoadFromJson();
        //DontDestroyOnLoad(gameObject);
    }
    private void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/gameData.json";
        if (System.IO.File.Exists(filePath))
        {
            string gameData = System.IO.File.ReadAllText(filePath);
            Debug.Log(gameData);

            GameData wrapper = JsonUtility.FromJson<GameData>(gameData);
            currentMoney = wrapper.money;
           
        }
    }
}
