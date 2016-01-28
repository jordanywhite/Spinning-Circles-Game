using UnityEngine;
using System.Collections;

public class SpinCenterCircle : MonoBehaviour
{
	float speed = .5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotateLeft();
    }



    void RotateLeft()
    {
        transform.Rotate(Vector3.forward * speed);
    }


}