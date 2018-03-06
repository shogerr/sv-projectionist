using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScreenHandler : MonoBehaviour {
    private Mesh[] screens;

    private string[] meshNames = new string[] { "Group#423_001" };
    public Mesh screen;

	// Use this for initialization
	void Start () {
        screens[0] = screen;
        screens = new Mesh[meshNames.Length + 1];
        for (int i = 1; i <= meshNames.Length; i++)
        {
            screens[i] = (Mesh) GameObject.Find(meshNames[i]).GetComponent<MeshFilter>().mesh;
        }

        this.transform.parent.GetComponent<Button>().onClick.AddListener(PositionCamera);
	}
    void PositionCamera()
    {
        
    }

	// Update is called once per frame
	void Update () {
		
	}
}
