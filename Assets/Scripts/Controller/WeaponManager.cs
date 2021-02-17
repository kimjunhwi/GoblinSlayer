using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public bool isChangeWeapon { get; set; }

    //무기 교체 딜레이
    [SerializeField]
    private float changeWeaponDelayTime;

    [SerializeField]
    private float changeWeaponEndDelayTime;

    [SerializeField]
    private Dictionary<int, Weapon> weaponDictionary = new Dictionary<int, Weapon>();

    [SerializeField]
    private int currectWeaponType;
    public Transform currentWeapon;

}
