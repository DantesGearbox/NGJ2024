using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BetweenScenes : MonoBehaviour
{
    public static BetweenScenes instance;

	public bool IsShootingEnabled = false;
	public bool IsDashingEnabled = false;
	public bool IsTrippleShotEnabled = false;

	private void Awake()
	{
		if(instance == null)
		{
            instance = this;
		}
        else
		{
            Destroy(gameObject);
            return;
		}

        DontDestroyOnLoad(gameObject);
	}

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var player = FindObjectOfType<CharacterController>();
        var playerShooter = FindObjectOfType<CharacterShooting>();

        if(player != null)
		{
            player.IsDashingEnabled = IsDashingEnabled;
		}

        if(playerShooter != null)
		{
            playerShooter.IsShootingEnabled = IsShootingEnabled;
            playerShooter.IsTrippleShotEnabled = IsTrippleShotEnabled;
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
