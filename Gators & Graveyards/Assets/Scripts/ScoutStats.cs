using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutStats : MonoBehaviour
{
    public int maxMoveRange = 4;
    public int actionsBase = 1;
    public int actionsLeft = 1;

    public int attackBase = 1;
    public int attackCurrent;

    public bool isScared = false;
    
    // Start is called before the first frame update
    void Start()
    {
        attackCurrent = attackBase;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool UseAction()
    {
        if (actionsLeft > 0)
        {
            actionsLeft--;
            return true;
        }
        else return false;
    }
}
