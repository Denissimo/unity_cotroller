using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootBox : MonoBehaviour {

	public MyController Controller;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		Controller.isGroundContact = true;
		Controller.groundFriction = getGroundFriction (other);
		Controller.groundVelocity = getGroundVelocity (other);
		Controller.groundMatherial = getGroundMatherial (other);
		//Debug.Log (Controller.groundMatherial);
		//Debug.Log(other.gameObject.GetComponent<Rigidbody>().velocity);
		//Debug.Log(other.gameObject.GetComponent<Renderer>().material);
		//Debug.Log(other.gameObject.GetComponent<Collider>().material.name);
		//Debug.Log(other.gameObject.GetComponent<Collider>().material.dynamicFriction);
		//Debug.Log(other.gameObject.GetComponent<Collider>().material.staticFriction);
	}

	private void OnTriggerExit (Collider other)
	{
		Controller.isGroundContact = false;
		Controller.groundFriction = 0.0f;
		Controller.groundMatherial = "";
	}


	private void OnTriggerStay (Collider other)
	{
		Controller.isGroundContact = true;
		Controller.groundFriction = getGroundFriction (other);
		Controller.groundVelocity = getGroundVelocity (other);
		Controller.groundMatherial = getGroundMatherial (other);
		//Debug.Log (other.transform.position);
	}

	private float getGroundFriction (Collider other) {
		if (other.gameObject.GetComponent<Collider> () == null) {
			return 1.0f;
		} else {
			return (other.gameObject.GetComponent<Collider> ().material.staticFriction
				+ other.gameObject.GetComponent<Collider> ().material.staticFriction) / 2;
		}
	}

	private Vector3 getGroundVelocity (Collider other) {
		if (other.gameObject.GetComponent<Rigidbody> () == null) {
			return new Vector3 (0.0f, 0.0f, 0.0f);
		} else {
			return other.gameObject.GetComponent<Rigidbody> ().velocity;
		}
	}

	private string getGroundMatherial (Collider other) {
		if (other.gameObject.GetComponent<Collider>() == null) {
			return "";
		} else {
			return other.gameObject.GetComponent<Collider>().material.name;
		}
	}
}
