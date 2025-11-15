using System.Collections.Generic;
using UnityEngine;
using Holistic3D.Inventory;

public class SaleManager : MonoBehaviour
{
    public static SaleManager Instance { get; private set; }

    public List<SaleListing> listings = new List<SaleListing>();
    [SerializeField] private InventorySystem InventorySystem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void ListItemForSale(ItemInstance item, int price, int currentDay)
    {
        SaleListing listing = new SaleListing(item, price, currentDay);
        
        listings.Add(listing);
    }

    // Наприклад, метод для перевірки продажу на новий день
    public void ProcessSales(int currentDay)
    {
        foreach (var listing in listings)
        {
            if (listing.willBeSold && listing.dayToBeSold == currentDay)
            {                
                listings.Remove(listing);
                InventorySystem.RemoveItem(listing.item);
                GameMoneyManager.Instance.Sell(listing.listedPrice);
            }else if (!listing.willBeSold && listing.dayToExpire == currentDay)
            {
                listings.Remove(listing);
                listing.item.status = ItemStatus.Bought;
            }
        }
    }
}
