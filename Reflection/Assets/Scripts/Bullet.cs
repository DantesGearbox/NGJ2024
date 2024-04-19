using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float bulletVelocity = 18;

	private Rigidbody2D rb2d;

	// Start is called before the first frame update
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.velocity = Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * new Vector3(0, bulletVelocity, 0);
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
