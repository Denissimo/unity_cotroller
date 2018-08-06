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

	public float MouseSpeedX = 4.0f;
	public float MouseSpeedY = 4.0f;
	public GameObject testOb;

	//private float velocity;

	private float rotationSpeedY = 10.0f;
	private int signRotationY;
	private Vector3 bodyCenterPos;
	private Quaternion cameraRotation;
	private float distanceBodyCamera = 3.0f;

	//private float cameraRotY = 0.1f;
	//private float cameraRotX = 0.1f;
	private float mouseSence = 0.05f;
	private bool isGroundContact;

	// Use this for initialization
	private void Start () {

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
	private void FixedUpdate () {

		bodyCenterPos = transform.TransformPoint (0, 1, 0);
		MouseController(bodyCenterPos);
		//transform.Rotate (0, rotationSpeedY * Math.Sign(Input.GetAxis("Mouse X")), 0);
		//camera.transform.LookAt (bodyCenterPos);


		/*
		if (Vector3.Distance (camera.transform.position, bodyCenterPos) > distanceBodyCamera) {
			camera.transform.position = Vector3.MoveTowards(camera.transform.position, bodyCenterPos, 1);
		}
		*/

		foreach(DictionaryEntry item in AnimateKeys){
			animator.SetBool ((string)item.Value, Input.GetButton((string)item.Key));
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
			//transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles.x, camera.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

			transform.rotation = Quaternion.Lerp(transform.rotation,
				new Quaternion(transform.rotation.x, cameraBox.transform.rotation.y, transform.rotation.z, cameraBox.transform.rotation.w),
				Time.deltaTime * rotationSpeedY
			);

		}

		//Debug.Log ("" + cameraBox.transform.rotation.eulerAngles.y/180 + " >> " + cameraBox.transform.rotation.y);
		Debug.Log ((RigidBody.velocity - cameraBox.transform.forward).magnitude); //RigidBody.velocity - cameraDirection

		foreach(DictionaryEntry item in Forces){
			if(Input.GetButton((string)item.Key)) {
				//Debug.Log ((Vector3)item.Value);
				RigidBody.AddForce((Vector3)item.Value, ForceMode.Impulse);
			}
		}

		//Debug.Log (RigidBody.GetPointVelocity(sphere.transform.position));
		//Debug.Log (Input.GetButton("Walk"));
	}

	private void MouseController(Vector3 bodyCenterPos) {
		cameraBox.transform.position = bodyCenterPos;
		cameraBox.transform.Rotate(0, Input.GetAxis("Mouse X") * MouseSpeedX, 0);
		camera.transform.RotateAround(bodyCenterPos, camera.transform.rotation*Vector3.right, -Input.GetAxis("Mouse Y") * MouseSpeedY);
	}

	private void OnTriggerEnter(Collider other)
	{
		//Debug.Log(other.gameObject.name);
		//Debug.Log(other.gameObject.GetComponent<Renderer>().material);
		Debug.Log(other.gameObject.GetComponent<Collider>().material.dynamicFriction);
		Debug.Log(other.gameObject.GetComponent<Collider>().material.staticFriction);
	}

	private void OnGUI () {
		// Make a label that uses the "box" GUIStyle.
		GUI.Label (new Rect (0,0,200,100), "" + Screen.width + " " + Screen.height
			+ " X: " + Input.GetAxis("Mouse X")
			+ " Y: " + Input.GetAxis("Mouse Y")
			+ " rot: " + (camera.transform.rotation.y)// - transform.rotation.y)
		, "box");

		GUI.Label (new Rect (0,40,200,100), "" + transform.rotation.y, style);

		// Make a button that uses the "toggle" GUIStyle
		GUI.Button (new Rect (10,140,180,20), "" + RigidBody.velocity.magnitude, "toggle");
	}
}
