using UnityEngine;

public class SaleListing
{
    public ItemInstance item;     // Посилання на сам предмет
    public int listedPrice;       // Ціна, яку обрав гравець
    public bool willBeSold;       // Чи буде продано (true/false)
    public int dayToBeSold;       // День, коли буде продано (якщо буде)
    public int dayToExpire;       // День закриття оголошення

    public SaleListing(ItemInstance item, int listedPrice, int currentDay)
    {
        this.item = item;
        this.listedPrice = listedPrice;
        int daysToSale = CalculateSaleDelay();
        item.status = ItemStatus.Listed;


        if (daysToSale != -1)
        {
            willBeSold = true;
            dayToBeSold = currentDay + daysToSale;
            dayToExpire = -1;
        }
        else
        {
            willBeSold = false;
            dayToBeSold = -1;
            dayToExpire = currentDay + 6;
        }
    }
    int CalculateSaleDelay()
    {
        float priceRatio = listedPrice / item.sellPrice;
        if (priceRatio < 1.0f)
            return 1; 
        else if (priceRatio < 1.1f)
            return 2;
        else if (priceRatio < 1.2f)
            return 3;
        else if (priceRatio < 1.25f)
            return 4;
        else if (priceRatio < 1.3f)
            return 5;
        else
            return -1;
    }
}
