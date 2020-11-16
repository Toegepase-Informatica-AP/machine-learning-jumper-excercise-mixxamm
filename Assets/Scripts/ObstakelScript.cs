﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstakelScript : MonoBehaviour
{
    public float zSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float newZPos = transform.localPosition.z - (Time.deltaTime * zSpeed);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, newZPos);
    }
}
