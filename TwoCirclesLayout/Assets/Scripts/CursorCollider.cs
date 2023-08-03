using UnityEngine;

public class CursorCollider : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RightHiveButton")
        { 
           Debug.Log("Event Triggred right");
        }
        else if (other.gameObject.tag == "LeftHiveButton")
        {
            Debug.Log("Event Triggred Left");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "RightHiveButton")
        {
            Debug.Log("Triggred Exited right");
        }
        else if (other.gameObject.tag == "LeftHiveButton")
        {
            Debug.Log("Triggred Exited Left");
        }
    }
}
