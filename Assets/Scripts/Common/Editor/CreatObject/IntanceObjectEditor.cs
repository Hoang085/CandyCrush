/*
using H2910.Defines;
using System;
using Codice.CM.Common;
using UnityEditor;
using UnityEngine;

public class IntanceObjectEditor : EditorWindow
{
    EnemyType enemyType;
    ObjectType objectType;
    ObjectCreateBehaviour ObjCreat;
    [MenuItem("Window/MakeObject")]
    public static void ShowWindow()
    {
        GetWindow<IntanceObjectEditor>("MakeObject");
    }

    void OnEnable()
    {
        titleContent = new GUIContent("Select Object");
    }

    private void OnGUI()
    {
        if (ObjCreat == null)
            ObjCreat = FindObjectOfType<ObjectCreateBehaviour>();

        if (ObjCreat == null)
        {
            //Debug.LogError("Không tìm thấy Object Build Editor trong Hierachy");
            return;
        }
        GUILayout.Label("Enemy Type", EditorStyles.boldLabel);

        var enumValues = Enum.GetValues(typeof(EnemyType));
        var objectArray = Enum.GetValues(typeof(ObjectType));

        void AddMenuItemForEnemyType(GenericMenu menu, string menuPath, EnemyType enemy)
        {  
            menu.AddItem(new GUIContent(menuPath), false, OnEnemySelected, enemy);
        }

        void AddMenuItemForObjectType(GenericMenu menu, string menuPath, ObjectType enemy)
        {
            menu.AddItem(new GUIContent(menuPath), false, OnObjectSelected, enemy);
        }

        if (GUILayout.Button("Enemy Zone1"))
        {       
            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < ObjCreat.DictObjectEnemy.EnemyZone1.Count; i++)
                AddMenuItemForEnemyType(menu, $"{ObjCreat.DictObjectEnemy.EnemyZone1[i]}", ObjCreat.DictObjectEnemy.EnemyZone1[i]);

            menu.ShowAsContext();
        }

        if (GUILayout.Button("Enemy Zone2"))
        {
            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < ObjCreat.DictObjectEnemy.EnemyZone2.Count; i++)
                AddMenuItemForEnemyType(menu, $"{ObjCreat.DictObjectEnemy.EnemyZone2[i]}", ObjCreat.DictObjectEnemy.EnemyZone2[i]);

            menu.ShowAsContext();
        }

        if (GUILayout.Button("Enemy Zone3"))
        {
            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < ObjCreat.DictObjectEnemy.EnemyZone3.Count; i++)
                AddMenuItemForEnemyType(menu, $"{ObjCreat.DictObjectEnemy.EnemyZone3[i]}", ObjCreat.DictObjectEnemy.EnemyZone3[i]);

            menu.ShowAsContext();
        }

        if (GUILayout.Button("Enemy Zone4"))
        {
            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < ObjCreat.DictObjectEnemy.EnemyZone4.Count; i++)
                AddMenuItemForEnemyType(menu, $"{ObjCreat.DictObjectEnemy.EnemyZone4[i]}", ObjCreat.DictObjectEnemy.EnemyZone4[i]);

            menu.ShowAsContext();
        }

        if (GUILayout.Button("Enemy Zone5"))
        {
            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < ObjCreat.DictObjectEnemy.EnemyZone5.Count; i++)
                AddMenuItemForEnemyType(menu, $"{ObjCreat.DictObjectEnemy.EnemyZone5[i]}", ObjCreat.DictObjectEnemy.EnemyZone5[i]);

            menu.ShowAsContext();
        }

        GUILayout.Label("Object Type", EditorStyles.boldLabel);

        if (GUILayout.Button("Trap object"))
        {
            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < ObjCreat.DictObjectEnemy.ListTrapObject.Count; i++)
                AddMenuItemForObjectType(menu, $"{ObjCreat.DictObjectEnemy.ListTrapObject[i]}", ObjCreat.DictObjectEnemy.ListTrapObject[i]);

            menu.ShowAsContext();
        }
        if (GUILayout.Button("Ground object"))
        {
            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < ObjCreat.DictObjectEnemy.ListGroundObject.Count; i++)
                AddMenuItemForObjectType(menu, $"{ObjCreat.DictObjectEnemy.ListGroundObject[i]}", ObjCreat.DictObjectEnemy.ListGroundObject[i]);

            menu.ShowAsContext();
        }
        if (GUILayout.Button("Trigger object"))
        {
            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < ObjCreat.DictObjectEnemy.ListTriggerObject.Count; i++)
                AddMenuItemForObjectType(menu, $"{ObjCreat.DictObjectEnemy.ListTriggerObject[i]}", ObjCreat.DictObjectEnemy.ListTriggerObject[i]);

            menu.ShowAsContext();
        }
    }
    void OnEnemySelected(object type)
    {
        enemyType = (EnemyType)type;
        if (ObjCreat == null)
            ObjCreat = FindObjectOfType<ObjectCreateBehaviour>();
        
        if(ObjCreat == null)
        {
            Debug.LogError("Không tìm thấy Object Build Editor trong Hierachy");
            return;
        }    

        UnityEngine.Object obj = PrefabUtility.InstantiatePrefab(ObjCreat.DictObjectEnemy.DictEnemy[enemyType]);
        SetTranfrom(obj);
    }
    void OnObjectSelected(object type)
    {
        objectType = (ObjectType)type;
        if (ObjCreat == null)
            ObjCreat = FindObjectOfType<ObjectCreateBehaviour>();
        if (ObjCreat == null)
        {
            Debug.LogError("Không tìm thấy Object Build Editor trong Hierachy");
            return;
        }

        UnityEngine.Object obj = PrefabUtility.InstantiatePrefab(ObjCreat.DictObjectEnemy.DictObject[objectType]);
        SetTranfrom(obj);
    }

    void SetTranfrom(UnityEngine.Object obj)
    {
        GameObject gameObj = (GameObject)obj;
        if (ObjCreat.transform.parent == null)
        {
            gameObj.transform.position = ObjCreat.transform.position;

        }
        else
        {
            gameObj.transform.position = ObjCreat.transform.position;
            gameObj.transform.SetParent(ObjCreat.transform.parent.transform);
        }
        Selection.activeObject = obj;
    }
}
*/

