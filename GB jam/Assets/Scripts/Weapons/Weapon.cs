using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "GB jam/New Weapon")]
public class Weapon : ScriptableObject 
{
    public new string name;
    public string description;
    public float damage;
    public float fireRate;

    [Space]

    public bool isGun;
    public bool hasSpread;

    [Space]

    public bool isMelle;
    public float range;

    [Space]

    public LayerMask layersToHit;
}
