using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	public float Movespeed = 5;

	private Rigidbody2D rb2d;
	private PolygonCollider2D collider2d;

	private Vector3 nonZeroDirection = new Vector3(5, 0, 0);
	private Vector3 movementInput = new Vector3(0, 0, 0);
	private Vector3 mousePos = new Vector3(0, 0, 0);

	// Start is called before the first frame update
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		collider2d = GetComponent<PolygonCollider2D>();
	}

	private void Update()
	{
		float xInput = Input.GetAxisRaw("Horizontal");
		float yInput = Input.GetAxisRaw("Vertical");
		movementInput = new Vector3(xInput, yInput, 0);

		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0f;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		SetVelocity();
		SetRotation();
	}

	private void SetVelocity()
	{
		if (movementInput != Vector3.zero)
		{
			nonZeroDirection = movementInput;
		}

		rb2d.velocity = movementInput.normalized * Movespeed;
	}

	private void SetRotation()
	{
		var playerToMouse = new Vector2(mousePos.x, mousePos.y) - rb2d.position;
		float angle = Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg - 90f;
		rb2d.rotation = angle;
	}
}
