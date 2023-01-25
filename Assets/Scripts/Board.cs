using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Vector2Int gridSize = Vector2Int.one;
    public Vector2Int GetGridSize { get { return gridSize; } }

    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private GameObject[] dotPrefabs;

    // private BackgroundTile[,] tiles;
    public GameObject[,] Dots { private set; get; }

    // Start is called before the first frame update
    private void Start()
    {
        // tiles = new BackgroundTile[gridSize.x, gridSize.y];
        Dots = new GameObject[gridSize.x, gridSize.y];
        Setup();
    }

    private void Setup()
    {
        if (dotPrefabs == null || dotPrefabs.Length == 0)
        {
            Debug.LogError("No Dot prefabs were assigned.");
            return;
        }

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2 spawnPosition = new(x, y);

                GameObject tile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity, transform);
                tile.name = $"({x}, {y})";

                int dotIndex = Random.Range(0, dotPrefabs.Length);
                GameObject dot = Instantiate(dotPrefabs[dotIndex], spawnPosition, Quaternion.identity, transform);
                dot.name = dotPrefabs[dotIndex].name;
                Dots[x, y] = dot;
                dot.GetComponent<Dot>().Board = this;
            }
        }
    }
}
