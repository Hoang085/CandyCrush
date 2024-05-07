using Bayat.SaveSystem;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class ClearData
{
    [MenuItem("Data/Clear Data")]
    static void ClearLocalData()
    {
        SaveSystemAPI.ClearAsync();
    }
}
