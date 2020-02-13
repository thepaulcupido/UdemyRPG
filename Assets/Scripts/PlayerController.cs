using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float movementSpeed = 10.0f;
	public Rigidbody2D rigidBody;

	public Animator myAnim;

	public static PlayerController instance;

	public string areaTransitionName;

	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical") ) * this.movementSpeed;
		myAnim.SetFloat("MoveX", rigidBody.velocity.x);
		myAnim.SetFloat("MoveY", rigidBody.velocity.y);

		if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == 1) {
			myAnim.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));
			myAnim.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
		}
	}
}
