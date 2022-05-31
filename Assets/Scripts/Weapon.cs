using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Items/Weapon")]
public class Weapon : Item
{
    [SerializeField] float weaponDamage = 5;
    [SerializeField] float weaponRange = 2;
    [SerializeField] AnimatorOverrideController weaponOverrideController;
    [SerializeField] GameObject equippedWeaponPrefab = null;
    [SerializeField] public bool isRanged = false;
    [SerializeField] GameObject projectile = null;
    

    const string weaponName = "Weapon";
    private void Awake()
    {
        type = ItemType.Weapon;
        
    }
    public  void Spawn(Transform handTransform, Animator animator)
    {
        DestroyOldWeapon(handTransform);
        if (equippedWeaponPrefab != null)
        { 
            GameObject weapon =  Instantiate(equippedWeaponPrefab, handTransform);
            weapon.name = weaponName;
        }

        if(weaponOverrideController!=null)
        {
           animator.runtimeAnimatorController = weaponOverrideController;
        }
   
    }

    private void DestroyOldWeapon( Transform handTransform)
    {
        Transform oldWeapon = handTransform.Find(weaponName);
        if (oldWeapon == null) return;

        oldWeapon.name = "Gone";
        Destroy(oldWeapon.gameObject);
        
    }

    public GameObject GetWeaponProjectile()
    {
        return projectile;
    }

    public float GetWeaponRange()
    {
        return weaponRange;
    }
    public float GetWeaponDamage()
    {
        return weaponDamage;
    }

   
}
