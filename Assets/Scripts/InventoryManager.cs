using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{
    public class InventoryManager : MonoBehaviour
    {
        public Inventory inventory;
        public Item itemInSlot;
        Fighter player;
        InventoryDisplay inventoryDisplay;
        public GameObject itemPickup;
         float pickUpSpawnOffset = 1f;
         float pickUpSpawnHeight = 0.2f;
        //private ItemType typeInSlot;
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
            inventoryDisplay = GameObject.FindGameObjectWithTag("WeaponWheel").GetComponent<InventoryDisplay>();
        }
        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void EquipItem()
        {
            if (inventory.GetItemCount(itemInSlot) >= 1)
            {

                if (itemInSlot.type == ItemType.Weapon)
                {

                    Weapon weapon = (Weapon)itemInSlot;
                    if (player.currentWeapon != weapon)
                    {
                        player.EquipWeapon(weapon);
                    }

                }

                if(itemInSlot.type == ItemType.Potion)
                {
                    player.gameObject.GetComponent<Health>().SetPlayerHealth(player.GetComponent<Health>().GetMaxHealth());
                    inventory.RemoveItem(itemInSlot, 1);
                    
                }
            }

        }
        public void SpawnPickup()
        {
            Vector3 pickUpSpawnPos = player.GetComponent<Transform>().position;
            pickUpSpawnPos.x += pickUpSpawnOffset;
            pickUpSpawnPos.y += pickUpSpawnHeight;
            pickUpSpawnPos.z += pickUpSpawnOffset;
            Instantiate(itemPickup, pickUpSpawnPos, Quaternion.identity);
        }
        public void DropItem()
        {
            if(inventory.GetItemCount(itemInSlot)>=1)
            {
                inventory.RemoveItem(itemInSlot, 1);
                player.RemoveWeapon();
                SpawnPickup();
                player.EquipWeapon(player.GetDefaultWeapoon());

            }
            else if(inventory.GetItemCount(itemInSlot) == 0)
            {
                
                inventoryDisplay.DeleteItemSlot(itemInSlot);
                player.RemoveWeapon();
                inventory.DeleteItemSlot(itemInSlot); 
               // SpawnPickup();
                Destroy(gameObject);
              
                
            }
        }
    }
}
