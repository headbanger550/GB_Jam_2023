using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Diagnostics;

public class Enemy : MonoBehaviour
{
    [Tooltip("0-melle, 1-projectile, 2-buffer, 3-spawner")]
    [SerializeField] int enemyId;
    public float speed = 200f;
    public float health;
    [SerializeField] int scoreToGive;
    [SerializeField] float nextWaypointDisctance;

    [Space]

    public float enemyAttackRate;
    [SerializeField] float attackRange;
    public float melleDamage;

    [Space]

    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] Transform shotPoint;

    [Space]

    [SerializeField] GameObject[] enemiesToSpawn;

    [Space]

    //For when we have the enemy sprites done
    [SerializeField] Transform enemyGraphics;
    [SerializeField] LayerMask enemyLayer;

    private Transform target;

    private Path path;
    private int currentWaypoint;
    private bool reachedEndOfPath = false;

    private float nextTimeToAttack;

    private bool canMove;
    private bool hasAppliedBuff = false;

    private Seeker seeker;
    private Rigidbody2D rb;
    private WaveSystem waveSystem;

    // Start is called before the first frame update
    void OnEnable()
    {
        waveSystem = FindObjectOfType<WaveSystem>();
        target = FindObjectOfType<Player>().transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveEnemy();
    }

    void Update()
    {
        transform.LookAt(transform.position, Vector3.up);

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);
        if(Time.time >= nextTimeToAttack && distanceToPlayer <= attackRange)
        {
            nextTimeToAttack = Time.time + 1f/enemyAttackRate;
            AttackPlayer();
        }
        if((enemyId == 2 || enemyId == 3) && Time.time >= nextTimeToAttack)
        {
            nextTimeToAttack = Time.time + 1f/enemyAttackRate;
            DoSomethingElse();
        }
    }

    void AttackPlayer()
    {
        switch(enemyId)
        {
            //Melle
            case 0:
                MelleAttack();
                break;

            //Projectile
            case 1:
                ProjectileAttack();
                break;
        }
    }

    //TODO:implement these attacks as events in the enemy animations
    public void MelleAttack()
    {
        Player player = target.GetComponent<Player>();
        if(player != null)
        {
            player.DamagePlayer(melleDamage);
        }
    }

    public void ProjectileAttack()
    {
        GameObject instBullet = Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        instBullet.GetComponent<EnemyBullets>().bulletDamage = melleDamage;
        instBullet.GetComponent<Rigidbody2D>().AddForce((target.position - transform.position) * bulletSpeed * Time.deltaTime);
    }

    void DoSomethingElse()
    {
        switch(enemyId)
        {
            //Buffer
            case 2:
                BuffEnemies();
                break;
            
            case 3:
                SpawnEnemies();
                break;
        }
    }

    public void BuffEnemies()
    {
        Collider2D[] gotEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        foreach(Collider2D enemy in gotEnemies)
        {
            Enemy e = enemy.GetComponent<Enemy>();
            if(e != null && !hasAppliedBuff)
            {
                e.health *= 1.5f;
                e.speed *= 1.5f;
                e.enemyAttackRate *= 1.5f;
                e.melleDamage *= 2f;
                hasAppliedBuff = true;
            }
        }
    }

    public void SpawnEnemies()
    {
        Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)], shotPoint.position, Quaternion.identity);
    }

    void MoveEnemy()
    {
        if(path == null)
            return;
        
        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 dirrection = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = dirrection * speed * Time.deltaTime;

        rb.MovePosition(rb.position + force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDisctance)
        {
            currentWaypoint++;
        }
    }

    public void DamageEnemy(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            waveSystem.enemyCount++;
            target.GetComponent<Player>().scoreAmmount += scoreToGive;
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
