using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{
	public Color ParticleColor;
	public int UpgradeNumber = 0;

	private ParticleEffectManager particleManager;
	private GameController gameController;


	// Start is called before the first frame update
	void Start()
	{
		particleManager = FindObjectOfType<ParticleEffectManager>();
		gameController = FindObjectOfType<GameController>();
	}

	// Update is called once per frame
	void Update()
	{
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 0.1f));

		float scale = (Mathf.Cos(Time.time) + 4) / 8;
		transform.localScale = new Vector3(scale, scale, 1);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			particleManager.SpawnCircleParticlesInward(ParticleColor, transform.position, 20, 2.5f);
			gameController.OnUpgrade(UpgradeNumber);

			GetComponent<SpriteRenderer>().enabled = false;
			gameObject.SetActive(false);
		}
	}
}
