using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] float degradeTime;

    [Space]

    [SerializeField] float interactionRange;

    private float startingHealth;
    private bool canRepair = false;

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().transform;

        startingHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        //The idea is that after we lose a certain amount of health we would need to repair the machine
        //So the machine i think is going to have a health (which you can later improve)
        health -= degradeTime * Time.deltaTime;

        //TODO: check if health has gone down certain amount of percent ( because i'm too lazy to implement this now :) ) 
        if(health <= 20)
        {
            Debug.Log("REPAIR ME");
            canRepair = true;
        }

        //TODO: have a delay when the player is fixing the machine
        if(distanceToPlayer <= interactionRange && canRepair && Input.GetKeyDown(KeyCode.J))
        {
            health = startingHealth;
            canRepair = false;
        }
    }
}
