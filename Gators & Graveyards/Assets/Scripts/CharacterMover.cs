using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    [SerializeField] Vector2Int startingGridPosition;
    public Vector2Int currentGridPosition;
    public SpriteRenderer mySprite;
    public float moveTime = 0.8f;
    
    GameObject[,] gridArray;
    GridBehavior gridBehavior;

    CharacterSelector charactorSelector;

    
    // Start is called before the first frame update
    void Start()
    {
        gridBehavior = FindObjectOfType<GridBehavior>();
        gridArray = gridBehavior.gridArray;
        charactorSelector = FindObjectOfType<CharacterSelector>();
        transform.position = gridArray[startingGridPosition.x, startingGridPosition.y].transform.position;
        currentGridPosition = startingGridPosition;
        gridArray[startingGridPosition.x, startingGridPosition.y].GetComponent<GridStats>().occupied = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (GetComponent<ScoutStats>().actionsLeft <= 0) return;
        charactorSelector.SelectCharacter(this);

    }

    public IEnumerator MoveCharacter(int x, int y)
    {
        charactorSelector.resolvingMove = true;
        moveTime = charactorSelector.moveTime;
        gridBehavior.gridArray[gridBehavior.path[gridBehavior.path.Count - 1].GetComponent<GridStats>().x, gridBehavior.path[gridBehavior.path.Count - 1].GetComponent<GridStats>().y].GetComponent<GridStats>().occupied = false;
        if(GetComponent<GhostStats>())
        {
            int moveRange;
            //moveRange = GetComponent<GhostStats>().maxMoveRange;
            gridBehavior.FindPath(currentGridPosition.x, currentGridPosition.y, x, y, 99);
        }
        List<GameObject> tempPath = new List<GameObject>(gridBehavior.path);
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
            currentGridPosition = new Vector2Int(tempPath[step - 1].GetComponent<GridStats>().x, tempPath[step - 1].GetComponent<GridStats>().y);
        }
        if(GetComponent<GhostStats>())
        {
            gridBehavior.movingCharacter = gameObject;
        }
        else
        {
            gridBehavior.movingCharacter = charactorSelector.selectedCharacter.gameObject;
        }
        gridBehavior.InitialGridSetup();
        if(GetComponent<ScoutStats>())
        {
            GetComponent<ScoutStats>().actionsLeft--;
        }
        gridBehavior.gridArray[x, y].GetComponent<GridStats>().occupied = true;
        charactorSelector.selectedCharacter = null;
        charactorSelector.resolvingMove = false;
        yield return null;
    }

}
