using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	public Color DefaultColor;
	public Color GotHitColor;

	public bool IsDashingEnabled = false;

	public SpriteRenderer tip;

	public TextPanelUI text;

	private float movespeed = 5;
	private float dashSpeed = 15;

	private Rigidbody2D rb2d;
	private PolygonCollider2D collider2d;
	private SpriteRenderer sprite;
	private PlayerHealthUI playerHealthUI;
	private ParticleEffectManager particleManager;
	private CameraController cameraController;
	private GameController gameController;
	private AudioManager audioManager;

	private Vector3 nonZeroDirection = new Vector3(5, 0, 0);
	private Vector3 movementInput = new Vector3(0, 0, 0);
	private Vector3 mousePos = new Vector3(0, 0, 0);

	private bool isDashing = false;
	private float dashTime = 0.20f;
	private float dashTimer = 0.0f;

	private bool gotHit = false;
	private float gotHitTime = 0.5f;
	private float gotHitTimer = 0.0f;

	private bool isDying = false;
	private float dyingTime = 1.5f;
	private float dyingTimer = 0.0f;

	private bool isUpgrading = false;
	private float upgradeTime = 1.0f;
	private float upgradeTimer = 0.0f;

	private float HP = 3;
	private float maxHP = 3;

	// Start is called before the first frame update
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		collider2d = GetComponent<PolygonCollider2D>();
		sprite = GetComponent<SpriteRenderer>();
		playerHealthUI = FindObjectOfType<PlayerHealthUI>();
		particleManager = FindObjectOfType<ParticleEffectManager>();
		cameraController = FindObjectOfType<CameraController>();
		gameController = FindObjectOfType<GameController>();
		audioManager = FindObjectOfType<AudioManager>();
	}

	private void Update()
	{
		float xInput = Input.GetAxisRaw("Horizontal");
		float yInput = Input.GetAxisRaw("Vertical");
		movementInput = new Vector3(xInput, yInput, 0);

		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0f;

		if (Input.GetKeyDown(KeyCode.Space) && !isDashing && IsDashingEnabled)
		{
			audioManager.Play("Dash");
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
				sprite.color = DefaultColor;
				gotHitTimer = 0;
			}
		}

		if (isDying)
		{
			rb2d.velocity = Vector2.zero;

			dyingTimer += Time.deltaTime;
			if(dyingTimer > dyingTime)
			{
				isDying = false;
				dyingTimer = 0.0f;
				sprite.color = DefaultColor;

				gameController.OnPlayerDeath();
			}
		}

		if (isUpgrading)
		{
			rb2d.velocity = Vector2.zero;

			upgradeTimer += Time.deltaTime;
			if (upgradeTimer > upgradeTime)
			{
				isUpgrading = false;
				upgradeTimer = 0.0f;

				//text.SetText("");
			}
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
		if (!isDying && !isUpgrading)
		{
			if (movementInput != Vector3.zero)
			{
				nonZeroDirection = movementInput;
			}

			rb2d.velocity = movementInput.normalized * movespeed;

			if (!isDashing)
			{
				rb2d.velocity = movementInput.normalized * movespeed;
			}
			else
			{
				rb2d.velocity = nonZeroDirection * dashSpeed;
			}
		}
	}

	private void SetRotation()
	{
		if (!isDying && !isUpgrading)
		{
			var playerToMouse = new Vector2(mousePos.x, mousePos.y) - rb2d.position;
			float angle = (Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg) - 90f;
			rb2d.rotation = angle;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "Enemy")
		{
			gotHit = true;
			HP -= 1;
			playerHealthUI.DecreasePlayerHealth();
			particleManager.SpawnSqaureParticles(GotHitColor, collision.transform.position);
			cameraController.AddTrauma();
			cameraController.AddTrauma();
			audioManager.Play("Hurt");
		}

		if(collision.transform.tag == "BossBullet")
		{
			var bullet = collision.transform.GetComponent<BossBullet>();
			if (!bullet.GetHasHitAlready() && !gotHit)
			{
				gotHit = true;
				HP -= 1;
				playerHealthUI.DecreasePlayerHealth();
				particleManager.SpawnSqaureParticles(GotHitColor, collision.transform.position);
				cameraController.AddTrauma();
				cameraController.AddTrauma();
				audioManager.Play("Hurt");
			}
			bullet.SetHasHitAlready(true);
		}

		if(HP <= 0)
		{
			isDying = true;
			particleManager.SpawnCircleParticles(GotHitColor, transform.position, 5, 1.0f);
			tip.enabled = false;
			sprite.enabled = false;

			audioManager.Play("Explosion");
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
			sprite.color = DefaultColor;
		}
		else if (gotHitTimer > 0.2f && gotHitTimer < 0.3f)
		{
			sprite.color = GotHitColor;
		}
		else if (gotHitTimer > 0.4f && gotHitTimer < 0.5f)
		{
			sprite.color = DefaultColor;
		}
		else if (gotHitTimer > 0.5)
		{
			sprite.color = GotHitColor;
		}
	}

	public void StartUpgrading()
	{
		isUpgrading = true;
	}

	public void SetVisible()
	{
		tip.enabled = true;
		sprite.enabled = true;
	}

	public void SetAlive()
	{
		isDying = false;
		HP = maxHP;
	}

	public bool GetGotHit()
	{
		return gotHit;
	}
}
