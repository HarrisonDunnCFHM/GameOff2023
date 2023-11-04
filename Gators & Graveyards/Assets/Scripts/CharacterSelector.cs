using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{

    public Color defaultColor;
    public Color selectedColor;

    CharacterMover[] activeCharacters;

    public CharacterMover selectedCharacter;
    public float moveTime = 0.2f;

    GridBehavior gridBehavior;
    public GameObject hoveredTile;
    
    // Start is called before the first frame update
    void Start()
    {
        activeCharacters = FindObjectsOfType<CharacterMover>();
        gridBehavior = FindObjectOfType<GridBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCharacterSelectedIndicator();
        //UpdateMovePathIndicator();
    }

    private void UpdateCharacterSelectedIndicator()
    {
        foreach (CharacterMover character in activeCharacters)
        {
            if (character == selectedCharacter)
            {
                character.mySprite.color = selectedColor;
            }
            else
            {
                character.mySprite.color = defaultColor;
            }
        }
    }

    //private void UpdateMovePathIndicator()
    //{
    //   if (selectedCharacter && hoveredTile.GetComponent<GridStats>().visited > -1)
    //   {
    //        Debug.Log("Tile in range!");
    //        if(pathIndicator == null)
    //        {
    //            GameObject newPath = Instantiate(pathIndicatorPrefab, selectedCharacter.transform.position, Quaternion.identity);
    //            pathIndicator = newPath.GetComponent<MovePathIndicator>();
    //        }
    //        else
    //        {
    //            pathIndicator.DisplayMovePathIndicator(gridBehavior,
    //                selectedCharacter.currentGridPosition.x,
    //                selectedCharacter.currentGridPosition.y,
    //                hoveredTile.GetComponent<GridStats>().x,
    //                hoveredTile.GetComponent<GridStats>().y);
    //        }
    //   }
    //}

    public void SelectCharacter(CharacterMover clickedCharacter)
    {
        if(clickedCharacter)
        {
            selectedCharacter = clickedCharacter;
            UpdateMovesInRange();
        }
        else
            selectedCharacter = null;
    }

   void UpdateMovesInRange()
    {
        gridBehavior.FindPath(selectedCharacter.currentGridPosition.x,
                                selectedCharacter.currentGridPosition.y,
                                selectedCharacter.currentGridPosition.x,
                                selectedCharacter.currentGridPosition.y,
                                selectedCharacter.GetComponent<ScoutStats>().maxMoveRange);

    }

}
