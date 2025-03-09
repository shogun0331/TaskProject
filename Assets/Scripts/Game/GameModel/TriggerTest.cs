using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject);
    }
 
}
