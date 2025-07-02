using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Maze mazePrefab;
    private Maze mazeInstance;

    public Player playerPrefab;
    private Player playerInstance;

    private void Start()
    {
        StartCoroutine(BeginGame());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    private IEnumerator BeginGame()
    {
        // Generate maze
        mazeInstance = Instantiate(mazePrefab);
        yield return StartCoroutine(mazeInstance.Generate());

        // Find center cell
        int centerX = mazeInstance.size.x / 2;
        int centerZ = mazeInstance.size.z / 2;
        IntVector2 centerCoordinates = new IntVector2(centerX, centerZ);

        MazeCell centerCell = mazeInstance.GetCell(centerCoordinates);

        if (centerCell == null)
        {
            Debug.LogError("Center cell is null!");
            yield break;
        }

        // Instantiate player at center
        playerInstance = Instantiate(playerPrefab, centerCell.transform.position, Quaternion.identity);
    } 

    private void RestartGame()
    {
        StopAllCoroutines();

        if (mazeInstance != null)
        {
            Destroy(mazeInstance.gameObject);
        }

        if (playerInstance != null)
        {
            Destroy(playerInstance.gameObject);
        }

        StartCoroutine(BeginGame());
    }
}
