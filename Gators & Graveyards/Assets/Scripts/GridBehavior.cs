using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    [SerializeField] int gridRows = 10;
    [SerializeField] int gridColumns = 10;
    [SerializeField] int gridScale = 1;
    [SerializeField] GameObject gridPointPrefab;
    [SerializeField] Vector3 bottomLeftLocation = new Vector3(0, 0, 0);

    public GameObject[,] gridPointArray;
    
    int _moveStartX = 0;
    int _moveStartY = 0;
    int _moveEndX = 2;
    int _moveEndY = 2;

    public GameObject MovingCharacter;

    public List<GameObject> MovePath = new List<GameObject>();

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

    public bool GenerateMovePath(int startX, int startY, int endX, int endY, int moveRange)
    {
        _moveStartX = startX;
        _moveStartY = startY;
        _moveEndX = endX;
        _moveEndY = endY;
        SetDistance(moveRange);
        if (SetPath())
            return true; 
        else return false ;
    }


    void SetDistance(int moveRange)
    {
        InitialGridSetup();
        int x = _moveStartX;
        int y = _moveStartY;
        int[] testArray = new int[gridRows * gridColumns];
        for (int step = 1; step <= moveRange; step++)
        {
            foreach(GameObject point in gridPointArray)
            {
                if(point && point.GetComponent<GridPointStats>().visited == step - 1)
                {
                    TestFourDirections(point.GetComponent<GridPointStats>().x, point.GetComponent<GridPointStats>().y, step);
                }
            }
        }
    }

    bool SetPath()
    {
        int step;
        int x = _moveEndX;
        int y = _moveEndY;
        List<GameObject> tempList = new List<GameObject>();
        MovePath.Clear();
        if(gridPointArray[_moveEndX,_moveEndY] && gridPointArray[_moveEndX,_moveEndY].GetComponent<GridPointStats>().visited > 0)
        {
            MovePath.Add(gridPointArray[x, y]);
            step = gridPointArray[x, y].GetComponent<GridPointStats>().visited - 1;
        }
        else
        {
            Debug.Log("can't reach the desired location!");
            return false;
        }
        for (int i = step; step > -1; step--)
        {
            if (TestDirection(x, y, step, 1))
                tempList.Add(gridPointArray[x, y + 1]);
            if (TestDirection(x, y, step, 2))
                tempList.Add(gridPointArray[x + 1, y]);
            if (TestDirection(x, y, step, 3))
                tempList.Add(gridPointArray[x, y - 1]);
            if (TestDirection(x, y, step, 4))
                tempList.Add(gridPointArray[x - 1 ,y]); 
            GameObject tempObj = FindClosestPoint(gridPointArray[_moveEndX, _moveEndY].transform, tempList);
            MovePath.Add(tempObj);
            x = tempObj.GetComponent<GridPointStats>().x;
            y = tempObj.GetComponent<GridPointStats>().y;
            tempList.Clear();
        }
        return true;
    }

    public void InitialGridSetup()
    {
        foreach(GameObject point in gridPointArray)
        {
            if (point)
            {
                point.GetComponent<GridPointStats>().visited = -1;
                point.GetComponent<GridMouseover>().inRange = false;
            }
        }
        //occupied spaces need to be set to -2 here
        CharacterMover[] allCharacters = FindObjectsOfType<CharacterMover>();
        if (MovingCharacter == null)
        {
            MovingCharacter = FindObjectOfType<CharacterSelector>().selectedCharacter.gameObject;
        }
        foreach(CharacterMover character in allCharacters)
        {
            if (MovingCharacter && MovingCharacter.GetComponent<ScoutStats>() && character.GetComponent<GhostStats>())
            {
                gridPointArray[character.currentGridPosition.x, character.currentGridPosition.y].GetComponent<GridPointStats>().visited = -2;
            }
            //else if (movingCharacter && movingCharacter.GetComponent<GhostStats>() && character.GetComponent<ScoutStats>())
            //{
            //    gridArray[character.currentGridPosition.x, character.currentGridPosition.y].GetComponent<GridStats>().visited = -2;
            //}
        }
        //the movers starting space is set to 0
        gridPointArray[_moveStartX, _moveStartY].GetComponent<GridPointStats>().visited = 0;

    }

    bool TestDirection(int x, int y, int step, int direction)
    {
        // int directions 1 is up, 2 is right, 3 is down, 4 is left
        switch(direction)
        {
            case 4:
                if (x - 1 > -1 && gridPointArray[x - 1, y] && gridPointArray[x - 1, y].GetComponent<GridPointStats>().visited == step)
                {
                    return true;
                }
                else return false;
            case 3:
                if (y - 1 > -1 && gridPointArray[x, y - 1] && gridPointArray[x, y - 1].GetComponent<GridPointStats>().visited == step)
                {
                    return true;
                }
                else return false;
            case 2:
                if (x + 1 < gridColumns && gridPointArray[x + 1, y] && gridPointArray[x + 1, y].GetComponent<GridPointStats>().visited == step)
                {
                    return true;
                }
                else return false;
            case 1:
                if (y + 1 < gridRows && gridPointArray[x, y + 1] && gridPointArray[x, y + 1].GetComponent<GridPointStats>().visited == step)
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
        if (gridPointArray[x,y])
        {
            gridPointArray[x, y].GetComponent<GridPointStats>().visited = step;
            gridPointArray[x, y].GetComponent<GridMouseover>().inRange = true;
        }
    }

    GameObject FindClosestPoint(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = gridScale * gridRows * gridColumns;
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
