using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletUI : MonoBehaviour
{
    public List<RectTransform> playerBullets;
    private int amountOfBullets;

    // Start is called before the first frame update
    void Start()
    {
        amountOfBullets = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecreasePlayerBullets()
    {
        if(amountOfBullets != 0)
		{
            playerBullets[amountOfBullets - 1].transform.gameObject.SetActive(false);
            amountOfBullets -= 1;
		}
    }

    public void IncreasePlayerBullets()
    {
        playerBullets[amountOfBullets].transform.gameObject.SetActive(true);
        amountOfBullets += 1;
    }
}
