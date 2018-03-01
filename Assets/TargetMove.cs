using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour {

    Vector3 newPosition;
	// Use this for initialization
	void Start () {
        newPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.name != gameObject.name)
                {
                    moveAndResizeToPoint(hit);
                }
            }
        }
	}

    void moveAndResizeToPoint(RaycastHit hit)
    {
        var r = hit.transform.gameObject.GetComponent<Renderer>();
        transform.position = r.bounds.center;
    }

    void moveToPoint(RaycastHit hit)
    {
        newPosition = hit.point;
        Debug.Log(hit.transform.gameObject);
        transform.position = newPosition;
    }

}
