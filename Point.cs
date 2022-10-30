using TMPro;
using UnityEngine;

public class Point
{
    public int Marker; // Only for visual
    public Vector2 Current; // Point coordinate
    public TextMeshPro Text; // Distance between point and player
    
    public Point(Vector2 current)
    {
        Current = current;
    }
}