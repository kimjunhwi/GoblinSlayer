using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Localization;
using UnityEditor.Localization.Editor;

public class DataManager : MonoBehaviour
{
    int nLoadCount = 1000000000;
    bool isDataAllLoad = false;
    private WeaponData weaponData = new WeaponData();

    public WeaponData WeaponData { get { return weaponData; }}

    void Update()
    {
        if(nLoadCount == 0)
            isDataAllLoad = true;
    }

    public void Load()
    {
        isDataAllLoad = false;
        nLoadCount = 1000000000;

        // 나중에는 다운받고 로컬에서 로드하는 식으로 바꿈
        // 현재는 그냥 로드만 
        StartCoroutine(SpreadSheetManager.LoadGoogleSpreadAllSheetName((allSheetString) =>
        {
            string[] sheetList = allSheetString.Split(',');

            nLoadCount = sheetList.Length;

            foreach (var sheetName in sheetList)
            {
                switch(sheetName)
                {
                    case "Weapon": 
                    StartCoroutine(SpreadSheetManager.LoadGoogleSpreadSheet(sheetName, (ResultCsv) =>{ weaponData.Init(ResultCsv); nLoadCount --;}));
                    break;

                    // 하나 씩 추가
                    // 개선 여지 o
                } 
            }
        }));
    }

    public bool GetDownloadEnd()
    {
        return isDataAllLoad;
    }

    string[] SplitStringToList(string splitString)
    {
        string[] returnStringArray = splitString.Split(',');

        return returnStringArray;
    }
}
