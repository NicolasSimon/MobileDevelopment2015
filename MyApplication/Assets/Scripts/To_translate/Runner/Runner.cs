using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {
	public static float			mDistanceTravelled;
	public Vector3				mJumpVelocity;

	public float				mAcceleration;
	private bool				mTouchingPlatform;

	private int					mStatus; 																			//0 for normal, 1 for small, 2 for big
	private float				mStatusChangedTime = 0.0f;
	private float				mDeltaTimeStatusChange = 3.0f;
	private bool				mHasStatusChanged = false;
	private int					mZoomRatio = 2;

	private Vector3				mCameraPosition;
	private float				mRunnerXPosition;



	
	private float fingerStartTime  = 0.0f;
	private Vector2 fingerStartPos = Vector2.zero;
	
	private bool isSwipe = false;
	private float minSwipeDist  = 50.0f;
	private float maxSwipeTime = 0.5f;







	void Start () {
		mCameraPosition = Camera.main.transform.position;
		mRunnerXPosition = transform.localPosition.x;
	}

	void LateUpdate() {
		Camera.main.transform.position = mCameraPosition;
		Vector3 position = new Vector3 (
			mRunnerXPosition,
			transform.localPosition.y,
			transform.localPosition.z);
		transform.localPosition = position;
	}
	
	void Update () {
																													//Check if zoomIn / zoomOut//
		if (mHasStatusChanged) {
			mStatusChangedTime += Time.deltaTime;
			if (mStatusChangedTime >= mDeltaTimeStatusChange) {
				if (mStatus == 1) {
					hasZoomIn(true);
				} else {
					hasZoomOut(true);
				}
			}
		}
		if ((Input.touchCount == 2)/* && (mStatus == 0)*/) {
			Touch touchZero = Input.GetTouch (0);
			Touch touchOne = Input.GetTouch (1);
			
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
			
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;											//Will be 0 if not changed, negative if gets bigger, positive if gets smaller
			if ((deltaMagnitudeDiff > 0) && (mStatus != 1)) {
				//Gets smaller//
				hasZoomOut(false);
			} else if ((deltaMagnitudeDiff < 0) && (mStatus != 2)) {
				//Gets bigger//
				hasZoomIn(false);
			}

		} else {
			if (Input.touchCount > 0) {
				foreach (Touch touch in Input.touches) {
					switch (touch.phase) {
					case TouchPhase.Began :
																													/* this is a new touch */
						isSwipe = true;
						fingerStartTime = Time.time;
						fingerStartPos = touch.position;
						break;
						
					case TouchPhase.Canceled :
																													/* The touch is being canceled */
																													//CHECK IF CAN DOUBLE-JUMP AND ADD CODE HERE//
						if (mTouchingPlatform) {																	//JUMP//
							rigidbody.AddForce (mJumpVelocity, ForceMode.VelocityChange);
							mTouchingPlatform = false;
						}
						isSwipe = false;
						break;
						
					case TouchPhase.Ended :
						
						float gestureTime = Time.time - fingerStartTime;
						float gestureDist = (touch.position - fingerStartPos).magnitude;
						
						if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist) {
							Vector2 direction = touch.position - fingerStartPos;
							Vector2 swipeType = Vector2.zero;
							
							if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
																													// the swipe is horizontal:
								swipeType = Vector2.right * Mathf.Sign(direction.x);
							}else {
																													// the swipe is vertical:
								swipeType = Vector2.up * Mathf.Sign(direction.y);
							}
							if(swipeType.x != 0.0f) {
								if(swipeType.x > 0.0f) {
																													// MOVE RIGHT
								}else {
																													// MOVE LEFT
								}
							}
							if(swipeType.y != 0.0f ) {
								if(swipeType.y > 0.0f) {
																													// MOVE UP
								}else {
																													// MOVE DOWN
								}
							}
						}
						break;
					}
				}
			}
		}
		for (int i = 0; i < Input.touchCount; i++) {
			if (mTouchingPlatform && Input.GetTouch(i).phase == TouchPhase.Began) {
				rigidbody.AddForce (mJumpVelocity, ForceMode.VelocityChange);
				mTouchingPlatform = false;
			}
		}
		mDistanceTravelled = transform.localPosition.x;
	}

	void hasZoomIn(bool force) {
		if ((mStatus == 0) || force) {
			Vector3 scale = new Vector3 (
				transform.localScale.x * mZoomRatio,
				transform.localScale.y * mZoomRatio,
				transform.localScale.z);
			transform.localScale = scale;
			if (mStatus == 0) {
				mStatus = 2;
				mHasStatusChanged = true;
				mStatusChangedTime = 0.0f;
			} else {
				mStatus = 0;
				mHasStatusChanged = false;
			}
		}
	}
	
	void hasZoomOut(bool force) {
		//CHECK IF CAN ZOOM OUT FIRST//
		if ((mStatus == 0) || force) {
			Vector3 position = transform.localPosition;
			Vector3 scale = new Vector3 (
				transform.localScale.x / mZoomRatio,
				transform.localScale.y / mZoomRatio,
				transform.localScale.z);
			transform.localScale = scale;
			transform.localPosition = position;
			if (mStatus == 0) {
				mStatus = 1;
				mHasStatusChanged = true;
				mStatusChangedTime = 0.0f;
			} else {
				mStatus = 0;
				mHasStatusChanged = false;
			}
		}
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
