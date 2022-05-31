using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Weapon,
    Potion,
    Equipment,
    Default
}
public class Item : ScriptableObject
{
    public GameObject itemDisplayIcon;
    public ItemType type;
    [TextArea(15,20)]
    public string description;

    public ItemType GetItemType()
    {
        return type;
    }
  //  public abstract void Spawn(Transform handTransform, Animator animator);
}
