using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCursorCollider : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LeftHiveButton")
        {
            Debug.Log("Left Hive Button Selected");
        }
    }
}
