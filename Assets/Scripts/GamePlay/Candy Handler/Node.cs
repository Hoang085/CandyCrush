using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node 
{
    public enum NodeContent 
    {
        Blank,
        WrappedCandy,
        HardCandy,
        Lolipop,
        Donut,
        Hole = -1 //a hole
    }; 
    public NodeContent value;
    public Point index; //node position
    
    public Node(NodeContent content, Point p)
    {
        value = content;  
        index = p; 
    }
    internal static NodeContent GetRandomEnum(NodeContent[] EnumValues)
    {
        int randomIndex = Random.Range(0, EnumValues.Length);
        return EnumValues[randomIndex];
    }
}
