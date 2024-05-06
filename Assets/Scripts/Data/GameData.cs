using UnityEngine;

public class GameData : MonoBehaviour
{
    public static void SetInt(string key, int _default = -1)
    {
        PlayerPrefs.SetInt(key,_default);
    }

    public static void SetFloat(string key, float _default = 0)
    {
        PlayerPrefs.SetFloat(key,_default);
    }

    public static void GetInt(string key, int _default = -1)
    {
        PlayerPrefs.GetInt(key, _default);
    }
    public static void GetFloat(string key, float _default = 0)
    {
        PlayerPrefs.GetFloat(key, _default);
    }
}