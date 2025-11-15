using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public string itemName;
    public string brand;
    //public Sprite image;
    public int price;
    public int weight;
    public Material color;
    public int condition; // 1 to 10
    public string uniqueId; // наприклад: item_0001234
    public ItemStatus status;
    

    public ItemTemplate template; // посилання на шаблон
    public int sellPrice
    {
        get
        {
            float brandMultiplier = 1f;

            if (brand == "Nike" || brand == "Adidas" || brand == "Puma")
                brandMultiplier = 1.1f;
            else if (brand == "none")
                brandMultiplier = 0.9f;

            float value = template.basePrice * (condition / 10f) * brandMultiplier;
            return Mathf.RoundToInt(value);
        }
    }
    public ItemInstance(ItemTemplate template)
    {
        this.template = template;
        itemName = template.baseName;
        brand = template.possibleBrands[Random.Range(0, template.possibleBrands.Length)];
        //image = template.baseImage;
        price = template.basePrice;
        weight = Random.Range(template.weightMin, template.weightMax);
        color = template.possibleColors[Random.Range(0, template.possibleColors.Length)];
        condition = Random.Range(1, 10); // стан речі
        uniqueId = System.Guid.NewGuid().ToString();
        status = ItemStatus.InShop;
    }
}

