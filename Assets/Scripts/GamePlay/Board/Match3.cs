using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Node;

public class Match3 : MonoBehaviour
{
    public ArrayLayout boardLayout;
    public static int width = 10;
    public static int height = 20; 
    private Node[,] gameBoard;
    [SerializeField] Transform board;
    [SerializeField] GameObject nodePiece;
    [SerializeField] GameObject tile;
    [SerializeField] Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        InitializeBoard(); //setting up board and value for each nodes
        VerifyBoard(); //verify if each piece is positioned correctly in board 
        InstantiateBoard(); //instantiating each pieces based on their node value
    }

    private void InitializeBoard()
    {
        gameBoard = new Node[width, height];
        for (int x = 0; x < gameBoard.GetLength(0); x++)
        {
            for (int y = 0; y < gameBoard.GetLength(1); y++)
            {
                gameBoard[x, y] = new Node(boardLayout.rows[y].row[x] ? NodeContent.Hole : FillPiece(), new Point(x, y));
                //rows = height, row = width
            }
        }
    }

    private void VerifyBoard()
    {
        List<NodeContent> remove;
        for (int x = 0; x < gameBoard.GetLength(0); x++)
        {
            for (int y = 0; y < gameBoard.GetLength(1); y++)
            {
                Point p = new Point(x, y);
                var val = GetNodeValueType(p);

                if (val == NodeContent.Blank || val == NodeContent.Hole) continue; //skip if node is blank 

                remove = new List<NodeContent>();

                while(IsConnected(p, true).Count > 0)
                {
                    val = GetNodeValueType(p);
                    if(!remove.Contains(val))
                    {
                        remove.Add(val);
                    }
                    SetValueAtPoint(p, NewValue(ref remove));
                }
            }
        }
    }
    private void InstantiateBoard()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                var val = gameBoard[x, y].value;
                if (val == NodeContent.Blank || val == NodeContent.Hole) continue;

                var spawnTile = Instantiate(tile, new Vector3(x, y) * tile.transform.localScale.x, Quaternion.identity, board);
                spawnTile.name = $"Tile {x} {y}";
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnTile.GetComponent<Tile>().CheckTile(isOffset);

;               GameObject p = Instantiate(nodePiece, new Vector3(x, y, -1) * tile.transform.localScale.x, Quaternion.identity, spawnTile.transform);
                p.transform.localScale = tile.transform.localScale / 4;
                NodePiece node = p.GetComponent<NodePiece>();
                node.Initialize(val, new Point(x, y));
            }
        }
        mainCam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10) * tile.transform.localScale.x;
    }

    private NodeContent NewValue(ref List<NodeContent> remove) //Generate new value to replace the ignored pieces
    {
        List<NodeContent> piecesToIgnore = new List<NodeContent>();
        foreach(var i in remove)
        {
            piecesToIgnore.Add(i);
        }
        if (piecesToIgnore.Count <= 0) return NodeContent.Blank;
        return FillPiece(piecesToIgnore);
    }

    private void SetValueAtPoint(Point p, NodeContent v)
    {
        gameBoard[p.x, p.y].value = v;
    }

    List<Point> IsConnected(Point p, bool main) //check if pieces is connected initially 
    {
        List<Point> connectedNode = new List<Point>();
        var value = GetNodeValueType(p);
        Point[] direction = 
        {
            Point.Up,
            Point.Right,
            Point.Down,
            Point.Left
        };

        foreach(var dir in direction) //checking if there's 2 or more same shapes in the same direction
        {
            List<Point> line = new List<Point>();

            int same = 0;
            for(int i = 1; i < 3; i++)
            {
                Point check = Point.Add(p, Point.Multiply(dir, i));
                if(value == GetNodeValueType(check))
                {
                    same++;
                    line.Add(check);    
                }
            }
            if(same > 1) //if there are more than 1 same shape in this direction then we would know that it's a match 
            {
                AddPoint(ref connectedNode, line); //add these points to the overarching connectedNode list
            }
        }
        //for (int i = 0; i < 2; i++) //checking if we are in the middle of 2 of the same shape xooox
        //{
        //    List<Point> line = new List<Point>();

        //    int same = 0;
        //    Point[] check = { Point.Add(p, direction[i]), Point.Add(p, direction[i + 2]) };

        //    foreach (var next in check)
        //    {
        //        if (value == GetNodeValueType(next))
        //        {
        //            same++;
        //            line.Add(next);
        //        }
        //    }
        //    if (same > 1)
        //    {
        //        AddPoint(ref connectedNode, line); //add these points to the overarching connectedNode list
        //    }
        //}

        //if (main) //check for other matches alongside the current match
        //{
        //    for (int i = 0; i < connectedNode.Count; i++)
        //    {
        //        AddPoint(ref connectedNode, IsConnected(connectedNode[i], false));
        //    }
        //}

        if (connectedNode.Count > 0)
        {
            connectedNode.Add(p);
        }
        return connectedNode;
    }

    private void AddPoint(ref List<Point> connectedNode, List<Point> add) //add points that aren't in the connected list
    {
        foreach(var p in add)
        {
            bool doAdd = true;
            for(int i = 0; i < connectedNode.Count; i++)
            {
                if (connectedNode[i].Equal(p)) //
                {
                    doAdd = false;
                    break;
                }
            }
            if(doAdd) connectedNode.Add(p); 
        }
    }

    private NodeContent GetNodeValueType(Point p) //get node value at point p
    {
        if (p.x < 0 || p.x >= width || p.y < 0 || p.y >= height) return NodeContent.Hole;

        return gameBoard[p.x, p.y].value; 
    }

    private NodeContent FillPiece() //fill node with random pieces
    {
        NodeContent nodeContent = GetRandomEnum(new NodeContent[] { NodeContent.WrappedCandy,
            NodeContent.HardCandy, NodeContent.Lolipop, NodeContent.Donut});

        return nodeContent;
    }
    private NodeContent FillPiece(List<NodeContent> ignorePieces) 
    {
        NodeContent nodeContent = GetRandomEnum(new NodeContent[] { NodeContent.WrappedCandy,
            NodeContent.HardCandy, NodeContent.Lolipop, NodeContent.Donut});

        while (ignorePieces.Contains(nodeContent))
        {
            foreach (var piece in ignorePieces)
            {
                if (nodeContent.Equals(piece))
                {
                    nodeContent = GetRandomEnum(new NodeContent[] { NodeContent.WrappedCandy,
                        NodeContent.HardCandy, NodeContent.Lolipop, NodeContent.Donut});
                    print("Picking different piece");
                }
            }
        }

        return nodeContent;
    }
    public Vector2 GetPositionFromPoint(Point p)
    {
        return p.PointToVector();
    }
}
