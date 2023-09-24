using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "GB jam/New Weapon")]
public class Weapon : ScriptableObject 
{
    public new string name;
    public string description;
    public Sprite weaponSprite;

    [Space]

    public float damage;
    public float fireRate;

    [Space]

    public GameObject[] impactEffects;

    [Space]

    public bool isGun;
    public bool hasSpread;
    public bool hasShotLine;

    [Space]

    public bool isMelle;
    public float range;

    [Space]

    public int upgradeId;
    public int[] upgradeGroup = new int[2];
    public Sprite[] upgradeImages = new Sprite[2];
    public string[] descriptions = new string[2];

    [Space]

    public LayerMask layersToHit;
}
