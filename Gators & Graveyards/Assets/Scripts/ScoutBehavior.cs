using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutBehavior : MonoBehaviour
{
    ScoutStats scoutStats;
    [SerializeField] SpriteRenderer myRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        scoutStats = GetComponent<ScoutStats>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (scoutStats.actionsLeft <= 0)
        {
            myRenderer.color = Color.gray;
        }
        else
        {
            myRenderer.color = Color.white;
        }
    }
}
