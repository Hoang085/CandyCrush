using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlippedPieces 
{
    public NodePiece one;
    public NodePiece two;
    public FlippedPieces(NodePiece one, NodePiece two)
    {
        this.one = one; 
        this.two = two; 
    }
    public NodePiece GetOtherPiece(NodePiece p)
    {
        if(p == one)
        {
            return two;
        }
        else if(p == two)
        {
            return one; 
        }
        else
        {
            return null;
        }
    }
    public static void SwapPiecesPos(NodePiece piece, NodePiece swappedPiece)
    {
        var tempPos = piece.pos;
        piece.pos = swappedPiece.pos;
        swappedPiece.pos = tempPos;

        var tempIndex = piece.nodePieceIndex;
        piece.nodePieceIndex = swappedPiece.nodePieceIndex;
        swappedPiece.nodePieceIndex = tempIndex;
    }
}
