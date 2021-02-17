using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public bool isChangeWeapon { get; set; }

    //무기 교체 딜레이
    [SerializeField]
    private float changeWeaponDelayTime;

    [SerializeField]
    private float changeWeaponEndDelayTime;

    [SerializeField]
    private List<Weapon> weaponDictionary = new List<Weapon>();

    [SerializeField]
    private int currectWeaponType;
    public Transform currentWeapon;

    void Init(int nFirstWeapon, int? nSecondWeapon)
    {
        weaponDictionary[0] = GameManager.Instance._DataManager.WeaponData.GetWeaponData(nFirstWeapon);

        if(nSecondWeapon != null) weaponDictionary[1] = GameManager.Instance._DataManager.WeaponData.GetWeaponData((int)nSecondWeapon);
    }

    public void WeaponChange(int nIndex)
    {

    }
}
