using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	public Color defaultColor;
	public Color GotHitColor;

	public float Movespeed = 5;
	public float DashSpeed = 15;

	private Rigidbody2D rb2d;
	private PolygonCollider2D collider2d;
	private SpriteRenderer sprite;

	private Vector3 nonZeroDirection = new Vector3(5, 0, 0);
	private Vector3 movementInput = new Vector3(0, 0, 0);
	private Vector3 mousePos = new Vector3(0, 0, 0);

	private bool isDashing = false;
	private float dashTime = 0.20f;
	private float dashTimer = 0.0f;

	private bool gotHit = false;
	private float gotHitTime = 0.5f;
	private float gotHitTimer = 0.0f;

	private float HP = 3;
	private float maxHP = 3;

	// Start is called before the first frame update
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		collider2d = GetComponent<PolygonCollider2D>();
		sprite = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		float xInput = Input.GetAxisRaw("Horizontal");
		float yInput = Input.GetAxisRaw("Vertical");
		movementInput = new Vector3(xInput, yInput, 0);

		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0f;

		if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
		{
			isDashing = true;
		}

		if (isDashing)
		{
			dashTimer += Time.deltaTime;
			if (dashTimer > dashTime)
			{
				isDashing = false;
				dashTimer = 0.0f;
			}
		}

		if (gotHit)
		{
			SetColorAfterHit();

			gotHitTimer += Time.deltaTime;
			if (gotHitTimer > gotHitTime) 
			{
				gotHit = false; 
				sprite.color = defaultColor;
				gotHitTimer = 0;
			}

			Debug.Log("PLAYER HP is: " + HP);
		}
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

		if (!isDashing)
		{
			rb2d.velocity = movementInput.normalized * Movespeed;
		}
		else
		{
			rb2d.velocity = nonZeroDirection * DashSpeed;
		}
	}

	private void SetRotation()
	{
		var playerToMouse = new Vector2(mousePos.x, mousePos.y) - rb2d.position;
		float angle = Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg - 90f;
		rb2d.rotation = angle;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "Enemy")
		{
			gotHit = true;
			HP -= 1;
		}

		if(collision.transform.tag == "BossBullet")
		{
			var bullet = collision.transform.GetComponent<BossBullet>();
			if (!bullet.GetHasHitAlready() && !gotHit)
			{
				gotHit = true;
				HP -= 1;
			}
			bullet.SetHasHitAlready(true);
		}
	}

	private void SetColorAfterHit()
	{
		if (gotHitTimer > 0 && gotHitTimer < 0.1f)
		{
			sprite.color = GotHitColor;
		}
		else if (gotHitTimer > 0.1f && gotHitTimer < 0.2f)
		{
			sprite.color = defaultColor;
		}
		else if (gotHitTimer > 0.2f && gotHitTimer < 0.3f)
		{
			sprite.color = GotHitColor;
		}
		else if (gotHitTimer > 0.4f && gotHitTimer < 0.5f)
		{
			sprite.color = defaultColor;
		}
		else if (gotHitTimer > 0.5)
		{
			sprite.color = GotHitColor;
		}
	}
}
