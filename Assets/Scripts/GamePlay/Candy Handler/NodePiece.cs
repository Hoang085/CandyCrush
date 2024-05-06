using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodePiece : MonoBehaviour
{
    [SerializeField] Candy[] candies;
    [SerializeField] private SpriteRenderer sprite;
    private Candy candyType;
    private Node.NodeContent value;
    public Point index;

    [HideInInspector]
    public Vector2 pos;

    public void Initialize(Node.NodeContent val, Point p)
    {
        foreach (Candy candy in candies)
        {
            if(val == candy.candyType)
            {
                candyType = candy;
                break;
            }
        }
        value = val;
        SetIndex(p);
        sprite.sprite = candyType.candySprite;
    }
    public void SetIndex(Point p)
    {
        index = p;
        ResetPosition();
        UpdateName();
    }
    public void MovePosition(Vector2 dir)
    {
        transform.position += (Vector3)dir * Time.deltaTime * 32;
    }
    public void MovePositionTo(Vector2 dir)
    {
        transform.position = Vector2.Lerp(transform.position, dir, Time.deltaTime * 32);
    }
    private void ResetPosition() 
    //Whenever the index of the game piece changes, the position needs to be updated to match the new index
    {
        pos = index.PointToVector() * 2;
        print(pos);
    }
    private void UpdateName()
    {
        transform.name = candyType.name;
    }
    private void OnMouseDown()
    {
        Debug.Log("Grab " + transform.name);
        MovePieces.instance.MovePiece(this);
    }

    public void OnMouseUp()
    {
        Debug.Log("Let go of " + transform.name);
        MovePieces.instance.DropPiece(this);
    }

}
