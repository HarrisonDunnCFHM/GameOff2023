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
    
    // Start is called before the first frame update
    void Start()
    {
        activeCharacters = FindObjectsOfType<CharacterMover>();
        gridBehavior = FindObjectOfType<GridBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(CharacterMover character in activeCharacters)
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
