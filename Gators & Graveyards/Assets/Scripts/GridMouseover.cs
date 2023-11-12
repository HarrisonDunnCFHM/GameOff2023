using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMouseover : MonoBehaviour
{
    [SerializeField] SpriteRenderer myRenderer;
    [SerializeField] Color defaultColor;
    [SerializeField] Color hoveredColor;
    [SerializeField] Color inRangeColor;
    [SerializeField] Collider myCollider;
    [SerializeField] GameObject pathIndicatorPrefab;
    MovePathIndicator pathIndicator;

    public bool inRange;
    bool hovered;

    GameObject hoveredPoint;

    GridBehavior gridBehavior;
    GridPointStats myGridStats;
    CharacterSelector characterSelector;

    // Start is called before the first frame update
    void Start()
    {
        gridBehavior = FindObjectOfType<GridBehavior>();
        myGridStats = GetComponent<GridPointStats>();
        characterSelector = FindObjectOfType<CharacterSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTileColor();
    }

    void UpdateTileColor()
    {
        if(characterSelector.resolvingMove) { return; }
        if(GetComponent<GridPointStats>().occupied)
        {
            myRenderer.color = defaultColor;
        }
        else if (hovered)
        {
            myRenderer.color = hoveredColor;
        }
        else if (inRange)
        {
            myRenderer.color = inRangeColor;
        }
        else
        {
            myRenderer.color = defaultColor;
        }
    }

    private void OnMouseEnter()
    {
        hovered = true;
        hoveredPoint = gameObject;
        characterSelector.hoveredTile = gameObject;

        if (characterSelector.resolvingMove) { return; }
        if (characterSelector.selectedCharacter)
        {


            if (gridBehavior.GenerateMovePath(characterSelector.selectedCharacter, 
                                characterSelector.selectedCharacter.currentGridPosition.x,
                                characterSelector.selectedCharacter.currentGridPosition.y,
                                myGridStats.x, myGridStats.y,
                                characterSelector.selectedCharacter.GetComponent<ScoutStats>().maxMoveRange))
            {
                GameObject newPath = Instantiate(pathIndicatorPrefab, characterSelector.selectedCharacter.transform.position, Quaternion.identity);
                pathIndicator = newPath.GetComponent<MovePathIndicator>();
                List<GameObject> tempPath = gridBehavior.MovePath;
                pathIndicator.GenerateMovePathIndicator(tempPath);
            }
        }
    }

    private void OnMouseExit()
    {
        hovered = false;
        hoveredPoint = null;

        if(pathIndicator)
        {
            Destroy(pathIndicator.gameObject);
            pathIndicator = null;
        }
    }

    private void OnMouseDown()
    {
        if(hoveredPoint && characterSelector.selectedCharacter && !myGridStats.occupied)
        {

            if (gridBehavior.GenerateMovePath(characterSelector.selectedCharacter, 
                                characterSelector.selectedCharacter.currentGridPosition.x,
                                characterSelector.selectedCharacter.currentGridPosition.y,
                                myGridStats.x, myGridStats.y,
                                characterSelector.selectedCharacter.GetComponent<ScoutStats>().maxMoveRange))
            {
                if (pathIndicator)
                {
                    Destroy(pathIndicator.gameObject);
                    pathIndicator = null;
                }
                StartCoroutine(characterSelector.selectedCharacter.MoveCharacterCoroutine(myGridStats.x, myGridStats.y));
            }
        }
    }


}
