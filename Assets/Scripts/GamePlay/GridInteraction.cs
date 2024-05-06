using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridInteraction : MonoBehaviour
{
    internal static Grid grid;
    // Start is called before the first frame update
    void Awake()
    {
        grid = new Grid(2, 4, 5f, new Vector3(0, 0, 0));
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 56);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
    }
}
