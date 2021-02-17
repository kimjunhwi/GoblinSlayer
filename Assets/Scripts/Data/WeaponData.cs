using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData
{
    List<Weapon> WeaponDataList = new List<Weapon>();
    List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

    public void Init(string strWeaponCsv)
    {
        data.Clear();
        data = CSVReader.Read(strWeaponCsv);

        for(int nIndex =0; nIndex < data.Count; nIndex++)
        {
            Weapon weaponData = new Weapon();
            weaponData.index = (int)data[nIndex]["index"];
            weaponData.SpriteName = System.Convert.ToString(data[nIndex]["spriteName"]);
            weaponData.Type = (int)data[nIndex]["type"];
            weaponData.Rank = (int)data[nIndex]["rank"];
            weaponData.Damage = (int)data[nIndex]["damage"];
            weaponData.AttackDelay = (float)System.Convert.ToDouble(data[nIndex]["attackDelay"]);
            weaponData.KnockBack = (float)System.Convert.ToDouble(data[nIndex]["knockBack"]);
            weaponData.Skill = (int)data[nIndex]["skill"];
            weaponData.KoreanName = System.Convert.ToString(data[nIndex]["koreaName"]);
            weaponData.EnglishName = System.Convert.ToString(data[nIndex]["englishName"]);
            weaponData.JapanName = System.Convert.ToString(data[nIndex]["japanName"]);
            weaponData.ChinaName = System.Convert.ToString(data[nIndex]["chinaName"]);

            WeaponDataList.Add(weaponData);
        }
    }

    public Weapon GetWeaponData(int _nIndex)
    {
        var weaponData = WeaponDataList.Find((x) => ( x.index == _nIndex ));
        
        if(weaponData == null)
        {
            Debug.LogWarning("Weapon Data NULL!!!");
            return null;
        }

        return weaponData;
    }
}
