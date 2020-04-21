using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// public variables
	public float movementSpeed = 10.0f;
	public Rigidbody2D rigidBody;
	public Animator myAnim;
	public bool movementEnabled = true;
	public static PlayerController instance;
	public string areaTransitionName;

	// private variables
	private Vector3 topRightLimit;
	private Vector3 bottomLeftLimit;

	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	
	// Update is called once per frame
	void Update () {


		if (movementEnabled) {
			rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical") ) * this.movementSpeed;
		} else {
			rigidBody.velocity = Vector2.zero;
		}

		myAnim.SetFloat("MoveX", rigidBody.velocity.x);
		myAnim.SetFloat("MoveY", rigidBody.velocity.y);
		
		if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == 1) {
			if (movementEnabled) {
				myAnim.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));
				myAnim.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
			}
		}

		var boundsX = Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x); 
        var boundsY = Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y);
        transform.position = new Vector3(boundsX, boundsY, transform.position.z);
	}

	public void SetBounds(Vector3 bottomLeft, Vector3 topRight) {
		this.bottomLeftLimit = bottomLeft + new Vector3(0.5f, 1f, 0f);
		this.topRightLimit = topRight + new Vector3(-0.5f, -1f, 0f);
	}

}
