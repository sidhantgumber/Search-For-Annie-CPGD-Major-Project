using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New default item", menuName = "Inventory/Items/Default Item")]
public class DefaultItem : Item
{
    public  void Spawn(Transform handTransform, Animator animator)
    {
       
    }

    private void Awake()
    {
        type = ItemType.Default;
    }
}
