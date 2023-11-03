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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        charactorSelector.SelectCharacter(this);

    }

    public IEnumerator MoveCharacter(int x, int y)
    {
        moveTime = charactorSelector.moveTime;
        for (int step = gridBehavior.path.Count - 1; step > 0; step--)
        {
            float elapsed = 0f;
            while (elapsed < moveTime)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(gridBehavior.path[step].transform.position, gridBehavior.path[step-1].transform.position, elapsed/ moveTime);
                yield return null;
            }
            transform.position = gridBehavior.path[step - 1].transform.position;
            currentGridPosition = new Vector2Int(gridBehavior.path[step - 1].GetComponent<GridStats>().x, gridBehavior.path[step - 1].GetComponent<GridStats>().y);
        }
        yield return null;
    }
}
