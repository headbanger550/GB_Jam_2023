using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullets : MonoBehaviour
{
    [SerializeField] float raycastSize;
    [SerializeField] float raycastRange;
    [SerializeField] LayerMask hitLayer;

    [HideInInspector] public float bulletDamage;

    private RaycastHit2D _hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _hit = Physics2D.CircleCast(transform.position, raycastSize, Vector2.down, raycastRange, hitLayer);
        if(_hit.collider)
        {
            Player player = _hit.collider.GetComponent<Player>();
            if(player != null)
            {
                player.DamagePlayer(bulletDamage);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject, 3f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, raycastSize);
    }
}
