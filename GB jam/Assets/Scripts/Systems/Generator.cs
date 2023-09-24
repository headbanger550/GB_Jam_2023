using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] float maxHealth;

    [Space]

    [SerializeField] float degradeTime;
    [SerializeField] float repairTime;

    [Space]

    [SerializeField] float interactionRange;
    [SerializeField] float pickUpTime;

    private float pickUpInterval = 0;

    private float startingHealth;
    private bool canRepair = false;

    private Transform player;
    private float startSpeed;

    private bool hasGeneratedWave = false;
    private WaveSystem[] waveSystems;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
        waveSystems = FindObjectsOfType<WaveSystem>();
        maxHealth = health;

        startSpeed = player.GetComponent<Player>().movementSpeed;
        startingHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        //The idea is that after we lose a certain amount of health we would need to repair the machine
        //So the machine i think is going to have a health (which you can later improve)
        health -= degradeTime * Time.deltaTime;
        float degradePercent = (health/maxHealth) * 100;

        if(degradePercent <= 60f)
        {
            Debug.Log("REPAIR ME");
            canRepair = true;
        }

        //TODO: make a little bar that shows the repair delay
        if(distanceToPlayer <= interactionRange && canRepair && Input.GetKey(KeyCode.J))
        {
            pickUpInterval += 0.1f;
            if(pickUpInterval <= pickUpTime)
            {
                StartCoroutine(RepairDelay());
            }
        }

        if(health <= 0)
        {
            Debug.Log("I'm ded :(");
            canRepair = true;

            if(!hasGeneratedWave)
            {
                for (int i = 0; i < waveSystems.Length; i++)
                {
                    waveSystems[i].GenerateWave();
                    hasGeneratedWave = true;
                }
            }
        }
    }

    IEnumerator RepairDelay()
    {
        Player playerObj = player.GetComponent<Player>();
        playerObj.movementSpeed = 0;

        yield return new WaitForSeconds(repairTime);

        health = startingHealth;
        canRepair = false;
        hasGeneratedWave = false;

        playerObj.movementSpeed = startSpeed;

        Debug.Log("Fixed c:");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
