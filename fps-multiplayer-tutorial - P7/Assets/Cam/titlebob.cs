using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titlebob : MonoBehaviour
{
    private Vector3 startPosition;

    [SerializeField] private float frequency = 2f;
    [SerializeField] private float magnitude = .2f;
    [SerializeField] private float offset = 0f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = startPosition + transform.localScale * Mathf.Sin(Time.time * frequency + offset) * magnitude;

    }
}
