using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    float rotationsPerMinute =10f;
 void  Update()
    {
        transform.Rotate(0, 0, 6.0f * rotationsPerMinute * Time.deltaTime);
    }
}
