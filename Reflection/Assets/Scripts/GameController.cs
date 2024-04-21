using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public CharacterController player;
	public CharacterShooting playerShooting;

	public BossCharacter BossCharacter1;
	public BossCharacter BossCharacter2;
	public BossCharacter BossCharacter3;

	public CameraController CameraController;

	public GameObject Door0;
	public GameObject Door1;
	public GameObject Door2;
	public GameObject Door3;

	public Transform spawnPoint1;
	public Transform spawnPoint2;
	public Transform spawnPoint3;
	public Transform spawnPoint4;

	public Text text1;
	public Text text2;
	public Text text3;
	public Text text4;

	public PlayerHealthUI playerHealthUI;
	public PlayerBulletUI playerBulletUI;
	public BossHealthUI bossHealthUI;

	public GameObject PowerUp0;

	private AudioManager audioManager;

	private bool defeatedBoss1 = false;
	private bool defeatedBoss2 = false;
	private bool defeatedBoss3 = false;

	// Start is called before the first frame update
	void Start()
	{
		audioManager = FindObjectOfType<AudioManager>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R) && defeatedBoss3)
		{
			ResetGame();
		}
	}

	private void ResetGame()
	{
		defeatedBoss1 = false;
		defeatedBoss2 = false;
		defeatedBoss3 = false;

		player.transform.position = spawnPoint1.position + new Vector3(0, -3.5f, 10);

		BossCharacter1.transform.position = spawnPoint2.position + new Vector3(0, 0, 10);
		BossCharacter1.transform.rotation = Quaternion.Euler(0, 0, 180);
		BossCharacter1.gameObject.SetActive(false);

		BossCharacter2.transform.position = spawnPoint3.position + new Vector3(0, 0, 10);
		BossCharacter2.transform.rotation = Quaternion.Euler(0, 0, 180);
		BossCharacter2.gameObject.SetActive(false);

		BossCharacter3.transform.position = spawnPoint4.position + new Vector3(0, 0, 10);
		BossCharacter3.transform.rotation = Quaternion.Euler(0, 0, 180);
		BossCharacter3.gameObject.SetActive(false);

		bossHealthUI.DecreaseBossHealth();
		bossHealthUI.DecreaseBossHealth();
		bossHealthUI.DecreaseBossHealth();
		playerHealthUI.ResetHearts();
		playerBulletUI.DecreasePlayerBullets();

		player.SetVisible();
		player.SetAlive();

		text1.text = "";
		text2.text = "";
		text3.text = "";
		text4.text = "";

		Door0.SetActive(true);
		Door1.SetActive(true);
		Door2.SetActive(true);
		Door3.SetActive(true);

		var bullets = FindObjectsOfType<Bullet>();
		foreach (var item in bullets)
		{
			Destroy(item.gameObject);
		}

		var bossBullets = FindObjectsOfType<BossBullet>();
		foreach (var item in bossBullets)
		{
			Destroy(item.gameObject);
		}

		Instantiate(PowerUp0);

		playerShooting.IsShootingEnabled = false;
		player.IsDashingEnabled = false;
		playerShooting.IsTrippleShotEnabled = false;
	}

	public void OnUpgrade(int number)
	{
		if(number == 0)
		{
			Door0.SetActive(false);
			playerShooting.IsShootingEnabled = true;
			player.StartUpgrading();
			text1.text = "LEFT CLICK";

			playerBulletUI.IncreasePlayerBullets();
			playerHealthUI.ResetHearts();
			player.SetAlive();

			audioManager.Play("DoorOpen");
			audioManager.Play("PowerUp");
		}

		if(number == 1)
		{
			Door1.SetActive(false);
			player.IsDashingEnabled = true;
			player.StartUpgrading();
			text2.text = "SPACE BAR";

			playerHealthUI.ResetHearts();
			player.SetAlive();

			audioManager.Play("DoorOpen");
			audioManager.Play("PowerUp");
		}

		if (number == 2)
		{
			Door2.SetActive(false);
			playerShooting.IsTrippleShotEnabled = true;
			player.StartUpgrading();
			text3.text = "LEFT CLICK";

			playerHealthUI.ResetHearts();
			player.SetAlive();

			audioManager.Play("DoorOpen");
			audioManager.Play("PowerUp");
		}
	}

	public void OnEnterBossRoom1()
	{
		if (!defeatedBoss1 && !BossCharacter1.gameObject.activeInHierarchy)
		{
			BossCharacter1.gameObject.SetActive(true);
			BossCharacter1.Spawn();
			bossHealthUI.ResetHearts();
		}
	}

	public void OnEnterBossRoom2()
	{
		if (!defeatedBoss2 && !BossCharacter2.gameObject.activeInHierarchy)
		{
			BossCharacter2.gameObject.SetActive(true);
			BossCharacter2.Spawn();
			bossHealthUI.ResetHearts();
		}
	}

	public void OnEnterBossRoom3()
	{
		if (!defeatedBoss3 && !BossCharacter3.gameObject.activeInHierarchy)
		{
			BossCharacter3.gameObject.SetActive(true);
			BossCharacter3.Spawn();
			bossHealthUI.ResetHearts();
		}
	}

	public void OnPlayerDeath()
	{
		if (BossCharacter1.gameObject.activeSelf)
		{
			player.transform.position = spawnPoint1.position + new Vector3(0,0,10);
		}
		if (BossCharacter2.gameObject.activeSelf)
		{
			player.transform.position = spawnPoint2.position + new Vector3(0, 0, 10);
		}
		if (BossCharacter3.gameObject.activeSelf)
		{
			player.transform.position = spawnPoint3.position + new Vector3(0, 0, 10);
		}

		BossCharacter1.transform.position = spawnPoint2.position + new Vector3(0, 0, 10);
		BossCharacter1.transform.rotation = Quaternion.Euler(0, 0, 180);
		BossCharacter1.gameObject.SetActive(false);

		BossCharacter2.transform.position = spawnPoint3.position + new Vector3(0, 0, 10);
		BossCharacter2.transform.rotation = Quaternion.Euler(0, 0, 180);
		BossCharacter2.gameObject.SetActive(false);

		BossCharacter3.transform.position = spawnPoint4.position + new Vector3(0, 0, 10);
		BossCharacter3.transform.rotation = Quaternion.Euler(0, 0, 180);
		BossCharacter3.gameObject.SetActive(false);

		bossHealthUI.DecreaseBossHealth();
		bossHealthUI.DecreaseBossHealth();
		bossHealthUI.DecreaseBossHealth();
		playerHealthUI.ResetHearts();

		player.SetVisible();
		player.SetAlive();
	}

	public void OnBossDeath(int number)
	{
		if(number == 0)
		{
			Debug.LogError("ERROR");
		}

		if(number == 1)
		{
			//Door1.SetActive(false);
			//BossCharacter2.gameObject.SetActive(true);
			defeatedBoss1 = true;
		}

		if (number == 2)
		{
			//Door2.SetActive(false);
			//BossCharacter3.gameObject.SetActive(true);
			defeatedBoss2 = true;
			
		}

		if (number == 3)
		{
			text4.text = "GAME WON - 'R' TO RESET";
			defeatedBoss3 = true;
		}
	}
}
