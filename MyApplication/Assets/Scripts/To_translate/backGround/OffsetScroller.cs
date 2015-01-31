using UnityEngine;
using System.Collections;

public class OffsetScroller : MonoBehaviour {
	public float		scrollSpeed;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float x = Mathf.Repeat (Time.time * scrollSpeed, 0.999999f);
		Vector2 offset = new Vector2 (x, 0);
		renderer.sharedMaterial.SetTextureOffset ("_MainTex", offset);
	}
}
