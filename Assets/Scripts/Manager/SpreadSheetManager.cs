using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

using System.Text.RegularExpressions;

public static class SpreadSheetManager 
{
    internal static IEnumerator LoadGoogleSpreadAllSheetName(Action<string> callback)
    {
        string allSheetName = null;

        string URL = "https://script.google.com/macros/s/AKfycbzoYhO37kNZu02_U3mMIqlCLh5ebQAfQos6SoRStNiXRtxxUyoHK7kw/exec";

        UnityWebRequest www = UnityWebRequest.Get(URL);

        yield return www.SendWebRequest();

        if(www.isNetworkError)
        {
            Debug.Log("Download Error: " + www.error);
        }
        else
        {
            Debug.Log("LoadSuccess!!");
            allSheetName = www.downloadHandler.text;
        }

        callback(allSheetName);
    }

    internal static IEnumerator LoadGoogleSpreadSheet(string _sheetName, Action<string> callback)
    {
        string downloadData = null;

        string URL = "https://docs.google.com/spreadsheets/d/1_fxlP_yJP8OsgIGbdL2e8oT9mV_hq28tJXvgSVrlZX0/export?format=csv";

        UnityWebRequest www = UnityWebRequest.Get(URL);

        yield return www.SendWebRequest();

        if(www.isNetworkError)
        {
            Debug.Log(_sheetName);
            Debug.Log("Download Error: " + www.error);
        }
        else
        {
            Debug.Log("LoadSuccess!!");
            downloadData = www.downloadHandler.text;
        }

        callback(downloadData);
    }
}


 
public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };
    
    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        
        var lines = Regex.Split (file, LINE_SPLIT_RE);
        
        if(lines.Length <= 1) return list;
        
        var header = Regex.Split(lines[0], SPLIT_RE);
        
        for(var i=1; i < lines.Length; i++) {
            
            var values = Regex.Split(lines[i], SPLIT_RE);
            if(values.Length == 0 ||values[0] == "") continue;
            
            var entry = new Dictionary<string, object>();
            
            for(var j=0; j < header.Length && j < values.Length; j++ ) {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
 
                value = value.Replace("<br>", "\n"); // 추가된 부분. 개행문자를 \n대신 <br>로 사용한다.
                value = value.Replace("<c>", ",");
 
                object finalvalue = value;
                int n;
                float f;
                if(int.TryParse(value, out n)) {
                    finalvalue = n;
                } else if (float.TryParse(value, out f)) {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add (entry);
        }
        return list;
    }
}