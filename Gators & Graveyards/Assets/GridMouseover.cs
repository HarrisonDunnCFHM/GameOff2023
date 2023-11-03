using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMouseover : MonoBehaviour
{
    [SerializeField] SpriteRenderer myRenderer;
    [SerializeField] Color defaultColor;
    [SerializeField] Color hoveredColor;
    [SerializeField] Collider myCollider;

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
       
    }

    private void OnMouseEnter()
    {
        myRenderer.color = hoveredColor;
        hoveredPoint = gameObject;
    }

    private void OnMouseExit()
    {
        myRenderer.color = defaultColor;
        hoveredPoint = null;
    }

    private void OnMouseDown()
    {
        if(hoveredPoint && characterSelector.selectedCharacter)
        {
            
            gridBehavior.FindPath(characterSelector.selectedCharacter.currentGridPosition.x,
                                characterSelector.selectedCharacter.currentGridPosition.y, 
                                myGridStats.x, myGridStats.y);
            StartCoroutine(characterSelector.selectedCharacter.MoveCharacter(myGridStats.x, myGridStats.y));
        }
    }


}
