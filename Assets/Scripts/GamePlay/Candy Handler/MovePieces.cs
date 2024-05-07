using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePieces : MonoBehaviour
{
    public static MovePieces instance;
    private Match3 game;

    internal NodePiece movingPiece;
    internal Point newIndex;
    internal Vector2 mouseStart;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        game = GetComponent<Match3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingPiece != null)
        {
            Vector2 dir = (Vector2)Input.mousePosition - mouseStart;
            Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));

            newIndex = Point.Clone(movingPiece.nodePieceIndex);
            Point addPos = Point.Zero;
            if (dir.magnitude > 32) //if mouse move a distance longer than 64 pixel on the game board 
            {
                if (aDir.x > aDir.y)
                {
                    addPos = new Point((dir.normalized.x > 0) ? 1 : -1, 0);
                }
                else if (aDir.y > aDir.x)
                {
                    addPos = new Point(0, (dir.normalized.y > 0) ? 1 : -1);
                }
            }
            newIndex.Add(addPos);

            Vector2 pos = game.GetPositionFromPoint(movingPiece.nodePieceIndex); //getting position based on the index given when instantiating game pieces
            if (!newIndex.Equals(pos))
            {
                //print("Add pos: " + addPos.PointToVector());
                pos += Point.Multiply(new Point(addPos.x, addPos.y), 1).PointToVector(); //create movement when dragging the piece around
                //print("Pos: " + pos);
            }
            movingPiece.MovePositionTo(pos);
        }
    }
    public void MovePiece(NodePiece piece)
    {
        if (movingPiece != null) return;
        movingPiece = piece;
        mouseStart = Input.mousePosition;
    }
    public void DropPiece()
    {
        if (movingPiece == null) return;
        if (!newIndex.Equal(movingPiece.nodePieceIndex))
        //swap piece position if new index is not equal to the current moving piece
        {
            game.FlipPieces(movingPiece.nodePieceIndex, newIndex, true);
        }
        else
        {
            game.ResetPiece(movingPiece);
        }
        movingPiece = null;
    }
}
