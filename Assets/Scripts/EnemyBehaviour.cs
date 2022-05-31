using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private float minHealthThreshHold;
    [SerializeField] private float healRate;
    [SerializeField] private float chaseRange = 4f;
    [SerializeField] private GameObject player;
    [SerializeField] Transform handTransform;
    [SerializeField] Weapon enemyWeapon;
    [SerializeField] Transform bulletSpawnPos;
    [SerializeField] private Cover[] availableCovers;
    private float attackRange = 4f;
    Transform playerTransform;
    private Transform bestCoverSpot;
    private Node topNode;
    bool isDead = false;

    NavMeshAgent agent;
    // GameObject player;
    private float _currentHealth;
    public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }
    
    Animator animator;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
        animator = GetComponent<Animator>(); 
        enemyWeapon.Spawn(handTransform, animator);
    }
    void Start()
    {
        attackRange = enemyWeapon.GetWeaponRange();
        _currentHealth = startingHealth;
        ConstructBehaviourTree();
       
    }

    // refer to diagram to see the arrangement of nodes
    // nodes ko bottom to top initialize karenge as higher nodes need to be initialized with lower nodes
    private void ConstructBehaviourTree()
    {
    /* //   IsCoverAvailableNode coverAvailableNode = new IsCoverAvailableNode(availableCovers, playerTransform, this);
        GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
        HealthNode healthNode = new HealthNode(this, minHealthThreshHold);
      //  IsInCoverNode isInCoverNode = new IsInCoverNode(playerTransform, transform);
      //  ChaseNode chaseNode = new ChaseNode(playerTransform, agent);
        RangeNode chasingRangeNode = new RangeNode(chaseRange, playerTransform, transform);
        RangeNode attackingRangeNode = new RangeNode(attackRange, playerTransform, transform);
       // AttackNode attackNode = new AttackNode(agent, this);

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode });
        Sequence attackSequence = new Sequence(new List<Node> { attackingRangeNode, chaseNode });
       
        Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvailableNode, goToCoverNode });
        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, chaseSequence });  // using already existing sequences as nodes
        Selector tryToTakeCoverSelector = new Selector(new List<Node> { isInCoverNode, findCoverSelector });
        Sequence mainCoverSequence = new Sequence(new List<Node> { healthNode, tryToTakeCoverSelector });
        topNode = new Selector(new List<Node> { mainCoverSequence, attackSequence, chaseSequence });

        */



    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentHealth);
        topNode.Evaluate();
      //  if(topNode.nodeState == NodeState.FAILIURE)
        {
          //  Debug.Log(gameObject + "Can't do anything");
        }
        UpdateAnimator();
        //currentHealth += Time.deltaTime * healRate;

    }
    private void UpdateAnimator()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        //  Debug.Log(navMeshAgent.velocity);
        animator.SetFloat("forwardSpeed", speed);
    }
    // 25:45 
    /* public float GetCurrentHealth()
     {
         return currentHealth;
     }
    */
    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject + "has health" + currentHealth);
        currentHealth -= damage;

        if (currentHealth == 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        GetComponent<Animator>().SetTrigger("die");
        if (gameObject.tag != "Player")
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
    public void Attack()
    {
        Debug.Log("isAttacking");
      if(!enemyWeapon.isRanged)
        {
            MeleeAttack();
        }
        else
        {
            Shoot();
        }
    }

    private void MeleeAttack()
    {
        animator.ResetTrigger("stopAttack");
        animator.SetTrigger("attack");
    }
    private void Shoot()
    {
        //Instantiate(enemyWeapon.GetWeaponProjectile(), bulletSpawnPos.position, Quaternion.identity);
        StartCoroutine(StartShooting());
    }

    private IEnumerator StartShooting()
    {
        Instantiate(enemyWeapon.GetWeaponProjectile(), bulletSpawnPos.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
    }
    void Hit()    // animationEvent
    {
        player.GetComponent<Health>().TakeDamage(enemyWeapon.GetWeaponDamage());
    }
    public void SetBestCoverSpot(Transform bestCoverSpot)
    {
        this.bestCoverSpot = bestCoverSpot;
    }
    public Transform GetBestCoverSpot()
    {
        return bestCoverSpot;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

}
