using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody rb;
    Fighter player; 
    public float bulletSpeed = 100f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
    }

    private void Start()
    {
       
        rb.velocity = transform.forward * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponent<EnemyAI>().TakeDamage(player.currentWeapon.GetWeaponDamage());
        }
        Destroy(gameObject);  
    }
}
