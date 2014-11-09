using UnityEngine;
using System.Collections;

public class JFlipBack : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("r")) {
			rigidbody.velocity = new Vector3(0f, 0f, 0f);
			Vector3 a = transform.localRotation.eulerAngles;
			a.x = 0;
			a.y = Mathf.Repeat(a.y + Input.GetAxis("Horizontal") * 5f, 360f);
			a.z = 0;
			transform.localRotation = Quaternion.Euler(a);
			transform.localPosition = transform.localPosition + Vector3.up;
		}		
	}
}
