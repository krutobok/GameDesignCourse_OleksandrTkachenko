using System.Collections.Generic;

public class SecondHandItemOrganizer
{
    public Dictionary<int, Dictionary<int, List<SecondHandItemData>>> organizedItems
        = new Dictionary<int, Dictionary<int, List<SecondHandItemData>>>();

    public SecondHandItemOrganizer(List<SecondHandItemData> items)
    {
        BuildStructure(items);
    }

    private void BuildStructure(List<SecondHandItemData> items)
    {
        organizedItems.Clear();

        foreach (var item in items)
        {
            if (!organizedItems.ContainsKey(item.hangerId))
                organizedItems[item.hangerId] = new Dictionary<int, List<SecondHandItemData>>();

            if (!organizedItems[item.hangerId].ContainsKey(item.singleHangerId))
                organizedItems[item.hangerId][item.singleHangerId] = new List<SecondHandItemData>();

            organizedItems[item.hangerId][item.singleHangerId].Add(item);
        }
    }

    public List<SecondHandItemData> GetItems(int hangerId, int singleHangerId)
    {
        if (organizedItems.ContainsKey(hangerId) && organizedItems[hangerId].ContainsKey(singleHangerId))
            return organizedItems[hangerId][singleHangerId];

        return new List<SecondHandItemData>();
    }
}

