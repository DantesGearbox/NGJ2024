using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotator : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 0.1f));

		float scale = (Mathf.Cos(Time.time) + 6) / 6;
		transform.localScale = new Vector3(scale, scale, 1);
	}
}
