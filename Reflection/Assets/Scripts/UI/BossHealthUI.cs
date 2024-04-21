using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public List<RectTransform> BossHearts;
    private int amountOfHearts;
    private int maxHearts = 3;

    // Start is called before the first frame update
    void Start()
    {
        amountOfHearts = BossHearts.Count;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DecreaseBossHealth()
    {
  //      if(amountOfHearts > 0)
		//{
  //          BossHearts[amountOfHearts - 1].GetComponent<Image>().enabled = false;
  //          amountOfHearts -= 1;
		//}
    }

    public void ResetHearts()
    {
        //amountOfHearts = maxHearts;
        //for (int i = 0; i < maxHearts; i++)
        //{
        //    BossHearts[i].GetComponent<Image>().enabled = true;
        //}
    }
}
