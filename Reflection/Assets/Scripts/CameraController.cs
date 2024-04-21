using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraController : MonoBehaviour
{
	public Camera currentCamera;

	public List<Transform> cameraPositions;

	private float trauma;
	private Vector2 goalPosition = Vector2.zero;
	private Vector3 prevPos = Vector3.zero;

	private CharacterController player;
	private GameController gameController;
	private PixelPerfectCamera pixelPerfectCamera;

	// Start is called before the first frame update
	void Start()
	{
		goalPosition = currentCamera.transform.position;

		player = FindObjectOfType<CharacterController>();
		gameController = FindObjectOfType<GameController>();
		pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
	}

	// Update is called once per frame
	void Update()
	{
		trauma -= 0.016f;
		if(trauma < 0)
		{
			trauma = 0;
		}

		float offsetX = 1 * (trauma * trauma) * Random.Range(-1.0f, 1.0f);
		float offsetY = 1 * (trauma * trauma) * Random.Range(-1.0f, 1.0f);

		goalPosition = Vector2.Lerp(transform.position, goalPosition, 0.25f);

		transform.position = new Vector3(goalPosition.x, goalPosition.y, -10) + new Vector3(offsetX, offsetY, 0);
	}

	private void FixedUpdate()
	{
		Vector3 pos = FindClosestPos();
		goalPosition = new Vector2(pos.x, pos.y);

		if(pos == cameraPositions[1].position && pos != prevPos)
		{
			gameController.OnEnterBossRoom1();
		}

		if (pos == cameraPositions[2].position && pos != prevPos)
		{
			gameController.OnEnterBossRoom2();
		}

		if (pos == cameraPositions[3].position && pos != prevPos)
		{
			gameController.OnEnterBossRoom3();
		}

		prevPos = pos;
	}

	private Vector3 FindClosestPos()
	{
		float dist = float.MaxValue;
		Vector3 res = Vector3.zero;
		foreach (Transform t in cameraPositions)
		{
			float d = (t.position - player.transform.position).magnitude;
			if (d < dist)
			{
				dist = d;
				res = t.position;
			}
		}

		return res;
	}

	public void AddTrauma()
	{
		trauma += 0.25f;
	}

	public void AddLargeTrauma()
	{
		trauma += 0.5f;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("Enter");
	}
}
