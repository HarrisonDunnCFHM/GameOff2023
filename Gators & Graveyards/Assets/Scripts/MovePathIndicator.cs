using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePathIndicator : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
        transform.eulerAngles = new Vector3(90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMovePathIndicator(List<GameObject> path)
    {
        if (path.Count > 0)
        {
            myLineRenderer.positionCount = path.Count;
            for (int step = 0; step < path.Count; step++)
            {
                Vector3 pointPosition = new Vector3(path[step].GetComponent<GridPointStats>().x,
                    0.1f,
                    path[step].GetComponent<GridPointStats>().y);
                myLineRenderer.SetPosition(path.Count - step - 1, pointPosition);
            }
           // myLineRenderer.Simplify(1);
        }
    }

}
