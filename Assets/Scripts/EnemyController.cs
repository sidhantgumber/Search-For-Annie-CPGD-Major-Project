using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 3;
    [SerializeField] float provokedTime = 3;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float wayPointTolerance = 1f;
    [SerializeField] float waypointDwellTime = 3f;
    [SerializeField] float shoutDistance = 4f;
    [Range(0, 1)]
    [SerializeField] float patrolSpeedFraction = 0.2f;
    GameObject player;
    Health health;
    EnemyMover mover;
    EnemyFighter fighter;



    Vector3 guardPosition;
    float timeSinceLastSawPlayer = Mathf.Infinity;
    int currentWaypointIndex = 0;
    float timeSinceArrivedAtWaypoint = Mathf.Infinity;
    float timeSinceProvoked = Mathf.Infinity;

    // Start is called before the first frame update
    private void Awake()
    {
      
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = GetComponent<Health>();
        fighter = GetComponent<EnemyFighter>();
        mover = GetComponent<EnemyMover>();
        guardPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (health.IsDead()) return;

        if (IsProvoked() && fighter.CanAttack(player))
        {

           AttackBehaviour();

        }

        else if (timeSinceLastSawPlayer < suspicionTime)
        {
            SuspicionBehaviour();

        }
        else
        {
           PatrolBehaviour();
        }
        UpdateTimer();
    }

    public void Provoke()
    {
        timeSinceProvoked = 0;
    }

    private void UpdateTimer()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceArrivedAtWaypoint += Time.deltaTime;
        timeSinceProvoked += Time.deltaTime;
    }

    private void PatrolBehaviour()
    {
        //  mover.SetWalkingSpeed(walkingSpeed);
        Vector3 nextPosition = guardPosition ;
        Debug.Log(gameObject + "is in patrol");
        if (patrolPath != null)
        {
            if (AtWaypoint())
            {
                timeSinceArrivedAtWaypoint = 0;
                CycleWaypoint();

            }
            nextPosition = GetCurrentWaypoint();
        }

        if (timeSinceArrivedAtWaypoint > waypointDwellTime)
        {
            mover.StartMovementAction(nextPosition, patrolSpeedFraction);
        }
       
    }

    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWayPoint(currentWaypointIndex);
    }

    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return (distanceToWaypoint < wayPointTolerance);



    }

    // will make the enemy wait at the last known position of the player after the player loses the enemy

    private void SuspicionBehaviour()
    {
        Debug.Log(gameObject + "is in suspicion");
        GetComponent<ActionScheduler>().CancelCurrentAction();
    }

    private void AttackBehaviour()
    {
        Debug.Log(gameObject + "is in attack");
        timeSinceLastSawPlayer = 0;
        fighter.Attack(player);
      //  Debug.Log("attacking player");
        AlertNearbyEnemies();
    }

    private void AlertNearbyEnemies()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
        foreach (RaycastHit hit in hits)
        {
            EnemyController nearbyEnemyController = hit.collider.GetComponent<EnemyController>();
            if (nearbyEnemyController == null) continue;
            nearbyEnemyController.Provoke();

        }
    }

    private bool IsProvoked()
    {
        if (player == null) return false;

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        return distanceToPlayer < chaseDistance || timeSinceProvoked < provokedTime;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}