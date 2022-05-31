using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    [SerializeField] float normalSensitivity;
    [SerializeField] float aimSensitivity;
    [SerializeField] private LayerMask aimColliderMask;
  //  [SerializeField] private Transform debugTransform;
   // [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform bulletSpawnPos;
   // [SerializeField] AudioSource gunShotSound;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController tpc;
    private Fighter fighter;

    private void Awake()
    {
        fighter = GetComponent<Fighter>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        tpc = GetComponent<ThirdPersonController>();
    }
    void Start()
    {
        
    }

   
    void Update()
    {
       

        
    }

    public void Shoot()
    {
        Vector3 mouseWorldPos = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderMask))
        {
           // debugTransform.position = hit.point;
            mouseWorldPos = hit.point;
        }


       
            Vector3 worldAimTarget = mouseWorldPos;
            worldAimTarget.y = transform.position.y; // up down movement nahi chahiye abhi
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;    // normalize isliye kiya kyuki sirf direction chahiye magnitude nahi   Isme pehle atan2 vaali approach try kari thi, voh nahi chali to seedha difference leke normalize kar diya
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        

        if(starterAssetsInputs.aim)
        {
            tpc.SetRotateOnMove(false);
            aimCamera.gameObject.SetActive(true);
            tpc.SetSensitivity(aimSensitivity);
        }
        else
        {
            aimCamera.gameObject.SetActive(false);
            tpc.SetSensitivity(normalSensitivity);
          // tpc.SetRotateOnMove(true);
        }

        if (starterAssetsInputs.isAttacking)
        {
            Vector3 aimDir = (mouseWorldPos - bulletSpawnPos.position).normalized;
            Instantiate(fighter.currentWeapon.GetWeaponProjectile(), bulletSpawnPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
            starterAssetsInputs.isAttacking = false;
        }
    }
}
 