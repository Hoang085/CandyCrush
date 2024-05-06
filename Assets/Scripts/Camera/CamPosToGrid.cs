using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPosToGrid : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        var xPos = GridInteraction.grid.width * GridInteraction.grid.cellSize / 2;
        var yPos = GridInteraction.grid.height * GridInteraction.grid.cellSize / 2;
        mainCam.transform.position = new Vector3(xPos, yPos, -10);
        mainCam.orthographicSize = Mathf.Max(xPos, yPos) + 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
