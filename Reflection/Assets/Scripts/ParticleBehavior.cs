using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehavior : MonoBehaviour
{
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 0.5f));

        timer += Time.deltaTime;
        float scale = Mathf.Lerp(0.25f, 0, timer);
        if(timer > 1) { timer = 0.0f; }

        transform.localScale = new Vector3(scale, scale, 1);

        if(scale <= 0)
		{
            Destroy(gameObject);
		}
    }
}
