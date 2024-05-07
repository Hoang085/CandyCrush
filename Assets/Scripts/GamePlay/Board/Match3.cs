using H2910.GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Node;

public class Match3 : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Transform board;
    [SerializeField] GameObject tile;

    [Header("Prefabs")]
    [SerializeField] GameObject nodePiece;
    [SerializeField] GameObject killedPiece;

    [Header("Board Grid")]
    public ArrayLayout boardLayout;
    private Node[,] gameBoard;
    public static int width = 10;
    public static int height = 20;
    int[] fills; //keeping track of how many time we need to fill a blank hole on each colume 

    [SerializeField] Camera mainCam;

    List<NodePiece> updatePieces;
    List<FlippedPieces> flippedPieces;
    // Start is called before the first frame update
    void Start()
    {
        updatePieces = new List<NodePiece>();
        flippedPieces = new List<FlippedPieces>();
        fills = new int[width];
        InitializeBoard(); //setting up board and value for each nodes
        VerifyBoard(); //verify if each piece is positioned correctly in board 
        InstantiateBoard(); //instantiating each pieces based on their node value
    }
    private void Update()
    {
        List<NodePiece> finishedUpdate = new List<NodePiece>();
        //print("updatePieces list count: " + updatePieces.Count);
        foreach (var piece in updatePieces)
        {
            if (!piece.UpdatePiece(tile.transform.localScale.x))
            {
                //print("Store piece to finished update");
                finishedUpdate.Add(piece);
            }
        }
        foreach (var piece in finishedUpdate)
        {
            FlippedPieces flip = GetFlipped(piece);
            NodePiece flippedPiece = null;

            Point pieceTrueIndex = Point.Multiply(piece.nodePieceIndex, 1 / tile.transform.localScale.x);
            //index need to be scaled down for IsConnected function to work perfectly

            List<Point> connected = IsConnected(pieceTrueIndex, true, piece.powerUpType);

            if (connected.Count == 4) //remove piece from being killed and convert it to special piece
            {
                print("Convert to Special");

                connected.Remove(connected.Last()); //prevent the swiped piece to be removed if match 4

                piece.powerUpType = PowerupType.Stripped;
                piece.sprite.sprite = piece.candyType.strippedCandySprite;
            }

            bool wasFlipped = (flip != null);
            if (wasFlipped) //if flipped then make proceed to check if piece matches with other piece
            {
                flippedPiece = flip.GetOtherPiece(piece);

                Point flippedPieceTrueIndex = Point.Multiply(flippedPiece.nodePieceIndex, 1 / tile.transform.localScale.x);
                //same as piece true index

                AddPoint(ref connected, IsConnected(flippedPieceTrueIndex, true, flippedPiece.powerUpType));
            }
            if (connected.Count == 0) //if pieces doesn't matches
            {
                if (wasFlipped)
                {
                    FlipPieces(piece.nodePieceIndex, flippedPiece.nodePieceIndex, false);
                }
            }
            else //if pieces matches
            {
                foreach (Point point in connected) //remove the connected node pieces
                {
                    Node node = GetNodeAtPoint(point);
                    NodePiece nodePiece = node.GetPiece();
                    KillPiece(point, nodePiece.GetComponentInChildren<SpriteRenderer>());

                    if (nodePiece != null)
                    {
                        //nodePiece.gameObject.SetActive(false);

                        ObjectPoolManager.ReturnObjectToPool(nodePiece.gameObject);
                    }
                    node.SetPiece(null);
                }
                ApplyGravityToBoard();
            }
            flippedPieces.Remove(flip); //remove the piece in flip list after update
            updatePieces.Remove(piece);
        }
    }

    private void KillPiece(Point point, SpriteRenderer deadSprite)
    {
        Vector2 piecePos = point.PointToVector();
        GameObject killed = ObjectPoolManager.SpawnObject(this.killedPiece, piecePos * tile.transform.localScale.x,
                            Quaternion.identity, ObjectPoolManager.PoolType.DeadCandy);
        KilledPiece killedPiece = killed.GetComponent<KilledPiece>();

        killedPiece.Initialize(deadSprite.sprite);
    }

    void ApplyGravityToBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Point p = new Point(x, y);
                Node node = GetNodeAtPoint(p);
                var val = GetNodeValueType(p);

                if (val != NodeContent.Blank) continue; //if it's not blank, move onto the next y checkpoint

                //if found blank, proceed with the current y as a checkpoint and check all of its proceeding y's upward
                for (int scoutY = (y + 1); scoutY <= height; scoutY++)
                {
                    Point next = new Point(x, scoutY);
                    var nextVal = GetNodeValueType(next);

                    if (nextVal == NodeContent.Blank) continue; //onto the next node upward if current node is blank

                    if (nextVal != NodeContent.Hole) //if current node is a piece
                    {
                        Node endNode = GetNodeAtPoint(next);
                        NodePiece piece = endNode.GetPiece();

                        //Move the piece down to the bottom of the found blank node
                        node.SetPiece(piece);
                        piece.pos = p.PointToVector() * tile.transform.localScale.x;
                        piece.nodePieceIndex = Point.Multiply(p, tile.transform.localScale.x);
                        updatePieces.Add(piece);

                        //replace the fell node value with blank
                        endNode.SetPiece(null);
                    }
                    else //use the deactivated pieces or create a new piece to fill holes on top 
                    {
                        //Generating new piece from pool
                        var newVal = FillPiece();
                        GameObject revived = ObjectPoolManager.SpawnObject(nodePiece, new Vector3(x, height + fills[x], -1) * tile.transform.localScale.x,
                            Quaternion.identity, ObjectPoolManager.PoolType.Candy);
                        NodePiece revivedPiece = revived.GetComponent<NodePiece>();
                        revivedPiece.Initialize(newVal, Point.Multiply(new Point(x, height + fills[x]), (int)tile.transform.localScale.x));

                        //Moving the revived new piece on top 
                        Node hole = GetNodeAtPoint(p);
                        hole.SetPiece(revivedPiece);
                        revivedPiece.pos = p.PointToVector() * tile.transform.localScale.x;
                        revivedPiece.nodePieceIndex = Point.Multiply(p, tile.transform.localScale.x);
                        ResetPiece(revivedPiece);

                        //With each piece moved on top, increase the fill distance
                        fills[x]++;
                    }
                    break;
                }
            }
        }
    }
    FlippedPieces GetFlipped(NodePiece p)
    {
        FlippedPieces flip = null;
        for (int i = 0; i < flippedPieces.Count; i++)
        {
            if (flippedPieces[i].GetOtherPiece(p) != null)
            {
                flip = flippedPieces[i];
                break;
            }
        }
        return flip;
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

                while (IsConnected(p, true, PowerupType.Normal).Count > 0)
                {
                    val = GetNodeValueType(p);
                    if (!remove.Contains(val))
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
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var val = gameBoard[x, y].value;
                if (val == NodeContent.Blank || val == NodeContent.Hole) continue;

                //Instantiating tiles
                var spawnTile = Instantiate(tile, new Vector3(x, y) * tile.transform.localScale.x, Quaternion.identity, board);
                spawnTile.name = $"Tile {x} {y}";
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnTile.GetComponent<Tile>().CheckTile(isOffset);

                //Instantiating game pieces 
                GameObject piece = ObjectPoolManager.SpawnObject(this.nodePiece, new Vector3(x, y, -1) * tile.transform.localScale.x, Quaternion.identity, ObjectPoolManager.PoolType.Candy);
                NodePiece nodePiece = piece.GetComponent<NodePiece>();
                nodePiece.Initialize(val, Point.Multiply(new Point(x, y), (int)tile.transform.localScale.x)); //set index to scale with tile in order to get its position stable on the board
                Node currentNode = GetNodeAtPoint(new Point(x, y));
                currentNode.SetPiece(nodePiece);
            }
        }
        mainCam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10) * tile.transform.localScale.x;
    }

    private NodeContent NewValue(ref List<NodeContent> remove) //Generate new value to replace the ignored pieces
    {
        List<NodeContent> piecesToIgnore = new List<NodeContent>();
        foreach (var i in remove)
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

    public void ResetPiece(NodePiece piece)
    {
        piece.ResetPosition();
        //piece.flippedPiece = null; //flippedPiece should be null if piece got reset
        updatePieces.Add(piece);
    }
    public void FlipPieces(Point p1, Point p2, bool root) //swap 2 piece position when dragged to a different tile
    {
        var value1 = GetNodeValueType(p1, tile.transform.localScale.x); //guarantee to get the correct value even if the index is not correct
        if (value1 == NodeContent.Blank || value1 == NodeContent.Hole) return;

        Node node1 = GetNodeAtPoint(p1, p1, tile.transform.localScale.x);
        NodePiece piece1 = node1.GetPiece();

        var value2 = GetNodeValueType(p2, tile.transform.localScale.x);
        if (value2 == NodeContent.Blank || value2 == NodeContent.Hole)
        {
            ResetPiece(piece1);
        }
        else
        {
            Node node2 = GetNodeAtPoint(p1, p2, tile.transform.localScale.x);
            NodePiece piece2 = node2.GetPiece();

            node1.SetPiece(piece2); //change node value to match the swapped piece
            node2.SetPiece(piece1);

            if (root) flippedPieces.Add(new FlippedPieces(piece1, piece2)); //only add pieces to list if it's their first time swapping

            if (flippedPieces.Count >= 1)
            {
                FlippedPieces.SwapPiecesPos(piece1, piece2);
            }

            updatePieces.Add(piece1); //adding the 2 pieces in a list and proceed to swap their position
            updatePieces.Add(piece2);
        }

    }
    List<Point> IsConnected(Point p, bool main, PowerupType type) //check if pieces is connected initially 
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

        foreach (var dir in direction) //checking if there's 2 or more same shapes in the same direction
        {
            List<Point> line = new List<Point>();

            int same = 0;
            for (int i = 1; i < 3; i++)
            {
                Point check = Point.Add(p, Point.Multiply(dir, i));
                if (value.Equals(GetNodeValueType(check)))
                {
                    same++;
                    line.Add(check);
                }
            }
            if (same > 1) //if there are more than 1 same shape in this direction then we would know that it's a match 
            {
                AddPoint(ref connectedNode, line); //add these points to the overarching connectedNode list
            }
        }
        for (int i = 0; i < 2; i++) //checking if we are in the middle of 2 of the same shape oOoox 
        {
            List<Point> line = new List<Point>();

            int same = 0;
            Point[] check = { Point.Add(p, direction[i]), Point.Add(p, direction[i + 2]) };

            foreach (var next in check)
            {
                if (value.Equals(GetNodeValueType(next)))
                {
                    same++;
                    line.Add(next);
                }
            }
            if (same > 1)
            {
                AddPoint(ref connectedNode, line); //add these points to the overarching connectedNode list

            }
        }

        if (main) //check for other matches alongside the current match (ConnectedNode would look like this xx0 00xx)
        {
            for (int i = 0; i < connectedNode.Count; i++)
            {
                AddPoint(ref connectedNode, IsConnected(connectedNode[i], false, type));
            }
        }
        switch (type)
        {
            case PowerupType.Normal:
                return connectedNode;
            case PowerupType.Stripped:
                print("Stripped Pattern");
                List<Point> candyColumn = new List<Point>();
                for(int y = 0; y < height; y++)
                {
                    Point point = new Point(p.x, y);

                    //if (!connectedNode.Any(duplicatePoint => duplicatePoint.Equals(point)))
                    //{
                    //    print("Skip dup piece");
                    //    continue;
                    //}

                    candyColumn.Add(point);
                } 
                connectedNode.AddRange(candyColumn);
                return connectedNode;
            default:
                return null;   
        }
    }

    private void AddPoint(ref List<Point> connectedNode, List<Point> add) //add connected points to the list of current connected pieces
    {
        foreach (var p in add)
        {
            bool doAdd = true;
            for (int i = 0; i < connectedNode.Count; i++)
            {
                if (connectedNode[i].Equal(p)) //
                {
                    doAdd = false;
                    break;
                }
            }
            if (doAdd) connectedNode.Add(p);
        }
    }

    private NodeContent GetNodeValueType(Point p) //get node value at point p
    {
        if (p.x < 0 || p.x >= width || p.y < 0 || p.y >= height) return NodeContent.Hole;

        return gameBoard[p.x, p.y].value;
    }
    private NodeContent GetNodeValueType(Point p, float scale) //get node value at point p depending on scale value
    {
        if (p.x < 0 || p.x >= width * scale - 1 || p.y < 0 || p.y >= height * scale - 1) return NodeContent.Hole;

        int scaledPx = Mathf.CeilToInt(p.x / scale);
        int scaledPy = Mathf.CeilToInt(p.y / scale);
        p = new Point(scaledPx, scaledPy);

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
                }
            }
        }

        return nodeContent;
    }
    public Vector2 GetPositionFromPoint(Point p)
    {
        return p.PointToVector();
    }
    public Node GetNodeAtPoint(Point p)
    {
        return gameBoard[p.x, p.y];
    }
    public Node GetNodeAtPoint(Point draggedP, Point newP, float scale)
    {
        int scaledPx = 0;
        int scaledPy = 0;
        if (draggedP == newP || draggedP.x < newP.x || draggedP.y < newP.y)
        {
            scaledPx = Mathf.CeilToInt(newP.x / scale);
            scaledPy = Mathf.CeilToInt(newP.y / scale);
        }
        else //if drag left or down, scale the point value down to match with the board position cause their pos are negative compare to up and right
        {
            if (draggedP.x > newP.x || draggedP.y > newP.y)
            {
                scaledPx = Mathf.FloorToInt(newP.x / scale);
                scaledPy = Mathf.FloorToInt(newP.y / scale);
            }
        }
        newP = new Point(scaledPx, scaledPy);

        return gameBoard[newP.x, newP.y];
    }
}
