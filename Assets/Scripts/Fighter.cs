using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StarterAssets
{
    public class Fighter : MonoBehaviour
    {
       
      
        [SerializeField] Transform handTransform = null;
        [SerializeField] LayerMask enemyLayers;   //using layermasks to identify enemies
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] AudioSource gunShotSound;
        // ADD EQUIPPED WEAPON

        private Animator playerAnimator;
        private StarterAssetsInputs input;
        private ThirdPersonShooterController tps;
        public Weapon currentWeapon = null;
        ThirdPersonController tpc;

        private void Awake()
        {
            input = GetComponent<StarterAssetsInputs>();
            playerAnimator = GetComponent<Animator>();
             tps = GetComponent<ThirdPersonShooterController>();
             tpc = GetComponent<ThirdPersonController>();
        }
        void Start()
        {
            EquipWeapon(defaultWeapon);
        }


        // Update is called once per frame
        void Update()
        {
            if(input.isAttacking && !currentWeapon.isRanged)
            {
                tpc.SetRotateOnMove(true);
                Attack();
            }

            if(currentWeapon.isRanged)
            {
               // tpc.SetRotateOnMove(false);
                  tps.Shoot();
               // gunShotSound.Play();
               
            }

        }

        private void Attack()
        {
            input.isAttacking = false;
         // Debug.Log("Attacking");
            playerAnimator.ResetTrigger("StopAttack");
            playerAnimator.SetTrigger("Attack");
           
          if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
              //  Debug.Log("attack stopped");
                playerAnimator.ResetTrigger("Attack");
               

            }  
          //transform.lookat(enemy)
           
        }

        private void Shoot()
        {

        }

        public void CancelAttack()
        {
            playerAnimator.ResetTrigger("Attack");
            playerAnimator.SetTrigger("StopAttack");
        }
        public void EquipWeapon(Weapon weapon)
        {
            
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            currentWeapon.Spawn(handTransform, animator);
                
        }
        public void RemoveWeapon()
        {
            currentWeapon = null;
            EquipWeapon(defaultWeapon);
           // playerAnimator.runtimeAnimatorController = GetComponent<Animator>();
        }

        public Weapon GetDefaultWeapoon()
        {
            return defaultWeapon;
        }

        void Hit()
        {
          //  Debug.Log("Animation Event Called");
            Collider[] enemiesHit = Physics.OverlapSphere(handTransform.position, currentWeapon.GetWeaponRange(), enemyLayers);
            foreach (Collider enemy in enemiesHit)
            {
                Debug.Log("We hit " + enemy.name);
                if (Vector3.Distance(transform.position, enemy.transform.position) < currentWeapon.GetWeaponRange())
                {
                    enemy.GetComponent<EnemyAI>().TakeDamage(currentWeapon.GetWeaponDamage());
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
          // Gizmos.DrawWireSphere(handTransform.position, currentWeapon.GetWeaponRange());
        }

    }
}
