using UnityEngine;
using System.Collections;

public class SkylineManager : MonoBehaviour {
	public Transform				mPrefab;
	public int						mNumberOfObjects;
	public Vector3					mStartPosition;
	public Vector3					mDistanceBetween;
	public float					mRecycleOffset;

	private Vector3					mNextPosition;
	private Queue					mObjectQueue;

	// Use this for initialization
	void Start () {
		mObjectQueue = new Queue(mNumberOfObjects);
		for (int i = 0; i < mNumberOfObjects; i++) {
			mObjectQueue.Enqueue ((Transform)Instantiate (mPrefab));
		}
		mNextPosition = mStartPosition;

		for (int i = 0; i < mNumberOfObjects; i++) {
			Recycle();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (((Transform)mObjectQueue.Peek()).localPosition.x + mRecycleOffset < Runner.mDistanceTravelled) {
			Recycle();
		}
	}

	void Recycle() {

		Vector3 position = mNextPosition;

		Transform o = (Transform)mObjectQueue.Dequeue();
		o.localPosition = position;
		mNextPosition.x += o.localScale.x + mDistanceBetween.x;
		mObjectQueue.Enqueue(o);
	}
}
