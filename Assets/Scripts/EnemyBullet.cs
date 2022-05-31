using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 50f;
    public float bulletDamage = 5f;
    private Rigidbody rb;
    GameObject player; 
   
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        GetComponent<AudioSource>().Play();
        transform.LookAt(player.transform);
        rb.velocity = transform.forward * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            player.GetComponent<Health>().TakeDamage(bulletDamage);
        }
        Destroy(gameObject);  
    }
}
