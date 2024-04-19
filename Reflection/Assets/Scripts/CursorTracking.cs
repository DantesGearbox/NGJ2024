using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTracking : MonoBehaviour
{
    void FixedUpdate()
    {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        transform.position = mouseWorldPos;
    }
}
