using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideTileManager : MonoBehaviour
{
    public GameObject[] leftTilePrefabs;
    public GameObject[] rightTilePrefabs;
    public float tileLength = 15f; // Assuming all tiles have the same length
    public int numberOfTiles = 5;
    public float spawnDistance = 10f; // Distance from player to trigger spawn (adjust based on road width)
    public float sideOffset = 5f; // Distance of tiles from the center of the road

    private List<GameObject> activeLeftTiles = new List<GameObject>();
    private List<GameObject> activeRightTiles = new List<GameObject>();

    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn initial tiles without gaps on both sides
        for (int i = 0; i < numberOfTiles; i++)
        {
            float zPosition = i * tileLength;
            SpawnLeftTile(Random.Range(0, leftTilePrefabs.Length), zPosition);
            SpawnRightTile(Random.Range(0, rightTilePrefabs.Length), zPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the last tile on either side is out of player's view
        float playerZ = playerTransform.position.z;
        if (playerZ - spawnDistance > activeLeftTiles[activeLeftTiles.Count - 1].transform.position.z)
        {
            // Spawn new tiles on both sides, adjusting for no gaps
            float newZ = activeLeftTiles[activeLeftTiles.Count - 1].transform.position.z + tileLength;
            SpawnLeftTile(Random.Range(0, leftTilePrefabs.Length), newZ);
            SpawnRightTile(Random.Range(0, rightTilePrefabs.Length), newZ);

            // Optionally delete the first tiles from both sides if managing a limited pool
            if (activeLeftTiles.Count > numberOfTiles)
            {
                Destroy(activeLeftTiles[0]);
                activeLeftTiles.RemoveAt(0);
                Destroy(activeRightTiles[0]);
                activeRightTiles.RemoveAt(0);
            }
        }
    }

    private void SpawnLeftTile(int tileIndex, float zPosition)
    {
        Vector3 spawnPosition = transform.forward * zPosition + transform.right * -sideOffset;
        GameObject go = Instantiate(leftTilePrefabs[tileIndex], spawnPosition, transform.rotation);
        activeLeftTiles.Add(go);
    }

    private void SpawnRightTile(int tileIndex, float zPosition)
    {
        Vector3 spawnPosition = transform.forward * zPosition + transform.right * sideOffset;
        GameObject go = Instantiate(rightTilePrefabs[tileIndex], spawnPosition, transform.rotation);
        activeRightTiles.Add(go);
    }
}
