using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float speed;

    public Transform middle;

    private float x;
    private float y;

	void Start () {
		
	}
	
	void Update () {
        x = Input.GetAxis("Mouse X") * Time.deltaTime * -speed;
        y = Input.GetAxis("Mouse Y") * Time.deltaTime * -speed;

        transform.RotateAround(Vector3.zero, new Vector3(0, x, 0), speed * Time.deltaTime);
        //transform.RotateAround(Vector3.zero, new Vector3(0, 0, y), speed * Time.deltaTime);
    }
}
