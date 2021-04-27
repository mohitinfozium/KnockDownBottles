using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastRotation : MonoBehaviour
{
    float rotationsPerMinute =10f;
    void  Update()
    {
        transform.Rotate(0, 0, 15.0f * rotationsPerMinute * Time.deltaTime);
    }
}
