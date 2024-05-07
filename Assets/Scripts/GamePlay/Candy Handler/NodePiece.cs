using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class NodePiece : MonoBehaviour
{
    [SerializeField] Candy[] candies;
    [SerializeField] internal SpriteRenderer sprite;
    internal Candy candyType;
    public Point nodePieceIndex; //represent node piece position
    internal bool updating;

    public Node.NodeContent value;
    public Node.PowerupType powerUpType;

    [HideInInspector]
    public Vector2 pos;

    [HideInInspector]
    public NodePiece piece;

    public void Initialize(Node.NodeContent val, Point p)
    {
        foreach (Candy candy in candies)
        {
            if (val == candy.candyType)
            {
                candyType = candy;
                break;
            }
        }
        //flippedPiece = null;
        value = val;
        SetIndex(p);
        powerUpType = Node.PowerupType.Normal;
        sprite.sprite = candyType.candySprite;
        piece = gameObject.GetComponent<NodePiece>();
    }
    public void SetIndex(Point p)
    {
        nodePieceIndex = p;
        ResetPosition();
        //UpdateName();
    }
    public void MovePosition(Vector2 dir)
    {
        transform.position += (Vector3)dir * Time.deltaTime * 32;
    }
    public void MovePositionTo(Vector2 dir)
    {
        transform.position = Vector2.Lerp(transform.position, dir, Time.deltaTime * 32);
    }
    internal void ResetPosition()
    {
        pos = nodePieceIndex.PointToVector(); //reset position to its current index
    }
    internal bool UpdatePiece(float scale)
    {
        if (Vector3.Distance(transform.position, pos) > 0) //check if piece position has changed
                                                           //pos should be the position of the tile it get dragged toward
        {
            MovePositionTo(pos);
            updating = true;
            GameObject pieceTile = GameObject.Find($"Tile {pos.x / scale} {pos.y / scale}");
            transform.SetParent(pieceTile.transform);
        }
        else
        {
            updating = false;
        }
        return updating;
    }

    private void UpdateName()
    {
        transform.name = candyType.name;
    }
    private void OnMouseDown()
    {
        MovePieces.instance.MovePiece(this);
    }

    public void OnMouseUp()
    {
        MovePieces.instance.DropPiece();
    }

}
