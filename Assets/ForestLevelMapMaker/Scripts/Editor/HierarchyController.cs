using System;
using H2910.Level;
using UnityEditor;
using UnityEngine;

namespace Mkey
{
    [InitializeOnLoad]
    public class HierarchyController
    {
        static HierarchyController()
        {
            EditorApplication.hierarchyChanged -= OnHierarchy_Change;
            EditorApplication.hierarchyChanged += OnHierarchy_Change;
        }

        private static void OnHierarchy_Change()
        {
            GameObject g = null;
            MapMaker[] mGs = GameObject.FindObjectsOfType<MapMaker>();
            if (mGs != null && mGs.Length > 0)
            {
                foreach (var mG in mGs)
                {
                    mG.OnManuallyHierChanged();
                }
            }
        }
    }
}
