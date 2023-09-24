using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractToEnable : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject objToEnable;
    [SerializeField] float interactionRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if(distance <= interactionRange && Input.GetKeyDown(KeyCode.J))
        {
            objToEnable.SetActive(true); 
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            objToEnable.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
