using UnityEngine;

[System.Serializable]
public class SecondHandItemData
{
    public ItemInstance item;
    public int hangerId;
    public int singleHangerId;

    public SecondHandItemData(ItemInstance item, int hangerId, int singleHangerId)
    {
        this.item = item;
        this.hangerId = hangerId;
        this.singleHangerId = singleHangerId;
    }
}
