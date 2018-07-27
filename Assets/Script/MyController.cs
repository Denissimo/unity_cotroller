using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyController : MonoBehaviour {

	public Animator animator;
	public Hashtable Keys = new Hashtable();

	// Use this for initialization
	void Start () {
		//animator.SetBool ("isWalk", true);
		//Keys["Fire1"] = "Fire";
		Keys["Run"] = "isRun";
		Keys["Walk"] = "isWalk";
		Keys["Left"] = "isLeft";
		Keys["Right"] = "isRight";
		Keys["Back"] = "isBack";
		foreach(DictionaryEntry item in Keys){
			//Debug.Log (item.Key);
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach(DictionaryEntry item in Keys){
			animator.SetBool ((string)item.Value, Input.GetButton((string)item.Key));
		//	Debug.Log (Input.GetButton((string)item.Key));
		}
		//Debug.Log (Input.GetButton("Walk"));
	}
}
