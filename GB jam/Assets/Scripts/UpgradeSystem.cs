using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.TerrainTools;

public class UpgradeSystem : MonoBehaviour
{
    [SerializeField] GameObject armObj;
    [SerializeField] bool isButton;

    [Space]

    [SerializeField] int upgradeType;
    [SerializeField] Image upgradeImage;
    [SerializeField] TextMeshProUGUI descriptionText;

    private Weapon weaponObj;
    private UseWeapon weapon;

    private Image weaponImage;
    private Button[] buttons;

    // Start is called before the first frame update
    void OnEnable()
    {
        weaponImage = GetComponent<Image>();
        buttons = GetComponentsInChildren<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isButton)
        {
            GetAndDisplayWeapon(armObj);
        }
    }

    void GetAndDisplayWeapon(GameObject arm)
    {
        weapon = arm.GetComponentInChildren<UseWeapon>();
        if(weapon != null)
        {
            weaponObj = weapon.baseWeapon;

            //Display the weapons image and the stats
            weaponImage.sprite = weaponObj.weaponSprite;
        }
    }

    void GetUpgrade()
    {
        switch(weaponObj.upgradeId)
        {
            //Melle 
            case 0: 
                DisplayUpgrade(upgradeType);
                break;

            //Pistol and assault rifle
            case 1:
                DisplayUpgrade(upgradeType);
                break;

            //Shotguns
            case 2:
                DisplayUpgrade(upgradeType);
                break;
        }
    }

    void DisplayUpgrade(int upgrdType)
    {
        switch(upgrdType)
        {
            case 0:
                upgradeImage.sprite = weaponObj.upgradeImages[0];
                descriptionText.text = weaponObj.descriptions[0];
                break;

            case 1:
                upgradeImage.sprite = weaponObj.upgradeImages[0];
                descriptionText.text = weaponObj.descriptions[0];
                break;
        }
    }

    public void ApplyUpgrade()
    {
        for(int i = 0; i < weaponObj.upgradeGroup.Length; i++)
        {
            int group = weaponObj.upgradeGroup[i];
            switch(group)
            {
                //melle
                case 0:
                    weaponObj.range += 3;
                    weaponObj.fireRate -= 1;
                    weaponObj.damage += 3;
                    Debug.Log("Melle 0");
                    break;
                case 1:
                    weaponObj.range -= 1.5f;
                    weaponObj.fireRate += 2;
                    weaponObj.damage -= 1.5f;
                    Debug.Log("Melle 1");
                    break;
                
                //Pistol, assault weapon
                case 2:
                    weapon.bulletCount = 2;
                    weapon.spreadX += 0.02f;
                    weapon.spreadY += 0.02f;
                    break;
                case 3:
                    weaponObj.fireRate += 3;
                    weapon.spreadX += 0.02f;
                    weapon.spreadY += 0.02f;
                    break;
                
                //Shotguns
                case 4:
                    weapon.bulletCount = 1;
                    weaponObj.damage += 3;
                    weapon.spreadX -= 0.01f;
                    weapon.spreadY -= 0.01f;
                    break;
                case 5:
                    weapon.bulletCount += 3;
                    weapon.spreadX += 0.01f;
                    weapon.spreadY += 0.01f;
                    weaponObj.damage -= 1.5f;
                    break;
            }
        }
    }
}
