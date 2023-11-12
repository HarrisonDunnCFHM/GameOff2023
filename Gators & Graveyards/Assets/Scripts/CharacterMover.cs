using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    [SerializeField] Vector2Int startingGridPosition;
    public Vector2Int currentGridPosition;
    public SpriteRenderer mySprite;
    public float moveTime = 0.8f;

    GridSetup grid;
    GameObject[,] gridPointArray;
    int _moveStartX = 0;
    int _moveStartY = 0;
    int _moveEndX = 2;
    int _moveEndY = 2;

    public List<GameObject> MovePath = new List<GameObject>();


    CharacterSelector charactorSelector;

    
    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridSetup>();
        gridPointArray = grid.gridPointArray;
        charactorSelector = FindObjectOfType<CharacterSelector>();
        transform.position = gridPointArray[startingGridPosition.x, startingGridPosition.y].transform.position;
        currentGridPosition = startingGridPosition;
        gridPointArray[startingGridPosition.x, startingGridPosition.y].GetComponent<GridPointStats>().occupied = true;
    }

    private void OnMouseDown()
    {
        if (GetComponent<ScoutStats>().actionsLeft <= 0 || GetComponent<GhostStats>()) return;
        charactorSelector.SelectCharacter(this);

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
        else return false;
    }

    private void SetDistance(int moveRange)
    {
        InitialGridSetup();
        for (int step = 1; step <= moveRange; step++)
        {
            foreach (GameObject point in gridPointArray)
            {
                if (point && point.GetComponent<GridPointStats>().visited == step - 1)
                {
                    TestFourDirections(point.GetComponent<GridPointStats>().x, point.GetComponent<GridPointStats>().y, step);
                }
            }
        }
    }

    public void InitialGridSetup()
    {
        foreach (GameObject point in gridPointArray)
        {
            if (point)
            {
                point.GetComponent<GridPointStats>().visited = -1;
                point.GetComponent<GridMouseover>().inRange = false;
            }
        }
        //occupied spaces need to be set to -2 here
        CharacterMover[] allCharacters = FindObjectsOfType<CharacterMover>();
        foreach (CharacterMover character in allCharacters)
        {
            if (GetComponent<ScoutStats>() && character.GetComponent<GhostStats>())
            {
                Debug.Log(character.name + " is at point " + character.currentGridPosition.x + "," + character.currentGridPosition.y);
                gridPointArray[character.currentGridPosition.x, character.currentGridPosition.y].GetComponent<GridPointStats>().visited = -2;
            }
        }
        //the movers starting space is set to 0
        gridPointArray[_moveStartX, _moveStartY].GetComponent<GridPointStats>().visited = 0;

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
    bool TestDirection(int x, int y, int step, int direction)
    {
        // int directions 1 is up, 2 is right, 3 is down, 4 is left
        switch (direction)
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
                if (x + 1 < grid.gridColumns && gridPointArray[x + 1, y] && gridPointArray[x + 1, y].GetComponent<GridPointStats>().visited == step)
                {
                    return true;
                }
                else return false;
            case 1:
                if (y + 1 < grid.gridRows && gridPointArray[x, y + 1] && gridPointArray[x, y + 1].GetComponent<GridPointStats>().visited == step)
                {
                    return true;
                }
                else return false;
            default:
                break;
        }
        return false;
    }

    void SetVisitedPoint(int x, int y, int step)
    {
        if (gridPointArray[x, y])
        {
            gridPointArray[x, y].GetComponent<GridPointStats>().visited = step;
            gridPointArray[x, y].GetComponent<GridMouseover>().inRange = true;
        }
    }

    bool SetPath()
    {
        int step;
        int x = _moveEndX;
        int y = _moveEndY;
        List<GameObject> tempList = new List<GameObject>();
        MovePath.Clear();
        if (gridPointArray[_moveEndX, _moveEndY] && gridPointArray[_moveEndX, _moveEndY].GetComponent<GridPointStats>().visited > 0)
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
                tempList.Add(gridPointArray[x - 1, y]);
            GameObject tempObj = FindClosestPoint(gridPointArray[_moveEndX, _moveEndY].transform, tempList);
            MovePath.Add(tempObj);
            x = tempObj.GetComponent<GridPointStats>().x;
            y = tempObj.GetComponent<GridPointStats>().y;
            tempList.Clear();
        }
        return true;
    }

    GameObject FindClosestPoint(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = grid.gridScale * grid.gridRows * grid.gridColumns;
        int indexNumber = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNumber = i;
            }
        }
        return list[indexNumber];
    }

    public IEnumerator MoveCharacterCoroutine(int x, int y)
    {
        charactorSelector.resolvingMove = true;
        moveTime = charactorSelector.moveTime;
        if(GetComponent<GhostStats>())
        {
            int moveRange;
            //moveRange = GetComponent<GhostStats>().maxMoveRange;
            GenerateMovePath(currentGridPosition.x, currentGridPosition.y, x, y, 99);
        }
        gridPointArray[MovePath[MovePath.Count - 1].GetComponent<GridPointStats>().x, MovePath[MovePath.Count - 1].GetComponent<GridPointStats>().y].GetComponent<GridPointStats>().occupied = false;
        List<GameObject> tempPath = new List<GameObject>(MovePath);
        for (int step = tempPath.Count - 1; step > 0; step--)
        {
            float elapsed = 0f;
            while (elapsed < moveTime)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(tempPath[step].transform.position, tempPath[step-1].transform.position, elapsed/ moveTime);
                yield return null;
            }
            transform.position = tempPath[step - 1].transform.position;
            currentGridPosition = new Vector2Int(tempPath[step - 1].GetComponent<GridPointStats>().x, tempPath[step - 1].GetComponent<GridPointStats>().y);
        }
        
        InitialGridSetup();
        if(GetComponent<ScoutStats>())
        {
            GetComponent<ScoutStats>().actionsLeft--;
        }
        gridPointArray[x, y].GetComponent<GridPointStats>().occupied = true;
        charactorSelector.selectedCharacter = null;
        charactorSelector.resolvingMove = false;
        yield return null;
    }

}
