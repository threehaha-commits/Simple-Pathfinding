using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFind
{
    // Only for different color path
    private int pathColorIndex;
    private int _pathColorIndex
    {
        get => pathColorIndex;
        set => pathColorIndex = value > 2 ? 2 : value;
    }

    private readonly Queue<Vector2> _path = new(); // Path that we will return
    private readonly List<Point> _points = new();
    private readonly Color[] _color = { Color.blue, Color.yellow, Color.magenta };
    private readonly float _offsetValue = 1.5f; // Radius find point
    private readonly MonoBehaviour _mono; // For coroutine
    private Point _currentPoint; // Where are we stand
    private Point _player; // Our player - green point
    private Point _target; // Our target - red point
    private Point _startPlayer; // Start point - for restart find path
    
    public PathFind(List<Point> points, MonoBehaviour mono)
    {
        _mono = mono;
        _points = points;
    }

    public Queue<Vector2> GetPath(Vector2 player, Vector2 target)
    {
        _player = new Point(player);
        _currentPoint = new Point(player);
        _startPlayer = new Point(player);
        _target = new Point(target);
        _points.Add(_target);
        _mono.StartCoroutine(Find());
        return _path;
    }
    
    private Vector2 FindPointsAround()
    {
        // Find all free point around point our stand and save them to List
        List<Point> points = new List<Point>();
        for (int i = 0; i < _points.Count; i++)
        {
            var dist = Distance(_currentPoint.Current, _points[i].Current);
            if (dist <= _offsetValue)
                points.Add(_points[i]);
        }
        
        // If no have free points around
        if (PointsCountIsZero(points))
        {
            _path.Clear();
            _pathColorIndex++; // Find path with next color
            _currentPoint.Current = _startPlayer.Current; // Reset our point
            return _currentPoint.Current;
        }
        return ChooseOptimalPoint(points);
    }
    
    private Vector2 ChooseOptimalPoint(List<Point> points)
    {
        // Find optimal point by distance
        var m_dist = (double)Mathf.Infinity;
        for (int i = 0; i < points.Count; i++)
        {
            var dist = Distance(_target.Current, points[i].Current);
            if (dist <= m_dist)
            {
                m_dist = dist;
                _currentPoint.Current = points[i].Current;
            }
        }
        
        // Draw marker and delete point from List
        for (int i = 0; i < _points.Count; i++)
        {
            if(_points[i].Current.Equals(_currentPoint.Current) // If found point is exist
               && 
               !_points[i].Current.Equals(_target.Current)) // If found point is not target point
            {
                _points[i].Text.color = _color[_pathColorIndex];
                _points.RemoveAt(i);
            }
        }

        return _currentPoint.Current;
    }
    
    // Main finder generator :)
    private IEnumerator Find()
    {
        var timeStep = 0.1f;
        var dist = (double)Mathf.Infinity;
        while (dist > 1)
        {
            dist = Distance(_player.Current, _target.Current);
            _path.Enqueue(FindPointsAround());
            _player.Current = _currentPoint.Current;
            yield return new WaitForSeconds(timeStep);
        }
    }

    private double Distance(Vector2 a, Vector2 b)
    {
        return Math.Round(Vector2.Distance(a, b), 1);
    }

    private bool PointsCountIsZero(List<Point> points)
    {
        return points.Count == 0;
    }
}
