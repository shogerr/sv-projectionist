using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceGrabber : MonoBehaviour {

    public GameObject target;

    public Material material;
    private Material lastMaterial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            var newTarget = GetMouseClickObject();
            Debug.Log(newTarget);
            if (newTarget != null)
            {
                if (target != null)
                {
                    target.GetComponent<Renderer>().material = lastMaterial;
                }
                lastMaterial = newTarget.GetComponent<Renderer>().material;
                target = newTarget;
                target.GetComponent<Renderer>().material = material;

                var mesh = target.GetComponent<MeshFilter>().mesh;

            }
        }
	}

    public static GameObject GetMouseClickObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform.gameObject;
        }
        else
        {
            return null;
        }
    }
}
