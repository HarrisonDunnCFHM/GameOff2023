using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    [SerializeField] bool findDistance = false;
    [SerializeField] int rows = 10;
    [SerializeField] int columns = 10;
    [SerializeField] int scale = 1;
    [SerializeField] GameObject gridPointPrefab;
    [SerializeField] Vector3 bottomLeftLocation = new Vector3(0, 0, 0);

    public GameObject[,] gridArray;
    [SerializeField] int startX = 0;
    [SerializeField] int startY = 0;
    [SerializeField] int endX = 2;
    [SerializeField] int endY = 2;

    public List<GameObject> path = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        gridArray = new GameObject[columns, rows];
        if (gridPointPrefab)
        {
            GenerateGrid();
        }
        else { Debug.Log("No grid prefab!"); }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public bool FindPath(int x, int y, int destX, int destY, int moveRange)
    {
        startX = x;
        startY = y;
        endX = destX;
        endY = destY;
        SetDistance(moveRange);
        if (SetPath())
            return true; 
        else return false ;
    }

    void GenerateGrid()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 newGridPointPos = new Vector3(bottomLeftLocation.x + scale * x, bottomLeftLocation.y, bottomLeftLocation.z + scale * y);
                GameObject newGridPoint = Instantiate(gridPointPrefab, newGridPointPos, Quaternion.identity);
                newGridPoint.transform.SetParent(gameObject.transform);
                newGridPoint.GetComponent<GridStats>().x = x;
                newGridPoint.GetComponent<GridStats>().y = y;
                newGridPoint.GetComponent<GridStats>().myCoords.text = x + " , " + y;
                newGridPoint.name = newGridPoint.GetComponent<GridStats>().myCoords.text;
                gridArray[x, y] = newGridPoint;
            }
        }
    }

    void SetDistance(int moveRange)
    {
        InitialGridSetup();
        int x = startX;
        int y = startY;
        int[] testArray = new int[rows * columns];
        for (int step = 1; step <= moveRange; step++)
        {
            foreach(GameObject point in gridArray)
            {
                if(point && point.GetComponent<GridStats>().visited == step - 1)
                {
                    TestFourDirections(point.GetComponent<GridStats>().x, point.GetComponent<GridStats>().y, step);
                }
            }
        }
    }

    bool SetPath()
    {
        int step;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject>();
        path.Clear();
        if(gridArray[endX,endY] && gridArray[endX,endY].GetComponent<GridStats>().visited > 0)
        {
            path.Add(gridArray[x, y]);
            step = gridArray[x, y].GetComponent<GridStats>().visited - 1;
        }
        else
        {
            Debug.Log("can't reach the desired location!");
            return false;
        }
        for (int i = step; step > -1; step--)
        {
            if (TestDirection(x, y, step, 1))
                tempList.Add(gridArray[x, y + 1]);
            if (TestDirection(x, y, step, 2))
                tempList.Add(gridArray[x + 1, y]);
            if (TestDirection(x, y, step, 3))
                tempList.Add(gridArray[x, y - 1]);
            if (TestDirection(x, y, step, 4))
                tempList.Add(gridArray[x - 1 ,y]); 
            GameObject tempObj = FindClosestPoint(gridArray[endX, endY].transform, tempList);
            path.Add(tempObj);
            x = tempObj.GetComponent<GridStats>().x;
            y = tempObj.GetComponent<GridStats>().y;
            tempList.Clear();
        }
        return true;
    }

    public void InitialGridSetup()
    {
        foreach(GameObject point in gridArray)
        {
            point.GetComponent<GridStats>().visited = -1;
            point.GetComponent<GridMouseover>().inRange = false;
        }
        gridArray[startX, startY].GetComponent<GridStats>().visited = 0;
        //occupied spaces need to be set to 0 here
    }

    bool TestDirection(int x, int y, int step, int direction)
    {
        // int directions 1 is up, 2 is right, 3 is down, 4 is left
        switch(direction)
        {
            case 4:
                if (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else return false;
            case 3:
                if (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else return false;
            case 2:
                if (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else return false;
            case 1:
                if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else return false;
            default:
                break;
        }
        return false;
    }

    void TestFourDirections(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, 1))
            SetVisitedPoint(x, y + 1, step);
        if (TestDirection(x, y, -1, 2))
            SetVisitedPoint(x + 1, y, step);
        if (TestDirection(x, y, -1, 3))
            SetVisitedPoint(x, y - 1, step);
        if (TestDirection(x, y, -1, 4))
            SetVisitedPoint(x - 1, y, step);
    }

    void SetVisitedPoint(int x, int y, int step)
    {
        if (gridArray[x,y])
        {
            gridArray[x, y].GetComponent<GridStats>().visited = step;
            gridArray[x, y].GetComponent<GridMouseover>().inRange = true;
        }
    }

    GameObject FindClosestPoint(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = scale * rows * columns;
        int indexNumber = 0;
        for(int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance) 
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNumber = i;
            }
        }
        return list[indexNumber];
    }
}
