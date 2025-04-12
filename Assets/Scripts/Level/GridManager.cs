using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Vault;

public class GridManager : MonoBehaviour
{
    public GameObject cylinderPrefab;
    public float cellSize = 1.0f;

    public float paddingX = 0.5f;
    public float paddingZ = 0.5f;

    public int rows = 3;
    public int columns = 3;

    private bool[,] activeCells;
    [SerializeField] private BoxCollider boxCollider;

    private string loadFilePath = "Assets/Resources/gridData.json";

    public int index;
    private void Start()
    {
        LoadGridData();
        GenerateGridInBounds();
        EventManager.Instance.TriggerEvent(new InitHireTimersEvent());

    }

    void GenerateGridInBounds()
    {

        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider is not assigned!");
            return;
        }

        Bounds bounds = boxCollider.bounds;

        Vector3 usableSpace = bounds.size - new Vector3(2 * paddingX, 0, 2 * paddingZ);

        int cellsX = Mathf.FloorToInt(usableSpace.x / cellSize);
        int cellsZ = Mathf.FloorToInt(usableSpace.z / cellSize);

        rows = Mathf.Min(rows, cellsZ);
        columns = Mathf.Min(columns, cellsX);

        Vector3 startPosition = bounds.center - new Vector3(usableSpace.x / 2, 0, usableSpace.z / 2);

        Vector3 offset = new Vector3(cellSize / 2, 0, cellSize / 2);

        for (int z = 0; z < rows; z++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (activeCells[z, x])
                {

                    Vector3 cellPosition = startPosition + new Vector3(x * cellSize, 0, z * cellSize) + offset;
                    GameObject obj = Instantiate(cylinderPrefab, cellPosition, Quaternion.identity);
                    BaseHandler handler = obj.GetComponent<BaseHandler>();
                    handler.Id = index;
                    index++;
                    EventManager.Instance.TriggerEvent(new AddSpawnedBasesEvent(handler));
                        
                }
            }
        }
    }

    /*  private void OnDrawGizmos()
      {
          if (boxCollider == null)
              return;

          // Check if activeCells is initialized
          if (activeCells == null)
              return;

          Bounds bounds = boxCollider.bounds;

          // Calculate usable space considering padding on the x and z axes
          Vector3 usableSpace = bounds.size - new Vector3(2 * paddingX, 0, 2 * paddingZ);
          int cellsX = Mathf.FloorToInt(usableSpace.x / cellSize);
          int cellsZ = Mathf.FloorToInt(usableSpace.z / cellSize);

          // Determine the starting position of the grid
          Vector3 startPosition = bounds.center - new Vector3(usableSpace.x / 2, 0, usableSpace.z / 2);

          // Draw the wireframe gizmos for the grid
          Gizmos.color = Color.green;
          for (int x = 0; x < cellsX; x++)
          {
              for (int z = 0; z < cellsZ; z++)
              {
                  Vector3 cellPosition = startPosition + new Vector3(x * cellSize, 0, z * cellSize);
                  Gizmos.DrawWireCube(cellPosition + Vector3.one * (cellSize / 2), Vector3.one * cellSize);
              }
          }

          // Draw the active cells in the gizmos
          for (int z = 0; z < rows; z++)
          {
              for (int x = 0; x < columns; x++)
              {
                  if (z < activeCells.GetLength(0) && x < activeCells.GetLength(1))
                  {
                      Vector3 position = startPosition + new Vector3(x * cellSize, 0, z * cellSize);
                      Gizmos.color = activeCells[z, x] ? Color.white : Color.black; // White for active, black for inactive
                      Gizmos.DrawCube(position + Vector3.one * (cellSize / 2), Vector3.one * (cellSize * 0.9f));
                  }
              }
          }
      }*/


    private void LoadGridData()
    {
        // Load JSON file from Resources folder
        TextAsset jsonFile = Resources.Load<TextAsset>("gridData"); // Omit file extension

        if (jsonFile != null)
        {
            string json = jsonFile.text;
            GridData gridData = JsonUtility.FromJson<GridData>(json);

            rows = gridData.rows;
            columns = gridData.columns;

            activeCells = new bool[rows, columns];

            for (int z = 0; z < rows; z++)
            {
                for (int x = 0; x < columns; x++)
                {
                    activeCells[z, x] = gridData.grid[z * columns + x];
                }
            }

            Debug.Log("Grid data loaded from Resources!");
        }
        else
        {
            Debug.LogError("Grid data file not found in Resources.");
        }


    }



}
