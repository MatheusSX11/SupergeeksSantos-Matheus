using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 5;
    public Vector3 rotationAxis = Vector3.up;
    void Update()
    {
        transform.Rotate(rotationAxis * speed * Time.deltaTime);
    }
}
