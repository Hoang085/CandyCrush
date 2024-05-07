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
    public enum PowerupType
    {
        Normal,
        Stripped,
        Bomb,
        Rainbow
    }; 
    public NodeContent value;
    public Point index; //node position
    public NodePiece piece;
    
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
    internal void SetPiece(NodePiece piece)
    {
        this.piece = piece;
        value = (this.piece == null) ? NodeContent.Blank : this.piece.value;
        if (piece == null) return;
        this.piece.SetIndex(piece.nodePieceIndex);
    }
    internal NodePiece GetPiece()
    {
        return piece;
    }
}
