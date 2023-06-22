using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyByTime : MonoBehaviour
{
    [SerializeField]
    private float destroyTime;

    private void Awake()
    {
        Destroy(gameObject, destroyTime);
    }
}
