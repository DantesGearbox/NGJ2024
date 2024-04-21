using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleParticleBehavior : MonoBehaviour
{
	float timer = 0;
	float size = 0.25f;

	// Start is called before the first frame update
	void Start()
	{
		size = Random.Range(0.25f, 1.5f);
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

		if (timer > 1) {
			Destroy(gameObject);
		}
	}
}
