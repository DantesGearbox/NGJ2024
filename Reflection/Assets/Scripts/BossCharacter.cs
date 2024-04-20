using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharacter : MonoBehaviour
{
	public Color defaultColor;
	public Color GotHitColor;

	private SpriteRenderer sprite;
	private Bullet bullet;
	private Rigidbody2D rb2d;

	private float HP = 10;
	private float maxHP = 10;

	private bool gotHit = false;
	private float gotHitTime = 0.5f;
	private float gotHitTimer = 0.0f;

	private CharacterController player;

	public Transform firePoint;
	public GameObject bulletPrefab;

	private bool isShooting = true;
	private float shootTime = 5.0f;
	private float shootTimer = 4.0f;

	private bool isMoving = false;
	private float movementWidth = 13f;
	private float movementHeight = 6.0f;
	public Vector3 goalPosition = Vector3.zero;
	private float movementSpeed = 2.5f;

	// Start is called before the first frame update
	void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
		player = FindObjectOfType<CharacterController>();
		rb2d = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		if (!isMoving)
		{
			float x = Random.Range(0, movementWidth) - (movementWidth/2);
			float y = Random.Range(0, movementHeight) - (movementHeight/2);
			goalPosition = new Vector3(x, y, 0);
			Debug.Log("Goal Position: " + goalPosition);
			isMoving = true;
		}

		if (isMoving)
		{
			Vector3 moveDir = (goalPosition - transform.position).normalized * movementSpeed;
			rb2d.velocity = moveDir;

			if(IsClose(transform.position, goalPosition, 0.1f))
			{
				isMoving = false;
			}
		}

		SetVelocity();
		SetRotation();
	}

	private bool IsClose(Vector3 vector1, Vector3 vector2, float dist)
	{
		return vector1.x > vector2.x - dist && vector1.x < vector2.x + dist &&
			vector1.y > vector2.y - dist && vector1.y < vector2.y + dist;



	}

	// Update is called once per frame
	void Update()
	{
		if (isShooting)
		{
			shootTimer += Time.deltaTime;
			if(shootTimer > shootTime)
			{
				Shoot();
				shootTimer = 0.0f;
			}
		}

		if (gotHit)
		{
			SetColorAfterHit();

			gotHitTimer += Time.deltaTime;
			if(gotHitTimer > gotHitTime)
			{
				gotHit = false;
				sprite.color = defaultColor;
				gotHitTimer = 0;
			}

			Debug.Log("BOSS HP is: " + HP);
		}
	}

	private void SetRotation()
	{
		var playerPos = player.transform.position;
		var enemyToPlayer = new Vector2(playerPos.x, playerPos.y) - rb2d.position;
		float goalAngle = Mathf.Atan2(enemyToPlayer.y, enemyToPlayer.x) * Mathf.Rad2Deg - 90f;

		rb2d.rotation = Mathf.LerpAngle(rb2d.rotation, goalAngle, 0.1f);
	}

	private void SetVelocity()
	{

		//rb2d.velocity = movementInput.normalized * Movespeed;
	
		
	
	}

	private void Shoot()
	{
		GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		bullet.transform.rotation = transform.rotation;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.transform.tag == "Bullet")
		{
			var bullet = collision.transform.GetComponent<Bullet>();

			if (!bullet.GetHasHitAlready() && !gotHit)
			{
				if (bullet != null)
				{
					HP -= bullet.GetDamage();
				}

				gotHit = true;
			}

			bullet.SetHasHitAlready(true);
			this.bullet = bullet;
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
