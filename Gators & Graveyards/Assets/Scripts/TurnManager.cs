using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public ScoutStats[] allScouts;
    public GhostStats[] allGhosts;
    public CharacterMover[] allCharacters;

    [SerializeField] Button nextRoundButton;
    
    // Start is called before the first frame update
    void Start()
    {
        FindAllCharacters();
    }

    // Update is called once per frame
    void Update()
    {
        ManageRoundButton();
    }

    public void ManageRoundButton()
    {
        bool scoutsFinished = true;
        {
            foreach(ScoutStats scout in allScouts)
            {
                if (scout.actionsLeft > 0)
                {
                    scoutsFinished = false;
                    break;
                }
            }
        }
        nextRoundButton.interactable = scoutsFinished;
    }

    public void FindAllCharacters()
    {
        allCharacters = FindObjectsOfType<CharacterMover>();
        allScouts = FindObjectsOfType<ScoutStats>();
        allGhosts = FindObjectsOfType<GhostStats>();
    }

    public void EndRound()
    {
        RunGhostRound();
        
        foreach(ScoutStats scout in allScouts)
        {
            scout.actionsLeft = scout.actionsBase;
        }
    }

    public void RunGhostRound()
    {
        foreach(GhostStats ghost in allGhosts)
        {
            //take turn!
            CharacterMover ghostMover = ghost.GetComponent<CharacterMover>();
            StartCoroutine(ghostMover.MoveCharacterCoroutine(ghostMover.currentGridPosition.x - 1, ghostMover.currentGridPosition.y));
        }
    }
}
