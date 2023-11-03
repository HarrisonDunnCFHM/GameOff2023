using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMouseover : MonoBehaviour
{
    [SerializeField] SpriteRenderer myRenderer;
    [SerializeField] Color defaultColor;
    [SerializeField] Color hoveredColor;
    [SerializeField] Collider myCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnMouseEnter()
    {
        myRenderer.color = hoveredColor;
    }

    private void OnMouseExit()
    {
        myRenderer.color = defaultColor;
    }

}
