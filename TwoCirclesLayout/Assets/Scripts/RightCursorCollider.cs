using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RightCursorCollider : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RightHiveButton")
        {
            Debug.Log("Right Hive Button Selected");          
        }
    }
}
