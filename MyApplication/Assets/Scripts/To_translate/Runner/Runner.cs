using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {
	public static float			mDistanceTravelled;
	public Vector3				mJumpVelocity;

	public float				mAcceleration;
	private bool				mTouchingPlatform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < Input.touchCount; i++) {
			if (/*mTouchingPlatform && */(Input.GetTouch(i).phase == TouchPhase.Began)) {
				rigidbody.AddForce(mJumpVelocity, ForceMode.VelocityChange);
				mTouchingPlatform = false;
			}
		}
		/*
		if (mTouchingPlatform && Input.GetButtonDown ("Jump")) {
			rigidbody.AddForce(mJumpVelocity, ForceMode.VelocityChange);
			mTouchingPlatform = false;
		}
		*/
		//transform.Translate (5.0f * Time.deltaTime, 0f, 0f);
		mDistanceTravelled = transform.localPosition.x;
	}

	void FixedUpdate() {
		if (mTouchingPlatform) {
			rigidbody.AddForce(mAcceleration, 0f, 0f, ForceMode.Acceleration);
		}
	}

	void OnCollisionEnter() {
		mTouchingPlatform = true;
	}
	
	void OnCollisionExit() {
		mTouchingPlatform = false;
	}
}
