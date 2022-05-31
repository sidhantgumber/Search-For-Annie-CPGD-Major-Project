using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class PlayerInteraction : MonoBehaviour
    {
        public Inventory inventory; 
        private StarterAssetsInputs input;
     //   public InventoryDisplay inventoryDisplay;
        private bool isInrange;
      //  public GameObject inventoryCanvas;
        // Start is called before the first frame update
        void Start()
        {
           input = GetComponent<StarterAssetsInputs>();
          
        }

        // Update is called once per frame
        void Update()
        {
          
            if (input.interact_PickUp)
            {
                Debug.Log("Is interacting");
            }
         

        }
        private void OnTriggerEnter(Collider other)
        {
            var item = other.GetComponent<ItemPickup>();
            if(item)
            {
                if (inventory.IsInInventory(item.item))
                {
                    inventory.AddItemCount(item.item, 1);
                    Destroy(other.gameObject);
                }
                else
                {

                    if (item.item.GetItemType() == ItemType.Weapon && inventory.CanAddWeapon())
                    {
                        /*if (inventory.IsInInventory(item.item))
                        {
                            inventory.AddItemCount(item.item, 1);
                            Destroy(other.gameObject);
                        }
                        */
                        // else
                        //   {
                        inventory.CreateNewSlot(item.item, 1);
                        Destroy(other.gameObject);
                        //   }
                    }
                    if (item.item.GetItemType() == ItemType.Potion && inventory.AreSlotsLeft())
                    {
                        /*  if (inventory.IsInInventory(item.item))
                          {
                              inventory.AddItemCount(item.item, 1);
                              Destroy(other.gameObject);
                          }
                        */
                        // else { 
                        inventory.CreateNewSlot(item.item, 1);
                        Destroy(other.gameObject);
                        // }
                    }
                }


            }
        }

        private void OnApplicationQuit()
        {
            inventory.slots.Clear();
        }


    }
}
