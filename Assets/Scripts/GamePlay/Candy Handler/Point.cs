using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Point //manage the 2D coordinate of the candies
{
    public int x;
    public int y;
    public Point(int x,  int y)
    {
        this.x = x; 
        this.y = y;
    }
    public void Add(Point p)
    {
        x += p.x;
        y += p.y;   
    }
    public void Multiply(Point p)
    {
        x *= p.x;
        y *= p.y;   
    }
    public Vector2 PointToVector()
    {
        return new Vector2(x, y);   
    }
    public bool Equal(Point p)
    {
        return (x == p.x && y == p.y);    
    }
    public static Point FromVector(Vector2 v)
    {
        return new Point((int)v.x, (int)v.y);
    }
    public static Point FromVector(Vector3 v)
    {
        return new Point((int)v.x, (int)v.y);
    }
    public static Point Multiply(Point p, float m)
    {
        return new Point((int)(p.x * m), (int)(p.y * m)); 
    }
    public static Point Add(Point p1, Point p2)
    {
        return new Point(p1.x + p2.x, p1.y + p2.y);
    }
    public static Point Clone(Point p)
    {
        return new Point(p.x, p.y);
    }
    public static Point Zero
    {
        get { return new Point(0, 0);}
    }
    public static Point Up
    {
        get { return new Point(0, 1);}
    }
    public static Point Down
    {
        get { return new Point(0, -1);}
    }
    public static Point Left
    {
        get { return new Point(-1, 0);}
    }
    public static Point Right
    {
        get { return new Point(1, 0);}
    }
}
