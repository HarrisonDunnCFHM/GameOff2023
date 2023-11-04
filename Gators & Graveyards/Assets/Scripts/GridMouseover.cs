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

    public bool inRange;
    bool hovered;

    GameObject hoveredPoint;

    GridBehavior gridBehavior;
    GridStats myGridStats;
    CharacterSelector characterSelector;

    // Start is called before the first frame update
    void Start()
    {
        gridBehavior = FindObjectOfType<GridBehavior>();
        myGridStats = GetComponent<GridStats>();
        characterSelector = FindObjectOfType<CharacterSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTileColor();
    }

    void UpdateTileColor()
    {
        if (hovered)
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
    }

    private void OnMouseExit()
    {
        hovered = false;
        hoveredPoint = null;
    }

    private void OnMouseDown()
    {
        if(hoveredPoint && characterSelector.selectedCharacter)
        {

            if (gridBehavior.FindPath(characterSelector.selectedCharacter.currentGridPosition.x,
                                characterSelector.selectedCharacter.currentGridPosition.y,
                                myGridStats.x, myGridStats.y,
                                characterSelector.selectedCharacter.GetComponent<ScoutStats>().maxMoveRange))
            {
                StartCoroutine(characterSelector.selectedCharacter.MoveCharacter(myGridStats.x, myGridStats.y));
            }
        }
    }


}
