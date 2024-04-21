using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBulletRotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 0.1f));

        float scaleX = Mathf.Lerp(0.20f, 0.30f, (Mathf.Cos(Time.time) + 1) / 2);
        float scaleY = Mathf.Lerp(0.40f, 0.60f, (Mathf.Cos(Time.time) + 1) / 2);
        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
}
