using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{
    public int numberOfSlots = 3;
    public int weaponSlots = 2;
    public int potionSlots = 1;
    
    public List<InventorySlot> slots = new List<InventorySlot>();
   // bool inventoryFull = false;
    public void AddItem(Item item, int amount)
    {
        bool hasItem = false;
       
      //  if(!hasItem)
        {
            
        }
    }
   
    public void CreateNewSlot(Item item, int amount)
    {
        if (AreSlotsLeft() && CanAddWeapon())
        {
            if (item.GetItemType() == ItemType.Weapon)
                slots.Add(new InventorySlot(item, amount));    // yahaan constructor kaam aaya item aur amount add karaane ke liye
        }
        if (AreSlotsLeft() && CanAddPotion())
        {
            if (item.GetItemType() == ItemType.Potion)
                slots.Add(new InventorySlot(item, amount));    // yahaan constructor kaam aaya item aur amount add karaane ke liye
        }
    }
    public void AddItemCount(Item item, int amount)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == item)
            {
                slots[i].AddAmount(amount);
              //  hasItem = true;
                break;
            }
        }
    }

    public void RemoveItem(Item item, int amount)
    {
       // bool hasItem = true;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == item)
            {
                slots[i].SubtractAmount(amount);
               // hasItem = true;
                break;
            }
        }
    }
    public bool AreSlotsLeft()
    {
        return slots.Count < numberOfSlots;
    }

    public bool IsInInventory(Item item)
    {
        foreach(InventorySlot slot in slots)
        {
            if(slot.item == item)
            {
                return true;
            }
        }
         return false;
    }
    public InventorySlot GetItemSlot(Item item)
    {
        foreach(InventorySlot slot in slots)
        {
            if(item == slot.item)
            {
                return slot;
            }
        }
       return null;
    }
    public bool CanAddWeapon()
    {
        int weaponsInSlot = 0;
        foreach (InventorySlot slot in slots)
        {
            if(slot.item.GetItemType() == ItemType.Weapon)
            {
                weaponsInSlot++;
                Debug.Log(weaponsInSlot);
            }
        }
        if (weaponsInSlot < weaponSlots)
        {
            return true;
        }
        else return false;
    }

    public int GetItemCount(Item item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item)
            {
                return slot.amount;
            }
        }
         return 0;
    }

    public bool CanAddPotion()
    {
        int potionsInSlot = 0;
        foreach (InventorySlot slot in slots)
        {
            if (slot.item.GetItemType() == ItemType.Potion)
            {
                potionsInSlot++;
               
            }
        }
        if (potionsInSlot < potionSlots)
        {
            return true;
        }
        else return false;
    }

    public void DeleteItemSlot(Item item)
    {
       foreach(InventorySlot slot in slots)
        {
            if(slot.item == item )
            {
               // weaponsInSlot--;
                RemoveItem(item, 1);
                slots.Remove(slot);
                break;     // USED BREAK STATEMENT TO REMOVE COLLECTION WAS MODIFIED, ENUMERATION ERROR. List par iteration chal rahi thi to during iteration item remove nahi kar sakte hence we have to break the loop after removing item
            }
         
        }
    }
}

/*
public enum SlotType
{
    Weapon,
    Potion,
    Equipment,
    Default
} */

[System.Serializable]
public class InventorySlot
{
   // public SlotType slotType;
    public Item item;
    public int amount;
    /*
     * Using a parametrized constructor to initialize the values
     */
    public InventorySlot(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
      //  this.slotType = slotType;
    }
    public void AddAmount(int value)
    {
       // Debug.Log("Added " + item.GetItemType());
        amount += value;
    }
    public void SubtractAmount(int value)
    {
        if (amount >= 1)
        {
            amount -= value;
        }
    }


}
