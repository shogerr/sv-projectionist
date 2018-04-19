using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorMesh : MonoBehaviour {

    MeshRenderer meshRenderer;
    public Sensor sensor;
    Color targetColor = Color.green;
    Color originalColor;

	// Use this for initialization
	void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
	}


    void OnMouseOver()
    {
        meshRenderer.material.color = targetColor;
    }

    void OnMouseExit()
    {
        meshRenderer.material.color = originalColor;
    }
}
