using UnityEngine;

namespace Holistic3D.Inventory
{
    public class ItemPickup : MonoBehaviour
    {
        public ItemInstance item;       

        private void Start()
        {
            gameObject.GetComponent<Collider>().isTrigger = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerInventorySystem playerInventory = other.GetComponent<PlayerInventorySystem>();
                if(playerInventory != null)
                {
                    bool result = playerInventory.PickUpItem(item);
                    if (result)
                        Destroy(gameObject);
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            ItemPickup other = collision.gameObject.GetComponent<ItemPickup>();
            if (other != null) return;
            //if (other != null && itemData.itemId == collision.gameObject.GetComponent<ItemPickup>().itemData.itemId) return;
            gameObject.GetComponent<Collider>().isTrigger = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}

