using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
	public List<RectTransform> PlayerHearts;
	private int amountOfHearts;
	private int maxHearts = 3;

	// Start is called before the first frame update
	void Start()
	{
		amountOfHearts = PlayerHearts.Count;
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void DecreasePlayerHealth()
	{
		if(amountOfHearts > 0)
		{
			PlayerHearts[amountOfHearts - 1].GetComponent<Image>().enabled = false;
			amountOfHearts -= 1;
		}
	}

	public void ResetHearts()
	{
		amountOfHearts = maxHearts;
		for(int i = 0; i < maxHearts; i++)
		{
			PlayerHearts[i].GetComponent<Image>().enabled = true;
		}
	}
}
