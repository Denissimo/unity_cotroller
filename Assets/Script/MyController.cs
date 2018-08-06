using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MyController : MonoBehaviour {

	public GUIStyle style;
	public Animator animator;
	public Hashtable AnimateKeys = new Hashtable();
	public Hashtable Forces = new Hashtable();
	public Hashtable Movements = new Hashtable();
	public GameObject player;
	public GameObject sphere;
	public GameObject camera;
	public GameObject cameraBox;
	public Rigidbody RigidBody;
	public float koefForce;
	public float koefMove;
	public Vector3 MovementSumm;
	public bool isMove;
	public Vector3 MousePos;

	public float MouseSpeedX = 4.0f;
	public float MouseSpeedY = 4.0f;
	public GameObject testOb;

	private float RotationSpeedY = 3.0f;
	private int signRotationY;
	private Vector3 bodyCenter;
	private Quaternion cameraRotation;
	private float distanceBodyCamera = 3.0f;

	//private float cameraRotY = 0.1f;
	//private float cameraRotX = 0.1f;
	private float mouseSence = 0.05f;

	// Use this for initialization
	void Start () {
		Debug.Log (Vector3.up);
		//testOb = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Rigidbody.ClosestPointOnBounds();
		//rb.AddForce( Vector3.forward * 1000f, ForceMode.Acceleration );

		//animator.SetBool ("isWalk", true);
		//AnimateKeys["Fire1"] = "Fire";
		//camera = Camera(0);

		AnimateKeys["Run"] = "isRun";
		AnimateKeys["Walk"] = "isWalk";
		AnimateKeys["Left"] = "isLeft";
		AnimateKeys["Right"] = "isRight";
		AnimateKeys["Back"] = "isBack";

		//Movements["Run"] = transform.forward * koefMove;
		//Movements["Walk"] =transform.forward * koefMove/2;
		//Movements["Left"] = -transform.right * koefMove;
		//Movements["Right"] = transform.right * koefMove;
		//Movements["Back"] = - transform.forward * koefMove;

		Movements["Run"] = Vector3.forward * koefMove;
		Movements["Walk"] =Vector3.forward * koefMove/2;
		Movements["Left"] = -Vector3.right * koefMove;
		Movements["Right"] = Vector3.right * koefMove;
		Movements["Back"] = - Vector3.forward * koefMove;

		Forces["Jump"] = Vector3.up * koefForce;
		//Debug.Log (koefForce);
		//Debug.Log ((Vector3)Forces["Jump"]);

		RigidBody = GetComponent<Rigidbody>();
		foreach(DictionaryEntry item in AnimateKeys){
			//Debug.Log (item.Key);
		}
	}
	


	// Update is called once per frame
	void FixedUpdate () {

		//camera.transform.Translate(new Vector3(0, 1, 0));
		MousePos = Input.mousePosition;
		bodyCenter = transform.TransformPoint (0, 1, 0);
		cameraBox.transform.position = bodyCenter;

		//camera.transform.RotateAround(bodyCenter, Vector3.up, Input.GetAxis("Mouse X") * MouseSpeedX);
		cameraBox.transform.Rotate(0, Input.GetAxis("Mouse X") * MouseSpeedX, 0);
		//camera.transform.RotateAround(bodyCenter, transform.rotation*Vector3.right, -Input.GetAxis("Mouse Y") * MouseSpeedY);
		camera.transform.RotateAround(bodyCenter, camera.transform.rotation*Vector3.right, -Input.GetAxis("Mouse Y") * MouseSpeedY);
		//transform.Rotate (0, RotationSpeedY * Math.Sign(Input.GetAxis("Mouse X")), 0);
		//camera.transform.LookAt (bodyCenter);


		/*
		if (Vector3.Distance (camera.transform.position, bodyCenter) > distanceBodyCamera) {
			camera.transform.position = Vector3.MoveTowards(camera.transform.position, bodyCenter, 1);
		}
		*/

		foreach(DictionaryEntry item in AnimateKeys){
			animator.SetBool ((string)item.Value, Input.GetButton((string)item.Key));
		}

		foreach(DictionaryEntry item in Forces){
			if(Input.GetButton((string)item.Key)) {
			    //Debug.Log ((Vector3)item.Value);
				RigidBody.AddForce((Vector3)item.Value);
			}
		}



		isMove = false;
		MovementSumm = new Vector3(0, 0, 0);
		foreach(DictionaryEntry item in Movements){
        	if(Input.GetButton((string)item.Key)) {
				isMove = true;
				MovementSumm += (Quaternion.Euler(0.0f, camera.transform.rotation.eulerAngles.y, 0.0f)*(Vector3)item.Value);
				//MovementSumm += (transform.rotation*(Vector3)item.Value);
        		//transform.Translate((Vector3)item.Value/1000);
        		//RigidBody.AddForce((Vector3)item.Value);
				//RigidBody.AddRelativeForce((Vector3)item.Value);
				//RigidBody.velocity = (Vector3)item.Value;
        	}
        }
		if (isMove) {
			RigidBody.velocity = MovementSumm;
			transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles.x, camera.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
			//transform.rotation = Quaternion.Euler (transform.rotation.x * 180, 90.0f, transform.rotation.z * 180);
			//Debug.Log ("Camera: " + cameraBox.transform.rotation.y + " Player: " + transform.rotation.y);
			//transform.rotation = camera.transform.rotation;
		}

		Debug.Log (cameraBox.transform.rotation.eulerAngles.y);

		//Debug.Log ("Camera: " + camera.transform.rotation.y + "Player: " + transform.rotation.y);
		//camera.transform.position = bodyCenter + new Vector3(0.01f, 0.01f, 0.01f);

		//if(camera.transform.rotation.x > -0.4f & camera.transform.rotation.x < 0.4f) {
		//testOb.transform.position = bodyCenter + cameraRotation * new Vector3(0, 3, 0);

		//Vector3 camRotX = bodyCenter * camera.transform.rotation;
		//Debug.Log (camRotX);
		//}

		//Debug.Log (RigidBody.GetPointVelocity(sphere.transform.position));
		//Debug.Log (Input.GetButton("Walk"));
	}


	void OnGUI () {
		// Make a label that uses the "box" GUIStyle.
		GUI.Label (new Rect (0,0,200,100), "" + Screen.width + " " + Screen.height
			+ " X: " + Input.GetAxis("Mouse X")
			+ " Y: " + Input.GetAxis("Mouse Y")
			+ " rot: " + (camera.transform.rotation.y)// - transform.rotation.y)
		, "box");

		GUI.Label (new Rect (0,40,200,100), "" + transform.rotation.y, style);

		// Make a button that uses the "toggle" GUIStyle
		GUI.Button (new Rect (10,140,180,20), "This is a button", "toggle");
	}
}
