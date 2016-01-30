using UnityEngine;
using System.Collections;

public class SpinCircles : MonoBehaviour {

    public float SpinSpeed = 0;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
			RotateLeft();
	}



	void RotateLeft () {
		transform.Rotate (Vector3.forward * SpinSpeed);
	}


}