using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UseWeapon : MonoBehaviour
{
    [SerializeField] Weapon baseWeapon;

    //[SerializeField] Transform firePoint;

    [Header("For gun")]
    [SerializeField] float spreadX;
    [SerializeField] float spreadY;
    [SerializeField] int bulletCount;

    [Header("For picking up the weapon")]
    [SerializeField] float pickUpRange;
    [SerializeField] float pickUpTime;
    [SerializeField] Transform rHand;
    [SerializeField] Transform lHand;
    
    private RaycastHit2D _hit;

    /*[HideInInspector]*/ public bool hasPickedUp = false;
    public bool isLeft;
    
    private float pickUpInterval = 0;

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
            //I have no idea how to implement it diferently, so i have to do this fucking madness
            if(isLeft)
            {
                if(Input.GetKey(KeyCode.J) && Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 1f/baseWeapon.fireRate;
                    Shoot();
                }
            }
            else
            {
                if(Input.GetKey(KeyCode.K) && Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 1f/baseWeapon.fireRate;
                    Shoot();
                }
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
            for(int i = 0; i < bulletCount; i++)
            {
                _hit = Physics2D.Raycast(transform.position, transform.up + additionalSpread);
                if(_hit.collider)
                {
                    CreateEffects(baseWeapon.impactEffects);
                    Enemy enemy = _hit.transform.GetComponent<Enemy>();
                    if(enemy != null)
                    {
                        enemy.DamageEnemy(baseWeapon.damage);
                    }
                }
            }
        }
        if(baseWeapon.isMelle)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, baseWeapon.range, baseWeapon.layersToHit);
            foreach(Collider2D enemy in hitEnemies)
            {
                CreateEffects(baseWeapon.impactEffects);
                Enemy e = enemy.GetComponent<Enemy>();
                if(e != null)
                {
                    e.DamageEnemy(baseWeapon.damage);
                }
            }
        }
    }

    void PickUpWeapon(float distance)
    {
        if(distance <= pickUpRange)
        {   
            //Left hand
            if(Input.GetKey(KeyCode.J))
            {
                pickUpInterval += 0.1f;
                if(pickUpTime <= pickUpInterval)
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
                    pickUpInterval = 0;
                    isLeft = true;
                }
            }
            //Right hand
            if(Input.GetKey(KeyCode.K))
            {
                pickUpInterval += 0.1f;
                if(pickUpTime <= pickUpInterval)
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
                    pickUpInterval = 0;
                    isLeft = false;
                }
            }
        }
    }

    void ParentObj(Transform obj)
    {
        transform.SetParent(obj, false);
        transform.position = obj.position;
        transform.rotation = player.rotation;

        lArm.weaponContainer = gameObject;
    }

    void ThrowWeapon(Arm arm)
    {
        //Again i have no fucking idea how to implement this better 
        Transform gotWeapon = arm.weaponContainer.transform;
        UseWeapon wep = gotWeapon.GetComponent<UseWeapon>();
        wep.hasPickedUp = false;
        wep.isLeft = false;
        gotWeapon.SetParent(null);

    }

    void CreateEffects(GameObject[] impcts)
    {
        for(int i = 0; i < impcts.Length; i++)
        {
            GameObject instObj = Instantiate(impcts[i], _hit.point, Quaternion.LookRotation(_hit.normal));
            Destroy(instObj, 2f);
        }
    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(transform.position, baseWeapon.range);
        Gizmos.DrawWireSphere(transform.position, pickUpRange);
    }
}
