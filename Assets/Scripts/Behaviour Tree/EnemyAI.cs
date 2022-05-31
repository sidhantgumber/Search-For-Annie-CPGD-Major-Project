using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private float lowHealthThreshold;
    [SerializeField] private float healthRestoreRate;
    [SerializeField] private float alertDistance = 3f;
    [SerializeField] private float chasingRange;
   // [SerializeField] private float provokedTime = 2f;
    private float shootingRange;


    [SerializeField] private Transform playerTransform;
    [SerializeField] private Cover[] avaliableCovers;

    [SerializeField] Transform handTransform;
    [SerializeField] Weapon enemyWeapon;
    [SerializeField] Transform bulletSpawnPos;
    [SerializeField] HealthBar healthBar;
    [SerializeField] TakeDamageEvent takeDamage;
    [SerializeField] AudioSource takeDamageSound;
    [SerializeField] AudioSource deathSound;

    private Material material;
    private Transform bestCoverSpot;


    private NavMeshAgent agent;
    private Animator animator;
    bool isDead = false;
   
    private Node topNode;
    public bool isProvoked = false;
  

    private float _currentHealth;
    public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }
    [System.Serializable]
    public class TakeDamageEvent : UnityEvent<float>
    {

    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        material = GetComponentInChildren<MeshRenderer>().material;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        healthBar.SetMaxHealth(startingHealth);
        enemyWeapon.Spawn(handTransform, animator);
        shootingRange = enemyWeapon.GetWeaponRange();
        _currentHealth = startingHealth;
        ConstructBehahaviourTree();
    }

    private void ConstructBehahaviourTree()
    {
        IsCovereAvaliableNode coverAvaliableNode = new IsCovereAvaliableNode(avaliableCovers, playerTransform, this);
        GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
        HealthNode healthNode = new HealthNode(this, lowHealthThreshold);
        IsCoveredNode isCoveredNode = new IsCoveredNode(playerTransform, transform);
        ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);
        RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTransform, transform);
        RangeNode shootingRangeNode = new RangeNode(shootingRange, playerTransform, transform);
        ShootNode shootNode = new ShootNode(agent, this, playerTransform);

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode });
        Sequence shootSequence = new Sequence(new List<Node> { shootingRangeNode, shootNode });

        Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvaliableNode, goToCoverNode });
        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, chaseSequence });
        Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredNode, findCoverSelector });
        Sequence mainCoverSequence = new Sequence(new List<Node> { healthNode, tryToTakeCoverSelector });

        topNode = new Selector(new List<Node> { mainCoverSequence, shootSequence, chaseSequence });

        
    }

    private void Update()
    {
     //   Debug.Log(isProvoked);
        if (isDead) return;
        UpdateAnimator();
        EvaluateBehaviour();
  
     if(isProvoked)
        {
            ProvokedBehaviour();
        }
        
        currentHealth += Time.deltaTime * healthRestoreRate;
        healthBar.SetHealth(currentHealth);
      
    }
    private void UpdateAnimator()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        //  Debug.Log(navMeshAgent.velocity);
        if(speed > 0.1f)
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }
        animator.SetFloat("forwardSpeed", speed);
    }

    private void EvaluateBehaviour()
    {
        topNode.Evaluate();
        if (topNode.nodeState == NodeState.FAILURE)
        {
            SetColor(Color.red);
            // agent.isStopped = true;

        }
    }
    public bool IsDead()
    {
        return isDead;
    }
    public void Attack()
    {
        // Vector3 lookDir = (transform.position - playerTransform.position).normalized;
        // transform.forward = lookDir;
        //   agent.updateRotation;
        Vector3 lookPos = playerTransform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
        Debug.Log("isAttacking");
       // transform.LookAt(playerTransform);
        if (!enemyWeapon.isRanged)
        {
            MeleeAttack();
        }
        else
        {
            Shoot();
        }
    }
    public void TakeDamage(float damage)
    {
        isProvoked = true;
        if (isDead) return;
     //   if (!isProvoked)
        {
        //    ProvokedBehaviour();
        }
        currentHealth -= damage;
        takeDamageSound.Play();
        takeDamage.Invoke(damage);
        Debug.Log(currentHealth);
        //  healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            deathSound.Play();
            healthBar.SetHealth(0f);
            Destroy(healthBar.gameObject);
            agent.isStopped = true;
            isDead = true;
            animator.SetTrigger("die");
            this.enabled = false;
        }
    }
    void ProvokedBehaviour()
    {
        AlertNearByEnemies();
       // if (isProvoked) return;
       // isProvoked = true;
       
       // agent.isStopped = false;
        agent.SetDestination(playerTransform.position);
        if (Vector3.Distance(playerTransform.position, transform.position) <= enemyWeapon.GetWeaponRange())
        {
           //  behaviourTreeEnabled = true;
            Attack();
            isProvoked = false;
        }


    }

    private void AlertNearByEnemies()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, alertDistance, Vector3.up, 0);
        foreach (RaycastHit hit in hits)
        {
            EnemyAI nearbyEnemyController = hit.collider.GetComponent<EnemyAI>();
            if (nearbyEnemyController == null) continue;
            if (nearbyEnemyController.isProvoked) continue;
            else
            {
               nearbyEnemyController.isProvoked = true; 
            }
           

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
        if(Vector3.Distance(playerTransform.position, transform.position) <= enemyWeapon.GetWeaponRange())
        {
            playerTransform.GetComponent<Health>().TakeDamage(enemyWeapon.GetWeaponDamage());
        }
        
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }

    public void SetBestCoverSpot(Transform bestCoverSpot)
    {
        Debug.Log("Cover spot set to " + bestCoverSpot);
        this.bestCoverSpot = bestCoverSpot;
    }

    public Transform GetBestCoverSpot()
    {
        return bestCoverSpot;
    }


}
