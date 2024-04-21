using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharacter : MonoBehaviour
{
	public Color DefaultColor;
	public Color GotHitColor;

	public SpriteRenderer tip;

	public bool TrippleShotActive = true;
	public bool DashActive = true;

	public int BossNumber = 0;

	private SpriteRenderer sprite;
	private Rigidbody2D rb2d;

	private float HP = 3;
	private float maxHP = 3;

	private bool gotHit = false;
	private float gotHitTime = 0.5f;
	private float gotHitTimer = 0.0f;

	private CharacterController player;
	private BossHealthUI bossHealthUI;
	private BossBulletUI bossBulletUI;
	private ParticleEffectManager particleManager;
	private CameraController cameraController;
	private GameController gameController;
	private AudioManager audioManager;

	public Transform firePoint1;
	public Transform firePoint2;
	public Transform firePoint3;
	public GameObject bulletPrefab;

	public GameObject UpgradePrefab;

	private bool isShooting = true;
	private float shootTime = 3.0f;
	private float shootTimer = 2.0f;

	private bool isMoving = false;
	private float movementWidth = 13f;
	private float movementHeight = 6.0f;
	public Vector3 goalPosition = Vector3.zero;
	private float movementSpeed = 2.5f;
	private float movementMaxTime = 5f;
	private float movementMaxTimer = 0.0f;

	private bool isDashing = false;
	private float dashTime = 0.30f;
	private float dashTimer = 0.0f;
	private bool isDashOnCooldown = false;
	private float dashCooldownTime = 3f;
	private float dashCooldownTimer = 0f;
	private float dashSpeed = 10f;
	private float dashAggroLength = 5f;

	private bool isDying = false;
	private float dyingTime = 1.5f;
	private float dyingTimer = 0.0f;

	private bool isSpawning = false;
	private float spawnTime = 1.0f;
	private float spawnTimer = 0.0f;

	// Start is called before the first frame update
	void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
		player = FindObjectOfType<CharacterController>();
		rb2d = GetComponent<Rigidbody2D>();

		bossHealthUI = FindObjectOfType<BossHealthUI>();
		bossBulletUI = FindObjectOfType<BossBulletUI>();
		particleManager = FindObjectOfType<ParticleEffectManager>();
		particleManager.SpawnCircleParticles(GotHitColor, transform.position, 5, 1.0f);
		cameraController = FindObjectOfType<CameraController>();
		gameController = FindObjectOfType<GameController>();
		audioManager = FindObjectOfType<AudioManager>();
		audioManager.Play("Explosion");
	}

	void FixedUpdate()
	{
		if (isSpawning || isDying)
		{
			rb2d.velocity = Vector2.zero;
			return;
		}

		if (!isDashing)
		{
			if (!isMoving)
			{
				float x = Random.Range(0, movementWidth) - (movementWidth/2);
				float y = Random.Range(0, movementHeight) - (movementHeight/2);

				if(BossNumber == 1)
				{
					y += 11f;
				}
				if (BossNumber == 2)
				{
					y += 22f;
				}
				if (BossNumber == 3)
				{
					y += 33f;
				}

				goalPosition = new Vector3(x, y, 0);
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

				movementMaxTimer += Time.deltaTime;
				if(movementMaxTimer > movementMaxTime)
				{
					isMoving = false;
					movementMaxTimer = 0.0f;
				}
			}
		}

		if (isDashing)
		{
			Vector3 moveDir = (player.transform.position - transform.position).normalized * dashSpeed;
			rb2d.velocity = moveDir;
		}

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
		if (DistanceToPlayer() < dashAggroLength && !isDashOnCooldown && DashActive && !isDying && !isSpawning)
		{
			audioManager.Play("Dash");
			isDashing = true;
		}

		if (isDashing && !isDying && !isSpawning)
		{
			dashTimer += Time.deltaTime;
			if(dashTimer > dashTime)
			{
				isDashing = false;
				dashTimer = 0.0f;
				isDashOnCooldown = true;
			}
		}

		if (isDashOnCooldown)
		{
			dashCooldownTimer += Time.deltaTime;
			if(dashCooldownTimer > dashCooldownTime)
			{
				isDashOnCooldown = false;
				dashCooldownTimer = 0.0f;
			}
		}

		if (isShooting && !isDying && !isSpawning)
		{
			if (!gotHit)
			{
				sprite.color = Color.Lerp(GotHitColor, DefaultColor, shootTimer / shootTime);
			}

			shootTimer += Time.deltaTime;
			if(shootTimer > shootTime)
			{
				Shoot();
				bossBulletUI.DecreaseBossBullets();
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
				sprite.color = DefaultColor;
				gotHitTimer = 0;
			}
		}

		if (isSpawning)
		{
			spawnTimer += Time.deltaTime;
			if (spawnTimer > spawnTime)
			{
				isSpawning = false;
				spawnTimer = 0.0f;
			}
		}
	}

	private float DistanceToPlayer()
	{
		return (transform.position - player.transform.position).magnitude;
	}

	private void SetRotation()
	{
		if (!isDying && !isSpawning)
		{
			var playerPos = player.transform.position;
			var enemyToPlayer = new Vector2(playerPos.x, playerPos.y) - rb2d.position;
			float goalAngle = Mathf.Atan2(enemyToPlayer.y, enemyToPlayer.x) * Mathf.Rad2Deg - 90f;

			rb2d.rotation = Mathf.LerpAngle(rb2d.rotation, goalAngle, 0.1f);
		}
	}

	private void Shoot()
	{
		if (!TrippleShotActive)
		{
			GameObject bullet = Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
			bullet.transform.rotation = transform.rotation;
			cameraController.AddTrauma();
			audioManager.Play("Shoot");
		}

		if (TrippleShotActive)
		{
			GameObject bullet1 = Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);

			var rotation2 = Quaternion.Euler(firePoint2.rotation.eulerAngles + new Vector3(0, 0, -15));
			GameObject bullet2 = Instantiate(bulletPrefab, firePoint2.position, rotation2);

			var rotation3 = Quaternion.Euler(firePoint3.rotation.eulerAngles + new Vector3(0, 0, 15));
			GameObject bullet3 = Instantiate(bulletPrefab, firePoint3.position, rotation3);

			cameraController.AddTrauma(); 
			cameraController.AddTrauma();

			audioManager.Play("Shoot");
			audioManager.Play("Shoot");

			bullet1.transform.rotation = transform.rotation;
		}

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (isSpawning)
		{
			return;
		}

		if(collision.transform.tag == "Bullet")
		{
			var bullet = collision.transform.GetComponent<Bullet>();

			if (!bullet.GetHasHitAlready() && !gotHit)
			{
				if (bullet != null)
				{
					HP -= bullet.GetDamage();
					bossHealthUI.DecreaseBossHealth();
				}

				audioManager.Play("Hurt");

				cameraController.AddTrauma();
				cameraController.AddTrauma();
				particleManager.SpawnSqaureParticles(GotHitColor, collision.transform.position);
				gotHit = true;
			}

			bullet.SetHasHitAlready(true);

			if (HP <= 0)
			{
				isDying = true;
				particleManager.SpawnCircleParticles(GotHitColor, transform.position, 5, 1.0f);
				
				if(UpgradePrefab != null)
				{
					Instantiate(UpgradePrefab, transform.position, transform.rotation);
				}

				audioManager.Play("Explosion");

				gameController.OnBossDeath(BossNumber);
				gameObject.SetActive(false);
			}
		}
	}

	public void Spawn()
	{
		isSpawning = true;
		if(particleManager != null)
		{
			particleManager.SpawnCircleParticles(GotHitColor, transform.position, 5, 1.0f);
		}
		if(audioManager != null)
		{
			audioManager.Play("Explosion");
		}

		HP = maxHP;

		isDying = false;
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
}
