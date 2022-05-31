using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighter : MonoBehaviour, IAction
{
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] Weapon defaultWeapon = null;
    [SerializeField] Transform handTransform;

    float timeSinceLastAttack = Mathf.Infinity;
    Health target;
    Weapon currentWeapon;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        if (currentWeapon == null)
        {
            EquipWeapon(defaultWeapon);
        }
    }
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (target == null) return;  // yeh nahi lagaya tha to isInRange calculate karte time null ref error aa raha tha kyuki target null tha starting mei
        if (target.IsDead()) return;


       if (!GetIsInRange())
        {
            GetComponent<EnemyMover>().MoveTo(target.transform.position, 1f);
        }

        // when player is inrange of the target it gets stopped, moves only when not in range of the target
       
        else
        {

            GetComponent<EnemyMover>().Cancel();
            AttackBehaviour();
        }
    }
    public void EquipWeapon(Weapon weapon)
    {


        currentWeapon = weapon;
        Animator animator = GetComponent<Animator>();
        weapon.Spawn(handTransform, animator);

    }

    private void AttackBehaviour()
    {
        transform.LookAt(target.transform);
        if (timeSinceLastAttack > timeBetweenAttacks)
        {
            TriggerAttack();
            timeSinceLastAttack = 0;

        }

    }

    private void TriggerAttack()
    {

        GetComponent<Animator>().ResetTrigger("stopAttack");
        GetComponent<Animator>().SetTrigger("attack");
       
    }
    public void Attack(GameObject combatTarget)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        target = combatTarget.GetComponent<Health>();

    }

    void Hit()     // animation event
    {
        if (currentWeapon == null) return;
        if (GetComponent<Health>().IsDead()) return;
        target.TakeDamage(currentWeapon.GetWeaponDamage());
    }

   private bool GetIsInRange()
   {

         return Vector3.Distance(transform.position, target.GetComponent<Transform>().position) < currentWeapon.GetWeaponRange();
      //  return false;
    }
    public bool CanAttack(GameObject combatTarget)     // here passing gameobject as a parameter so that same script can be used for player as well as enemies, specified the type when this mehtod is called in either of the controller scripts
    {
        if (combatTarget == null) { return false; }
        Health targetToTest = combatTarget.GetComponent<Health>();
        return targetToTest != null && !targetToTest.IsDead();

    }
    private void StopAttack()
    {
        GetComponent<Animator>().ResetTrigger("attack");
        GetComponent<Animator>().SetTrigger("stopAttack");
    }
    public void Cancel()
    {
        StopAttack();
        target = null;
        GetComponent<EnemyMover>().Cancel();
    }
}
