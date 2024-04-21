using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
	private float bulletVelocity = 20;

	private Rigidbody2D rb2d;
	private AudioManager audioManager;

	private bool hasHitAlready = false;
	private float damage = 1;

	// Start is called before the first frame update
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.velocity = Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * new Vector3(0, bulletVelocity, 0);

		audioManager = FindObjectOfType<AudioManager>();
	}

	public bool GetHasHitAlready()
	{
		return hasHitAlready;
	}

	public void SetHasHitAlready(bool hit)
	{
		hasHitAlready = hit;
	}

	public float GetDamage()
	{
		return damage;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "BossBullet" || collision.transform.tag == "Walls" || collision.transform.tag == "Bullet")
		{
			audioManager.Play("Pep");
			hasHitAlready = true;
		}
	}
}
