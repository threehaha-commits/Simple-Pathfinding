using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialField : MonoBehaviour
{
    private readonly List<Point> _points = new ();
    private Vector2 _player;
    private Vector2 _target;
    [SerializeField] private TextMeshPro _mesh;
    
    // Game field where:
    // 1 - field border
    // 2 - obstacle
    // 3 - player
    // 4 - target
    // 0 - free cell
    private readonly int[,] _field =
    { 
        {1,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,0,4,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,1},
        {1,0,2,2,2,2,2,2,0,1},
        {1,0,2,0,0,0,0,2,0,1},
        {1,0,2,0,0,2,2,2,0,1},
        {1,0,2,3,0,0,0,0,0,1},
        {1,0,2,2,2,2,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,1},
        {1,0,2,2,2,2,2,2,0,1},
        {1,0,0,0,0,0,0,0,0,1},
        {1,1,1,1,1,1,1,1,1,1}};
    
    private void Awake()
    {
        CreateGameField();
        // Start find path
        var pathFinder = new PathFind(_points, this);
        pathFinder.GetPath(_player, _target);
    }

    private void CreateGameField()
    {
        for (int i = 0; i < _field.GetLength(0); i++)
        {
            for (int j = 0; j < _field.GetLength(1); j++)
            {
                var vertex = new Vector2(i, j); //Point coordinate
                // Create new point with coordinate, text and marker 
                var point = new Point(vertex)
                {
                    Marker = _field[i, j], // Point marker - only for SetTheColorMesh()
                    Text = Instantiate(_mesh, vertex, Quaternion.identity) // Point Text for visualize field
                };
                _points.Add(point);
                //Assign points by their markers
                SetTheColorMesh(_points.Count - 1);
            }
        }
    }

    private void SetTheColorMesh(int i)
    {
        switch (_points[i].Marker)
        {
            case 1: // 1 - field border
                CreateObstacles(i, Color.gray);
                break;
            case 2: // 2 - obstacle
                CreateObstacles(i, Color.black);
                break;
            case 3: // 3 - player
                CreatePlayer(i);
                break;
            case 4: // 4 - target
                CreateEnemy(i);
                break;
        }
    }
    
    private void CreateObstacles(int index, Color color)
    {
        _points[index].Text.color = color;
        _points[index].Text.text = "-1"; // Only visual
        _points.RemoveAt(index);
    }

    private void CreatePlayer(int index)
    {
        _player = _points[index].Current;
        _points[index].Text.text = "3"; // Only for visual
        _points[index].Text.color = Color.green; // Only for visual
        _points.RemoveAt(index);
    }

    private void CreateEnemy(int index)
    {
        _target = _points[index].Current;
        _points[index].Text.text = "4"; // Only for visual
        _points[index].Text.color = Color.red; // Only for visual
        _points.RemoveAt(index);
    }

    // For test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(0);
    }
}
