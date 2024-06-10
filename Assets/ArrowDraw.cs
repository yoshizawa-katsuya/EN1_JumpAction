using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArrowDraw : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private Vector3 clickPosition;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 dist = clickPosition - Input.mousePosition;
            Debug.Log(dist);
        }
    }
}
