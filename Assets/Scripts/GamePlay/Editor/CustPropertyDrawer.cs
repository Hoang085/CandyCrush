using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(ArrayLayout))]
public class CustPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PrefixLabel(position, label);
        Rect newposition = position;

        newposition.y += 18f;
        SerializedProperty data = property.FindPropertyRelative("rows");

        if (data.arraySize != Match3.height)
            data.arraySize = Match3.height;

        //data.rows[0][]
        for (int j = 0; j < Match3.height; j++)
        {
            SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
            newposition.height = 18f;

            if (row.arraySize != Match3.width)
                row.arraySize = Match3.width;

            newposition.width = position.width / Match3.width;

            for (int i = 0; i < Match3.width; i++)
            {
                EditorGUI.PropertyField(newposition, row.GetArrayElementAtIndex(i), GUIContent.none);
                newposition.x += newposition.width;
            }

            newposition.x = position.x;
            newposition.y += 18f;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 18f * (Match3.height + 2);
    }
}
