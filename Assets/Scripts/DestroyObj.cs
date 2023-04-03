using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObj : MonoBehaviour
{
    [SerializeField] private float time;

    public DestroyObj() 
    {
        time = 0f;
    }

    public DestroyObj(float time)
    {
        this.time = time;
    }

    private void Start()
    {
        Destroy(gameObject, time);
    }
}
