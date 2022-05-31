using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 20;
    [SerializeField] private float healthPoints;
    bool isDead;
    [SerializeField] TakeDamageEvent takeDamage;
    [SerializeField] HealthBar healthBar;
    [SerializeField] AudioSource takeDamageSound;
    void Start()
    {
        healthPoints = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    // had to create this class to add a parameter to a unity event
    [System.Serializable]
    public class TakeDamageEvent : UnityEvent<float>
    {

    }
    public void TakeDamage(float damage)
    {
        takeDamageSound.Play();
        healthPoints -= damage;
        takeDamage.Invoke(damage);
        healthBar.SetHealth(healthPoints);

        if (healthPoints == 0)
        {
            Die();
        }
    }

    public void SetPlayerHealth(float health)
    {
        healthPoints = health;
        healthBar.SetHealth(healthPoints);
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    private void Die()
    {
        isDead = true;
        GetComponent<Animator>().SetTrigger("die");
        if(gameObject.tag != "Player")
        {
            // GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<Animator>().SetTrigger("die");
           
        }
      //  gameObject.SetActive(false);
    }
    public bool IsDead()
    {
        return isDead;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
