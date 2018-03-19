using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetMove : MonoBehaviour {
    private SensorController sensorController;
    public Material hiddenMaterial;
    public Material targetMaterial;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;

    Vector3 newPosition;

    private int fingerID = -1;

    private void Awake()
    {
#if UNITY_EDITOR
        fingerID = 0;
#endif
    }

    // Use this for initialization
    void Start ()
    {
        newPosition = transform.position;
        sensorController = SensorController.Instance;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            // If this is a UI object, just return.
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                // Set the active sensor for SensorController to the clicked sensor
                if (hit.transform.gameObject.GetComponentInParent<Sensor>() ?? null)
                {
                    if (sensorController != null)
                        sensorController.ActiveSensorObject = hit.transform.gameObject.GetComponentInParent<Sensor>();
                    return;
                }

                // Otherwise set the structure's shader
                if (hit.transform.gameObject.name != gameObject.name)
                {
                    AdjustShader(hit);
                }
            }
        }
	}

    void AdjustShader(RaycastHit hit)
    {
        var r = hit.transform.GetComponent<Renderer>();
        if (r.material.shader != targetMaterial.shader)
        {
            var c = r.gameObject.AddComponent<PaintedSurface>();
            c.originalMaterial = r.sharedMaterial;

            r.material = targetMaterial;
        }
        else
        {
            var c = r.gameObject.GetComponent<PaintedSurface>();
            r.material = c.originalMaterial;
            Destroy(c);
        }
    }

    void CreateNewScreen(RaycastHit hit)
    {
        Debug.Log(hit.transform.name);
        var r = hit.transform.gameObject.GetComponent<Renderer>();
        if (hit.transform.gameObject.layer == 0)
        {
            var screen = GameObject.CreatePrimitive(PrimitiveType.Cube);
            screen.transform.position = r.bounds.center;
            screen.transform.localScale = r.bounds.size;
            screen.layer = 9;
            screen.gameObject.GetComponent<Renderer>().material = targetMaterial;
            //screen.tag = "screen";
            screen.transform.parent = hit.transform;
            screen.name = "SCREEN_" + hit.transform.name;
            //r.material = hiddenMaterial;
            hit.transform.gameObject.layer = 8;
        }
        else if (hit.transform.gameObject.layer == 9)
        {
            hit.transform.parent.transform.gameObject.layer = 0;
            Destroy(hit.transform.gameObject);
        }
        else if (hit.transform.gameObject.layer == 8)
        {
            hit.transform.gameObject.layer = 0;
            var t = hit.transform.gameObject.GetComponentsInChildren<Transform>()[1];
            Debug.Log("Destroying: " + t.name);
            Destroy(t.gameObject);
        } 
    }

    void MoveAndResizeToPoint(RaycastHit hit)
    {
        var r = hit.transform.gameObject.GetComponent<Renderer>();
        Debug.Log(hit.transform.name);
        transform.position = r.bounds.center;
        transform.localScale = r.bounds.size;

        r.material = hiddenMaterial;
    }

    void MoveToPoint(RaycastHit hit)
    {
        newPosition = hit.point;
        Debug.Log(hit.transform.gameObject);
        transform.position = newPosition;
    }

}
