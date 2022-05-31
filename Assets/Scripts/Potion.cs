using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Items/Potion")]
public class Potion : Item
{
    public int healthRestored;

    public  void Spawn(Transform handTransform, Animator animator)
    {
       
    }

    private void Awake()
    {
        type = ItemType.Potion;
    }
}
