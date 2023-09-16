using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseWeapon : MonoBehaviour
{
    [SerializeField] Weapon baseWeapon;

    //[SerializeField] Transform firePoint;

    [Header("For gun")]
    [SerializeField] float spreadX;
    [SerializeField] float spreadY;
    
    private RaycastHit2D _hit;

    private Vector3 additionalSpread;
    private float nextTimeToFire;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        //TODO: add the arm functionality 
        if(Input.GetKey(KeyCode.J) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f/baseWeapon.fireRate;
            Shoot();
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

    void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(transform.position, baseWeapon.range);
    }
}
