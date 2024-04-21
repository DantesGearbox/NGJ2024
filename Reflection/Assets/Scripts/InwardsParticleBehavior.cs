using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InwardsParticleBehavior : MonoBehaviour
{
	private CharacterController player;
	private Rigidbody2D rb2d;

	float timer = 0;
	float size = 0.25f;

	// Start is called before the first frame update
	void Start()
	{
		player = FindObjectOfType<CharacterController>();
		rb2d = GetComponent<Rigidbody2D>();

		size = Random.Range(0.25f, 0.5f);
		transform.localScale = new Vector3(size, size, 1);
	}

	// Update is called once per frame
	void Update()
	{
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 0.5f));

		timer += Time.deltaTime;

		size -= 0.1f * Time.deltaTime;
		size = Mathf.Clamp(size, 0, 1.5f);
		transform.localScale = new Vector3(size, size, 1);

		Vector3 dir = player.transform.position - transform.position;
		rb2d.velocity = dir.normalized * Random.Range(2f, 5f);

		if (timer > 1)
		{
			Destroy(gameObject);
		}
	}
}
