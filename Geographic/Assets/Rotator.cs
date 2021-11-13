using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 _roation;

    private void Update()
    {
        transform.rotation *= Quaternion.Euler((_roation * Time.deltaTime));
    }
}
