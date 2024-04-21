using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
	public Color DefaultColor;
	public Color GotHitColor;
	public Color TipColor;

	public bool IsShootingEnabled = false;
	public bool IsTrippleShotEnabled = false;

	public Transform firePoint1;
	public Transform firePoint2;
	public Transform firePoint3;
	public GameObject bulletPrefab;

	public SpriteRenderer tip;

	private PlayerBulletUI playerBulletUI;
	private SpriteRenderer sprite;
	private CharacterController player;
	private CameraController cameraController;
	private AudioManager audioManager;
	private ParticleEffectManager particleEffectManager;

	private bool hasShotAlready;
	private float shootTime = 3.0f;
	private float shootTimer = 0;

	private void Start()
	{
		//rb2d = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
		playerBulletUI = FindObjectOfType<PlayerBulletUI>();
		player = GetComponent<CharacterController>();
		cameraController = FindObjectOfType<CameraController>();
		audioManager = FindObjectOfType<AudioManager>();
		particleEffectManager = FindObjectOfType<ParticleEffectManager>();
	}

	// Update is called once per frame
	void Update()
	{
		if (hasShotAlready)
		{
			if (!player.GetGotHit() && IsShootingEnabled)
			{
				sprite.color = Color.Lerp(GotHitColor, DefaultColor, shootTimer / shootTime);
				tip.color = Color.Lerp(GotHitColor, DefaultColor, shootTimer / shootTime);
			}
			shootTimer += Time.deltaTime;

			if(shootTimer > 2.9f)
			{
				sprite.color = GotHitColor;
				tip.color = GotHitColor;
			}

			if(shootTimer > shootTime)
			{
				shootTimer = 0;
				hasShotAlready = false;
				if (IsShootingEnabled)
				{
					playerBulletUI.IncreasePlayerBullets();
					tip.color = TipColor;
					sprite.color = DefaultColor;
					audioManager.Play("Ping");

					particleEffectManager.SpawnSqaureParticles(GotHitColor, transform.position, 4, 7, 1.0f);
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.Mouse0) && !hasShotAlready && IsShootingEnabled)
		{
			hasShotAlready = true;
			playerBulletUI.DecreasePlayerBullets();
			Shoot();
		}
	}

	private void Shoot()
	{
		if (!IsTrippleShotEnabled && IsShootingEnabled)
		{
			GameObject bullet = Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
			bullet.transform.rotation = transform.rotation;

			audioManager.Play("Shoot");
			cameraController.AddTrauma();
		}

		if (IsTrippleShotEnabled)
		{
			GameObject bullet1 = Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);

			var rotation2 = Quaternion.Euler(firePoint2.rotation.eulerAngles + new Vector3(0, 0, -15));
			GameObject bullet2 = Instantiate(bulletPrefab, firePoint2.position, rotation2);

			var rotation3 = Quaternion.Euler(firePoint3.rotation.eulerAngles + new Vector3(0, 0, 15));
			GameObject bullet3 = Instantiate(bulletPrefab, firePoint3.position, rotation3);

			cameraController.AddTrauma();
			cameraController.AddTrauma();

			audioManager.Play("ShootBig");

			bullet1.transform.rotation = transform.rotation;
		}
	}
}
