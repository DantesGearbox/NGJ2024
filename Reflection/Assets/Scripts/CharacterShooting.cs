using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
	public Transform firePoint;
	public GameObject bulletPrefab;

	private bool hasShot;
	private float shootTime = 5.0f;
	private float shootTimer = 0;

	private void Start()
	{
		//rb2d = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		if (hasShot)
		{
			shootTimer += Time.deltaTime;
			if(shootTimer > shootTime)
			{
				shootTimer = 0;
				hasShot = false;
			}
		}

		if (Input.GetKeyDown(KeyCode.Mouse0) && !hasShot)
		{
			hasShot = true;
			Shoot();
		}
	}

	private void Shoot()
	{
		GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		bullet.transform.rotation = transform.rotation;
	}
}
