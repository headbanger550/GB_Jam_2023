using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingStation : MonoBehaviour
{
    [SerializeField] float healAmount;
    [SerializeField] int cost;
    [SerializeField] Transform target;
    [SerializeField] float interactionRange;
    

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = target.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, target.position);
        if(distance <= interactionRange && Input.GetKeyDown(KeyCode.J))
        {
            player.health += healAmount;
            player.scoreAmmount -= cost;
        }
    }
}
