using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class UseWeapon : MonoBehaviour
{
    [SerializeField] Weapon baseWeapon;

    //[SerializeField] Transform firePoint;

    [Header("For gun")]
    [SerializeField] float spreadX;
    [SerializeField] float spreadY;

    [Header("For picking up the weapon")]
    [SerializeField] float pickUpRange;
    [SerializeField] Transform rHand;
    [SerializeField] Transform lHand;
    
    private RaycastHit2D _hit;

    /*[HideInInspector]*/ public bool hasPickedUp = false;
    private Transform player;

    private Vector3 additionalSpread;
    private float nextTimeToFire;

    private Arm lArm;
    private Arm rArm;

    void OnEnable()
    {
        player = FindObjectOfType<Player>().transform;

        lArm = lHand.GetComponent<Arm>();
        rArm = rHand.GetComponent<Arm>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(hasPickedUp)
        {
            if(Input.GetKey(KeyCode.J) && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f/baseWeapon.fireRate;
                Shoot();
            }
        }
        else
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            PickUpWeapon(distanceToPlayer);
        }

    }

    void Shoot()
    {
        if(baseWeapon.isGun)
        {
            if(baseWeapon.hasSpread)
            {
                additionalSpread = new Vector3(Random.Range(-spreadX, spreadX), Random.Range(-spreadY, spreadY), 0);
            }
            else
            {
                additionalSpread = Vector3.zero;
            }

            Debug.DrawRay(transform.position, transform.up, Color.red);
            _hit = Physics2D.Raycast(transform.position, transform.up + additionalSpread);
            if(_hit.collider)
            {
                Debug.Log("hit " + _hit.collider.name);
            }
        }
        if(baseWeapon.isMelle)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, baseWeapon.range, baseWeapon.layersToHit);
            foreach(Collider2D enemy in hitEnemies)
            {
                Debug.Log("Damaged");
            }
        }
    }

    void PickUpWeapon(float distance)
    {
        if(distance <= pickUpRange)
        {
            //Left hand
            if(Input.GetKeyDown(KeyCode.J))
            {
                if(lArm.weaponContainer == null)
                {
                    ParentObj(lHand);
                }
                else
                {
                    ThrowWeapon(lArm); 
                    ParentObj(lHand);
                }

                hasPickedUp = true;
            }
            //Right hand
            if(Input.GetKeyDown(KeyCode.K))
            {
                if(rArm.weaponContainer == null)
                {
                    ParentObj(rHand);
                }
                else
                {
                    ThrowWeapon(rArm);
                    ParentObj(rHand);
                }

                hasPickedUp = true;
            }
        }
    }

    void ParentObj(Transform obj)
    {
        transform.SetParent(obj, false);
        transform.position = obj.position;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        lArm.weaponContainer = gameObject;
    }

    void ThrowWeapon(Arm arm)
    {
        Transform gotWeapon = arm.weaponContainer.transform;
        gotWeapon.GetComponent<UseWeapon>().hasPickedUp = false;
        gotWeapon.SetParent(null);

    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(transform.position, baseWeapon.range);
        Gizmos.DrawWireSphere(transform.position, pickUpRange);
    }
}
