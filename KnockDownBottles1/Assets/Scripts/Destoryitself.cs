using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destoryitself : MonoBehaviour
{
    private void Start()
    {
        Destory_aftersometime();
    }
    public void Destory_aftersometime()
    {
        Destroy(gameObject, 5);
    }
}
