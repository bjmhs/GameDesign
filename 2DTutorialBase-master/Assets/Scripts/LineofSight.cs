﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineofSight : MonoBehaviour
{
    //Called when something enters the triigger collider
    private void OnTriggerEnter2D(Collider2D coll)
    {
        // Check if coll is the player
        if (coll.CompareTag("Player"))
        {
            GetComponentInParent<Enemy>().player = coll.transform;
            Debug.Log("See player run at player");
        }
    }
}
