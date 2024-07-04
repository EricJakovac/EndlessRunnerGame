using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject Tile1;
    public GameObject Tile2;
    public GameObject Tile3;
    public GameObject Tile4;
    public GameObject Tile5;
    public GameObject StartTile;

    private float nextTilePositionZ = 0f;
    private float startTileLength = 40f;
    private float tileLength = 40f;
    private List<GameObject> activeTiles = new List<GameObject>();

    public Transform playerTransform;
    private Vector3 initialPlayerPosition;
    private Vector3 initialStartTilePosition;

    public float tileSpeed = 10f;
    public float speedIncreaseRate = 0.2f;
    private float timeElapsed = 0f;

    private bool gameStopped = false;

    private void Start()
    {
        initialPlayerPosition = playerTransform.position;
        initialStartTilePosition = new Vector3(0, 0, initialPlayerPosition.z + startTileLength);

        GameObject startTile = Instantiate(StartTile, transform);
        startTile.transform.position = initialStartTilePosition;
        activeTiles.Add(startTile);

        nextTilePositionZ += startTileLength;
    }

    private void Update()
    {
        if (!gameStopped)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= 1f)
            {
                tileSpeed += speedIncreaseRate;
                timeElapsed = 0f;
            }

            while (playerTransform.position.z >= nextTilePositionZ - (startTileLength + tileLength))
            {
                GenerateTile();
            }

            MoveTiles();

            if (CheckResetConditions())
            {
                StopGame();
            }
        }
    }

    private void GenerateTile()
    {
        int randomInt = Random.Range(0, 5);
        GameObject newTile;

        if (randomInt == 0)
        {
            newTile = Instantiate(Tile1, transform);
        }
        else if (randomInt == 1)
        {
            newTile = Instantiate(Tile2, transform);
        }
        else if (randomInt == 2)
        {
            newTile = Instantiate(Tile3, transform);
        }
        else if (randomInt == 3)
        {
            newTile = Instantiate(Tile4, transform);
        }
        else
        {
            newTile = Instantiate(Tile5, transform);
        }

        float newPositionZ = nextTilePositionZ + tileLength / 10;
        if (activeTiles.Count > 0)
        {
            newPositionZ = activeTiles[activeTiles.Count - 1].transform.position.z + tileLength;
        }

        newTile.transform.position = new Vector3(0, 0, newPositionZ);
        activeTiles.Add(newTile);

        nextTilePositionZ += tileLength;
    }

    private void MoveTiles()
    {
        float currentSpeed = tileSpeed * Time.deltaTime;

        for (int i = 0; i < activeTiles.Count; i++)
        {
            activeTiles[i].transform.position += new Vector3(0, 0, -currentSpeed);

            if (activeTiles[i].transform.position.z < playerTransform.position.z - 30f)
            {
                Destroy(activeTiles[i]);
                activeTiles.RemoveAt(i);
                i--;

                GenerateTile();
            }
        }
    }

    private bool CheckResetConditions()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (var obstacle in obstacles)
        {
            Collider playerCollider = playerTransform.GetComponent<Collider>();
            Collider obstacleCollider = obstacle.GetComponent<Collider>();

            if (playerCollider != null && obstacleCollider != null &&
                playerCollider.bounds.Intersects(obstacleCollider.bounds))
            {
                return true;
            }
        }

        return false;
    }

    private void StopGame()
    {
        gameStopped = true;
    }
}
