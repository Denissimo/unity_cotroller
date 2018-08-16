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
	public GameObject footBox;

	public Rigidbody RigidBody;
	public float koefForce;
	public float koefMove;
	public float maxMove;
	public Vector3 MovementSumm;
	public bool isMove;

	public float MouseSpeedX = 4.0f;
	public float MouseSpeedY = 4.0f;
	public GameObject testOb;

	//private float velocity;

	private float rotationSpeedY = 25.0f;
	private int signRotationY;
	private Vector3 bodyCenterPos;
	private Vector3 bodyBottomPos;
	private Quaternion cameraRotation;
	private float distanceBodyCamera = 3.0f;

	//private float cameraRotY = 0.1f;
	//private float cameraRotX = 0.1f;
	private float mouseSence = 0.05f;
	public bool isGroundContact;
	public float groundFriction = 0.0f;
	public string groundMatherial = "";
	public Vector3 groundVelocity;
	private Vector3 stepUp = new Vector3 (0.0f, 100.0f, 0.0f);

	public bool isStep = false;

	private string label3;

	private string[] groundTypes = new string[] {"carpet", "concrete", "dirt", "glass", "grass", "gravel", "metal", "rock", "snow", "water", "wood"}; 
	private Hashtable footstepSounds = new Hashtable(); //carpet, concrete, dirt, glass, grass, gravel, metal, rock, snow, water, wood;
	private AudioSource source;
	private AudioClip clip;

	private int[] miuns = new int[] {1, 2, 3};

	// Use this for initialization
	private void Start () {

		//Debug.Log (Vector3.Cross(Vector3.up, -Vector3.forward));

		source = GetComponent<AudioSource>();
		LoadSounds ();
		//testOb = GameObject.CreatePrimitive(PrimitiveType.Cube);

		AnimateKeys["Run"] = "isRun";
		AnimateKeys["Walk"] = "isWalk";
		AnimateKeys["Left"] = "isLeft";
		AnimateKeys["Right"] = "isRight";
		AnimateKeys["Back"] = "isBack";
		AnimateKeys["Jump"] = "isJump";

		Movements["Run"] = Vector3.forward;
		Movements["Walk"] =Vector3.forward/2;
		Movements["Left"] = -Vector3.right;
		Movements["Right"] = Vector3.right;
		Movements["Back"] = - Vector3.forward;

		Forces["Jump"] = Vector3.up * koefForce;

		//Debug.Log (koefForce);
		//Debug.Log ((Vector3)Forces["Jump"]);

		RigidBody = GetComponent<Rigidbody>();
		foreach(DictionaryEntry item in AnimateKeys){
			//Debug.Log (item.Key);
		}
	}
	
	void LoadSounds()
	{
		//string path = Application.dataPath + "/Sounds/Footsteps";
		string path = "Sounds/Footsteps/";
		foreach(string item in groundTypes){
			AudioClip[] audioClips = Resources.LoadAll<AudioClip>(path + item + "/");
			footstepSounds [item] = audioClips;
		}
		//concrete = Resources.LoadAll<AudioClip>(path + "concrete/");
		//dirt = Resources.LoadAll<AudioClip>(path + "dirt/");
	}

	// Update is called once per frame

	private void Update () {
		bodyCenterPos = transform.position + Vector3.up * 1.3f; // transform.TransformPoint (0, 1.3f, 0);
		bodyBottomPos = transform.position; //transform.TransformPoint (0, 0, 0);
		animator.SetBool ("isGround", isGroundContact);
		MouseController(bodyCenterPos);
	}


	private void FixedUpdate () {
		Debug.DrawRay(new Vector3(0.0f, 1.0f, 0.0f), Vector3.up * 100, Color.green, 20, false);

		foreach(DictionaryEntry item in AnimateKeys){
			animator.SetBool ((string)item.Value, Input.GetButton((string)item.Key));
		}

		doForces ();
		float velo = RigidBody.velocity.magnitude;
		label3 = "" + velo;

		if (isGroundContact) {
			foreach (DictionaryEntry item in Forces) {
				if (Input.GetButton ((string)item.Key)) {
					//Debug.Log ((Vector3)item.Value);
					RigidBody.AddForce ((Vector3)item.Value, ForceMode.Impulse);
				}
			}
		}
	}

	private void footStepSound(){
		if (isStep) {
			//Debug.Log ("step: " + isStep);
			isStep = false;
			AudioClip[] clipArray = (AudioClip[])footstepSounds ["concrete"];
			clip = clipArray[UnityEngine.Random.Range(0, clipArray.Length)];
			source.PlayOneShot(clip, 1.0f);

		}
	}

	private void doForces() {
		Vector3 newMovement = new Vector3 (0.0f, 0.0f, 0.0f);
		bool isMoveAllowed = false;
		bool isRotateAllowed = false;
		foreach (DictionaryEntry item in Movements) {
			if (Input.GetButton ((string)item.Key)) {
				isRotateAllowed = true;
				newMovement += (Vector3)item.Value;
			}
		}

		if (isGroundContact) {
			isMoveAllowed = true;
			if(isMoveAllowed) {
				doForcePush (newMovement);
			}


		}
		footStepSound ();
		if(isRotateAllowed){
			doRotateToMouse();
		}
	}

	private void doForcePush(Vector3 newMovement) {
		newMovement -= groundVelocity;
		Vector3 directMovement = Quaternion.Euler (0.0f, camera.transform.rotation.eulerAngles.y, 0.0f) * newMovement * groundFriction;
		//float velocity = (RigidBody.velocity + directMovement).magnitude;
		float velocity = (RigidBody.velocity + directMovement).magnitude;
		//Debug.Log (velocity);
		if (velocity <= maxMove) {
			RigidBody.AddForce (directMovement * koefMove + stepUp);
		}
	}

	private void doRotateToMouse() {
		transform.rotation = Quaternion.Lerp (transform.rotation,
			new Quaternion (transform.rotation.x, cameraBox.transform.rotation.y, transform.rotation.z, cameraBox.transform.rotation.w),
			Time.deltaTime * rotationSpeedY
		);
	}

	private void MouseController(Vector3 bodyCenterPos) {
		cameraBox.transform.position = bodyCenterPos;
		cameraBox.transform.Rotate(0, Input.GetAxis("Mouse X") * MouseSpeedX, 0);
		camera.transform.RotateAround(bodyCenterPos, camera.transform.rotation*Vector3.right, -Input.GetAxis("Mouse Y") * MouseSpeedY);
	}




	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			Debug.DrawRay(contact.point, contact.normal, Color.red);
		}
		//if (collision.relativeVelocity.magnitude > 2)
		//	audioSource.Play();
	}

	void OnCollisionStay(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			Debug.DrawRay(contact.point, contact.normal, Color.red);
		}
		//if (collision.relativeVelocity.magnitude > 2)
		//	audioSource.Play();
	}

	private void OnGUI () {
		// Make a label that uses the "box" GUIStyle.
		GUI.Label (new Rect (0,0,200,100), "Friction: " + groundFriction, "box");

		GUI.Label (new Rect (0,30,200,100), "" + (transform.position - bodyCenterPos).magnitude, style);
		//GUI.Label (new Rect (0, 60, 200, 100), "Left: " + stepLeft + " Right: " + stepRight);

		// Make a button that uses the "toggle" GUIStyle
		GUI.Button (new Rect (10,140,180,20), "Ground: " + isGroundContact + " Velo: " + label3, "toggle");
	}
}
