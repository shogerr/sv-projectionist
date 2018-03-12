using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObject : MonoBehaviour {

    public void ToggleObject (GameObject o)
    {
        o.SetActive(!o.activeSelf);
    }
}
