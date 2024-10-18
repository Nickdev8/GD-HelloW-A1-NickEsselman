using System.Collections.Generic; // Use for List<>
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject player;
    public GameObject mapPrefab;
    public int gridSize = 10;   // grid size in Unity units
    public int loadRadius = 2;  // how many grids away to load maps around player

    private Vector2 currentMapPos;
    private HashSet<Vector2> activeMaps = new HashSet<Vector2>();  // Track loaded maps efficiently
    static public GameObject globalplayer;

    void Start()
    {
        // Initially load the grid around the player's starting position
        UpdateMapGrid();

        globalplayer = player;
    }

    void Update()
    {
        // Get the current grid position of the player
        currentMapPos = GetPlayerGridPosition();

        // Update map grid based on the player's current position
        UpdateMapGrid();
    }

    // Update the grid, add new maps and remove distant maps
    void UpdateMapGrid()
    {
        // Load maps in the radius around the player
        for (int x = -loadRadius; x <= loadRadius; x++)
        {
            for (int y = -loadRadius; y <= loadRadius; y++)
            {
                Vector2 newMapPos = currentMapPos + new Vector2(x, y);
                if (!activeMaps.Contains(newMapPos))
                {
                    // Instantiate the new map if it is not already active
                    Vector3 newMapWorldPos = GridToWorldPosition(newMapPos);
                    AddMap(mapPrefab, newMapWorldPos);
                    activeMaps.Add(newMapPos);
                }
            }
        }

        // Remove maps that are outside the load radius
        List<Vector2> mapsToRemove = new List<Vector2>();
        foreach (Vector2 mapPos in activeMaps)
        {
            if (Vector2.Distance(mapPos, currentMapPos) > loadRadius)
            {
                // Mark maps for removal that are outside the load radius
                mapsToRemove.Add(mapPos);
            }
        }

        foreach (Vector2 mapPos in mapsToRemove)
        {
            RemoveMapAtPosition(mapPos);
            activeMaps.Remove(mapPos);
        }
    }

    // Convert grid position to world position
    Vector3 GridToWorldPosition(Vector2 gridPos)
    {
        return new Vector3(gridPos.x * gridSize, 0, gridPos.y * gridSize);
    }

    // Get the player's current grid position based on their world position
    Vector2 GetPlayerGridPosition()
    {
        Vector3 playerPos = player.transform.position;
        return new Vector2(Mathf.Floor(playerPos.x / gridSize), Mathf.Floor(playerPos.z / gridSize));
    }

    // Add a new map to the world at a specific position
    void AddMap(GameObject prefab, Vector3 position)
    {
        Instantiate(prefab, position, Quaternion.identity);
    }

    // Remove a map at a specific grid position
    void RemoveMapAtPosition(Vector2 gridPos)
    {
        Vector3 worldPos = GridToWorldPosition(gridPos);
        GameObject mapToRemove = null;

        foreach (GameObject map in GameObject.FindGameObjectsWithTag("map"))
        {
            if (Vector3.Distance(map.transform.position, worldPos) < 0.1f) // Small tolerance
            {
                mapToRemove = map;
                break;
            }
        }

        if (mapToRemove != null)
        {
            Destroy(mapToRemove); // Destroy the map object
        }
    }
}
