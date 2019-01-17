using UnityEngine;
using System.Collections;
using UnityEditor;

public class GSIPEditor : MonoBehaviour
{
#if UNITY_EDITOR

    [MenuItem("Window/GSImagePicker/Help")]
    public static void OpenHelp()
    {
        string url = "www.gameslyce.com/assets/GSImagePicker";
        Application.OpenURL(url);
    }

    [MenuItem("Window/GSImagePicker/About")]
    public static void ShowAbout()
    {
        EditorUtility.DisplayDialog("FBLnI Version",
                                    "GameSlyce Image Picker and Taking Pic from Camera Plugin version is 1.0", "OK");
    }

    [MenuItem("Window/GSImagePicker/Credits")]
    public static void ShowCredits()
    {
        EditorUtility.DisplayDialog("Special Thanks",
                                    "Game Slyce: info.gameslyce@gmail.com", "OK");
    }
#endif
}
