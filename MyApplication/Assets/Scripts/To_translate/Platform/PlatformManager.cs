using UnityEngine;
using System.Collections;

public class PlatformManager : MonoBehaviour {
	public Transform				mPrefab;
	public int						mNumberOfObjects;
	public Vector3					mStartPosition;
	public float					mRecycleOffset;
	public Vector3					mMinSize;
	public Vector3					mMaxSize;
	public Vector3					mMinGap;
	public Vector3					mMaxGap;
	public float					mMinY;
	public float					mMaxY;
	
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
		Vector3 scale = new Vector3 (
			Random.Range (mMinSize.x, mMaxSize.x),
			Random.Range (mMinSize.y, mMaxSize.y),
			Random.Range (mMinSize.z, mMaxSize.z));
		
		Vector3 position = mNextPosition;
		position.x += scale.x * 0.5f;
		position.y += scale.y * 0.5f;
		
		Transform o = (Transform)mObjectQueue.Dequeue();
		o.localScale = scale;
		o.localPosition = position;
		mObjectQueue.Enqueue(o);

		mNextPosition += new Vector3 (
			Random.Range (mMinGap.x, mMaxGap.x) + scale.x,
			Random.Range (mMinGap.y, mMaxGap.y),
			Random.Range (mMinGap.z, mMaxGap.z));

		if (mNextPosition.y < mMinY) {
			mNextPosition.y = mMinY + mMaxGap.y;
		} else if (mNextPosition.y > mMaxY) {
			mNextPosition.y = mMaxY - mMaxGap.y;
		}
	}
}
