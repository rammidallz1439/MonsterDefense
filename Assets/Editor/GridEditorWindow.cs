using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class GridEditorWindow : EditorWindow
{
    public int rows = 10;    // Number of rows for the grid
    public int columns = 10; // Number of columns for the grid
    public float cellSize = 1.0f; // Size of the cells

    private bool[,] grid;  // True if a cylinder should be spawned in the cell

    // JSON file path to save/load data
    private string saveFilePath = "Assets/Resources/gridData.json";

    // Open the window from the Unity menu
    [MenuItem("Window/Grid Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridEditorWindow>("Grid Editor");
    }

    private void OnEnable()
    {
        // Initialize the grid if not already done
        InitializeGrid();
    }

    // Initialize the grid based on rows and columns
    private void InitializeGrid()
    {
        grid = new bool[rows, columns];
    }

    private void OnGUI()
    {
        GUILayout.Label("Grid Layout", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // Grid size controls
        rows = EditorGUILayout.IntField("Rows", rows);
        columns = EditorGUILayout.IntField("Columns", columns);
        cellSize = EditorGUILayout.FloatField("Cell Size", cellSize);

        // Display the grid in the editor window
        for (int x = 0; x < rows; x++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int y = 0; y < columns; y++)
            {
                // Toggle to mark whether a cylinder should be spawned in this cell
                grid[x, y] = EditorGUILayout.Toggle(grid[x, y], GUILayout.Width(40), GUILayout.Height(40));
            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(10);

        // Save and Load buttons
        if (GUILayout.Button("Save Grid to JSON"))
        {
            SaveGridData();
        }

        if (GUILayout.Button("Load Grid from JSON"))
        {
            LoadGridData();
        }
    }

    // Save the grid layout to JSON
    private void SaveGridData()
    {
        // Flatten the bool[,] grid to a List<bool> for serialization
        List<bool> flattenedGrid = new List<bool>();
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                flattenedGrid.Add(grid[x, y]);
            }
        }

        // Create and fill the grid data object
        GridData gridData = new GridData
        {
            rows = rows,
            columns = columns,
            grid = flattenedGrid
        };

        // Serialize to JSON and save
        string json = JsonUtility.ToJson(gridData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Grid data saved to " + saveFilePath);
    }

    // Load the grid layout from JSON
    private void LoadGridData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GridData gridData = JsonUtility.FromJson<GridData>(json);

            // Apply loaded data
            rows = gridData.rows;
            columns = gridData.columns;

            // Reinitialize the grid
            grid = new bool[rows, columns];

            // Convert the flat List<bool> back to bool[,]
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    grid[x, y] = gridData.grid[x * columns + y];
                }
            }

            Debug.Log("Grid data loaded from " + saveFilePath);
        }
        else
        {
            Debug.LogError("Grid data file not found at " + saveFilePath);
        }
    }

   
}
