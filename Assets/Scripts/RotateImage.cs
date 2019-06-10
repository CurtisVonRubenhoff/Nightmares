using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class RotateImage : MonoBehaviour {

    [SerializeField]
    private float rotateSpeed;
	
	// Update is called once per frame
	void Update ()
    {
        float input = Input.GetAxis("Horizontal");

        this.transform.Rotate(Vector3.forward, rotateSpeed * input * Time.deltaTime);
    }
}
