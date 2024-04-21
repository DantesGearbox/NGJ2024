using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletUI : MonoBehaviour
{
    public List<RectTransform> BossBullets;
    private int amountOfBullets;

    // Start is called before the first frame update
    void Start()
    {
        amountOfBullets = BossBullets.Count;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DecreaseBossBullets()
    {
        //BossBullets[amountOfBullets - 1].transform.gameObject.SetActive(false);
        //amountOfBullets -= 1;
    }

    public void IncreaseBossBullets()
    {
        //BossBullets[amountOfBullets].transform.gameObject.SetActive(true);
        //amountOfBullets += 1;
    }
}
