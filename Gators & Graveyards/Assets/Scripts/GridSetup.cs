using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSetup : MonoBehaviour
{
    public int gridRows = 10;
    public int gridColumns = 10;
    public int gridScale = 1;
    [SerializeField] GameObject gridPointPrefab;
    [SerializeField] Vector3 bottomLeftLocation = new Vector3(0, 0, 0);

    public GameObject[,] gridPointArray;
     
    // Start is called before the first frame update
    void Awake()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        if (gridPointPrefab == null)
        { 
            Debug.Log("No grid prefab!");
            return;
        }
        gridPointArray = new GameObject[gridColumns, gridRows];

        for (int gridColumn = 0; gridColumn < gridColumns; gridColumn++)
        {
            for (int gridRow = 0; gridRow < gridRows; gridRow++)
            {
                var newGridX = bottomLeftLocation.x + gridScale * gridColumn;
                var newGridY = bottomLeftLocation.y;
                var newGridZ = bottomLeftLocation.z + gridScale * gridRow;
                Vector3 newGridPointPos = new(newGridX, newGridY, newGridZ);

                GameObject newGridPoint = Instantiate(gridPointPrefab, newGridPointPos, Quaternion.identity);
                newGridPoint.transform.SetParent(gameObject.transform);
                newGridPoint.name = gridColumn + " , " + gridRow;

                GridPointStats newGridPointStats = newGridPoint.GetComponent<GridPointStats>();
                newGridPointStats.x = gridColumn;
                newGridPointStats.y = gridRow;
                newGridPointStats.myCoords.text = newGridPoint.name;

                gridPointArray[gridColumn, gridRow] = newGridPoint;
            }
        }
    }
}
