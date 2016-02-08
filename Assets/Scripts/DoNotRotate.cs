using UnityEngine;
using System.Collections;

public class DoNotRotate : MonoBehaviour {

    Quaternion rotation;

    // Use this for initialization
    void Start () {
        rotation = gameObject.transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        // Lock rotation
        gameObject.transform.rotation = rotation;
    }
}
