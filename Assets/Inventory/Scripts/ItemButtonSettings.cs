using UnityEngine;
using UnityEngine.UI;


namespace Holistic3D.Inventory
{
    public class ItemButtonSettings : MonoBehaviour
    {
        [SerializeField] private Image spriteImage;
        [SerializeField] private TMPro.TextMeshProUGUI numberInSlot;
        [SerializeField] private ItemType itemType;

        public void Init(ItemInstance item)
        {
            spriteImage.sprite = InventoryIconGenerator.GenerateIcon(item);
            numberInSlot.text = item.weight + "g";
            itemType = item.template.itemType;
        }
        public void UpdateQuantityDisplay(int quantity)
        {
            numberInSlot.text = quantity + "";
        }
    }
}