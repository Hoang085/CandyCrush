using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Candy", menuName = "Candy")]
public class Candy : ScriptableObject
{
    public new string name;

    public Sprite candySprite;

    public Sprite strippedCandySprite;

    public Node.NodeContent candyType;
}
