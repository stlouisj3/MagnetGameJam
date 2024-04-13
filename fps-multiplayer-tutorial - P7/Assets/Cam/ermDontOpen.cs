using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ermDontOpen : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;
    public float radius = 1f;
    public float angle = 0f;
    public float spinMagnifier = 0f;

    // Update is called once per frame
    void Update()
    {
        float x = target.position.x + Mathf.Cos(angle) * radius;
        float y = target.position.y;
        float z = target.position.z + Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x, y, z);

        angle += speed * Time.deltaTime;

        transform.LookAt(target);
    }
}
