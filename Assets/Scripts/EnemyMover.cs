using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMover : MonoBehaviour, IAction
{
    [SerializeField] float maxSpeed;

    NavMeshAgent navMeshAgent;
    Health health;
    Animator animator;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
      
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
       // navMeshAgent.enabled = !health.IsDead();   // done to stop interruption from enemy after death
        UpdateAnimator();
    
    }

    // making a separate startmovement action method to add additional functionalities when we want the player to move
    // seedha moveto mei daalne se fadde aa rahe the
    public void StartMovementAction(Vector3 destination, float speedFraction)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        MoveTo(destination, speedFraction);
    }

    public void MoveTo(Vector3 destination, float speedFraction)
    {
        navMeshAgent.destination = destination;
        navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
        navMeshAgent.isStopped = false;  // stop mei isse true kiya tha to firse chalaane ke liye false karna hi padhega
    }


    public void Cancel()
    {
        navMeshAgent.isStopped = true;
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
      //  Debug.Log(navMeshAgent.velocity);
        animator.SetFloat("forwardSpeed", speed);
    }
}
