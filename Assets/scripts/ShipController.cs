using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class ShipController : MonoBehaviour {

	//private Animation anim;
	public Vector3 rotationEulerAngles;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		//anim = GetComponent<Animation> ();
		rb = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {

		float x = CrossPlatformInputManager.GetAxis ("Horizontal");
		float y = CrossPlatformInputManager.GetAxis ("Vertical");

		//transform.position += new Vector3(0,0,y/10);
		//transform.position += new Vector3(x/10,0,0);

		Quaternion rotation = Quaternion.Euler(rotationEulerAngles.x, rotationEulerAngles.y, rotationEulerAngles.z);
		Matrix4x4 m = Matrix4x4.Rotate(rotation);
		Vector3 movement = new Vector3 (x/20, 0.0f, y/20);
		movement = m.MultiplyPoint3x4(movement);
		//enter trumps speed here!!!
		rb.velocity = movement * 1.5f;

		if (x != 0 && y != 0) {
			Vector3 rbangles = new Vector3 (transform.eulerAngles.x, Mathf.Atan2 (x, y) * Mathf.Rad2Deg, transform.eulerAngles.z);
			transform.eulerAngles = m.MultiplyPoint3x4(rbangles);
			//transform.eulerAngles = new Vector3 (transform.eulerAngles.x, Mathf.Atan2 (x, y) * Mathf.Rad2Deg, transform.eulerAngles.z);
		}
	}	
}
