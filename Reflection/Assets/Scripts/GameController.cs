using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public CharacterController Player;
	public CharacterShooting PlayerShooting;

	public List<BossCharacter> BossCharacters;

	public CameraController CameraController;

	public List<GameObject> Doors;

	public List<Transform> SpawnPoints;

	public Text text1;
	public Text text2;
	public Text text3;
	public Text text4;

	public PlayerHealthUI playerHealthUI;
	public PlayerBulletUI playerBulletUI;
	public BossHealthUI bossHealthUI;

	public GameObject PowerUp0;

	private AudioManager audioManager;

	public List<bool> defeatedBosses;

	void Start()
	{
		audioManager = FindObjectOfType<AudioManager>();
		for(int i = 0; i < BossCharacters.Count; i++)
		{
			defeatedBosses.Add(false);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			bool allDone = true;
			for (int i = 0; i < defeatedBosses.Count; i++)
			{
				if (!defeatedBosses[i])
				{
					allDone = false;
				}
			}

			if (allDone)
			{
				ResetGame();
			}
		}
	}

	private void ResetGame()
	{
		StartCoroutine(LoadYourAsyncScene());
	}

	public void OnUpgrade(int number)
	{
		if(number == 0)
		{
			Doors[0].gameObject.SetActive(false);
			PlayerShooting.IsShootingEnabled = true;
			BetweenScenes.instance.IsShootingEnabled = true;

			Player.StartUpgrading();
			text1.text = "LEFT CLICK";

			playerBulletUI.IncreasePlayerBullets();
			playerHealthUI.ResetHearts();
			Player.SetAlive();

			audioManager.Play("DoorOpen");
			audioManager.Play("PowerUp");
		}

		if(number == 1)
		{
			Doors[4].gameObject.SetActive(false);
			Player.IsDashingEnabled = true;
			BetweenScenes.instance.IsDashingEnabled = true;
			Player.StartUpgrading();
			text2.text = "SPACE BAR";

			playerHealthUI.ResetHearts();
			Player.SetAlive();

			audioManager.Play("DoorOpen");
			audioManager.Play("PowerUp");
		}

		if (number == 2)
		{
			Doors[6].gameObject.SetActive(false);
			PlayerShooting.IsTrippleShotEnabled = true;
			BetweenScenes.instance.IsTrippleShotEnabled = true;
			Player.StartUpgrading();
			text3.text = "TRIPLE SHOT";

			playerHealthUI.ResetHearts();
			Player.SetAlive();

			audioManager.Play("DoorOpen");
			audioManager.Play("PowerUp");
		}
	}

	public void OnEnterBossRoom(int num)
	{
		for(int i = 0; i < BossCharacters.Count; i++)
		{
			if(BossCharacters[i].LevelNumber == num)
			{
				if(!defeatedBosses[i] && !BossCharacters[i].gameObject.activeInHierarchy)
				{
					BossCharacters[i].gameObject.SetActive(true);
					BossCharacters[i].Spawn();
					bossHealthUI.ResetHearts();
				}
			}
		}
	}


	IEnumerator LoadYourAsyncScene()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("BossRoom");

		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}

	public void OnPlayerDeath()
	{
		StartCoroutine(LoadYourAsyncScene());
	}

	public void OnBossDeath(int number)
	{
		if (number == 0)
		{
			Debug.LogError("ERROR");
		}

		if (number == 1)
		{
			Doors[1].SetActive(false);
			defeatedBosses[0] = true;
		}

		int levelNum = 0;
		for (int i = 0; i < BossCharacters.Count; i++)
		{
			if (BossCharacters[i].BossNumber == number)
			{
				levelNum = BossCharacters[i].LevelNumber;
				defeatedBosses[i] = true;
			}
		}

		List<int> bossesToDie = new List<int>();
		for (int i = 0; i < BossCharacters.Count; i++)
		{
			if (BossCharacters[i].LevelNumber == levelNum)
			{
				bossesToDie.Add(i);
			}
		}

		if(bossesToDie.Count > 1)
		{
			bool allDead = true;
			for(int i = 0; i < bossesToDie.Count; i++)
			{
				if (!defeatedBosses[bossesToDie[i]])
				{
					allDead = false;
				}
			}

			if (allDead)
			{
				if(levelNum == 8)
				{
					text4.text = "GAME WON - PRESS 'R' TO RESET";
				} 
				else
				{
					Doors[levelNum].gameObject.SetActive(false);
				}
				
			}
		}
	}
}
