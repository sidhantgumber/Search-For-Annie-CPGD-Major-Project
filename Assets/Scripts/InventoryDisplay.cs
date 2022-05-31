using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum SlotType
{
    Weapon,
    Potion,
    Equipment,
}
public class InventoryDisplay : MonoBehaviour
{
    public Inventory inventory;     //inventory to be displayed

    // public int horizontalSpacing;
    //  public int verticalSpacing;
    //public int numberOfColumns;
    public Transform wheel1Pos;
    public Transform wheel3Pos;
    public Transform wheel5Pos;
    public Transform wheel7Pos;

    bool slot1Empty = true;
    bool slot3Empty = true;
    bool slot5Empty = true;
    bool slot7Empty = true;
    bool slotsFull = false;
    bool canAddWeapon= true;

    Item itemInSlot1 = null;
    Item itemInSlot3 = null;
    Item itemInSlot5 = null;
    Item itemInSlot7 = null;


    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    
    void Start()
    {
        CreateDisplay();
        gameObject.SetActive(false);
    }

    void Update()
    {
       // Debug.Log(inventory.weaponsInSlot);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
     for(int i = 0; i < inventory.slots.Count; i++)
        {
            if(itemsDisplayed.ContainsKey(inventory.slots[i]))
            { 
                
                    itemsDisplayed[inventory.slots[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.slots[i].amount.ToString();
                
            }
            else
            {
                if (inventory.slots[i].item.GetItemType() == ItemType.Weapon && CanAddWeapon())
                {
                    var obj = Instantiate(inventory.slots[i].item.itemDisplayIcon, wheel1Pos.position, Quaternion.identity, transform);
                    obj.GetComponent<RectTransform>().position = GetSlot(i).position;
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.slots[i].amount.ToString();
                    itemsDisplayed.Add(inventory.slots[i], obj);
                }
                else if(inventory.slots[i].item.GetItemType() == ItemType.Potion && CanAddPotion())
                {
                    var obj = Instantiate(inventory.slots[i].item.itemDisplayIcon, wheel1Pos.position, Quaternion.identity, transform);
                    obj.GetComponent<RectTransform>().position = GetSlot(i).position;
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.slots[i].amount.ToString();
                    itemsDisplayed.Add(inventory.slots[i], obj);
                }
                else
                {
                    Debug.Log("Slots Full");
                    break;
                }
            }
        }
    }

    private void CreateDisplay()
    {
      for(int i =0; i<inventory.slots.Count; i++)
        {
            var obj = Instantiate(inventory.slots[i].item.itemDisplayIcon, wheel1Pos.position, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().position = GetSlot(i).position;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.slots[i].amount.ToString();
            itemsDisplayed.Add(inventory.slots[i], obj);
        }
    }
    private Transform GetSlot(int i)
    {
        if(inventory.slots[i].item.GetItemType() == ItemType.Weapon && slot1Empty)
        {

            slot1Empty = false;
            itemInSlot1 = inventory.slots[i].item;
            return wheel1Pos;
        }
      /*  if (inventory.slots[i].item.GetItemType() == ItemType.Weapon && slot3Empty)
        {
            slot3Empty = false;
            return wheel3Pos;
        }
       */
        if (inventory.slots[i].item.GetItemType() == ItemType.Weapon && (slot5Empty||slot1Empty))
        {
            itemInSlot5 = inventory.slots[i].item;
            slot5Empty = false;
            return wheel5Pos;
        }
        if (inventory.slots[i].item.GetItemType() == ItemType.Potion && slot7Empty)
        {
            itemInSlot7 = inventory.slots[i].item;
            slot7Empty = false;
            return wheel7Pos;
        }
      
        else
        {
            return null;
        }
    }
    public void DeleteItemSlot(Item item)
    {
        if(item == itemInSlot1)
        {
            slot1Empty = true;
            
        }
        else if (item == itemInSlot3)
        {
            slot3Empty = true;

        }
        else if (item == itemInSlot5)
        {
            slot5Empty = true;

        }
        else if (item == itemInSlot7)
        {
            slot7Empty = true;

        }
    }
    public bool CanAddWeapon()
    {
        if (slot1Empty || slot5Empty )
        {
            return true;
        }
        else return false;
    }
    public bool CanAddPotion()
    {
        if (slot7Empty) //|| slot3Empty )
        {
            return true;
        }
        else return false;
    }
    /* public Vector3 GetSlotPosition(int i)
     {
         return new Vector3(horizontalSpacing * (i % numberOfColumns), (-verticalSpacing * (i / numberOfColumns)), 0f);
     }
    */
}
