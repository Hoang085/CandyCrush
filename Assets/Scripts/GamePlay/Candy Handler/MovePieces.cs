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
        if(movingPiece != null)
        {
            Vector2 dir = (Vector2)Input.mousePosition - mouseStart;
            Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));

            newIndex = Point.Clone(movingPiece.index);
            Point addPos = Point.Zero;
            if(dir.magnitude > 2) //if mouse move a distance longer than 64 pixel on the game board 
            {
                if(aDir.x > aDir.y)
                {
                    addPos = new Point((dir.normalized.x > 0) ? 1 : -1 ,0);
                }
                else
                {
                    addPos = new Point(0, (dir.normalized.x > 0) ? 1 : -1);
                }
            }
            newIndex.Add(addPos);

            Vector2 pos = game.GetPositionFromPoint(movingPiece.index);
            if(!newIndex.Equals(pos))
            {
                pos += Point.Multiply(addPos, 1).PointToVector(); //create movement when dragging the piece around
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
    public void DropPiece(NodePiece piece)
    {
        if (movingPiece == null) return;
        Debug.Log("Dropped");
        movingPiece = null; 
    }
}
