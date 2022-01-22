using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefsCleaner
{

    [MenuItem("Tools/Clear player prefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.LogWarning("Player Prefs are cleared.");
    }

}
