using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer sprite;

    public void CheckTile(bool isOffset)
    {
        sprite.color = isOffset ? offsetColor : baseColor;
    }
}
