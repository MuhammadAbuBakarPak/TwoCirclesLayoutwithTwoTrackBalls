using UnityEngine;

public class CursorCollider : MonoBehaviour
{

    // Reference to the Hive script.
    private Hive hiveScript;

    private Color selectedColor = new Color(0.055f, 0.561f, 0.243f);
    private Color originalColor = new Color(0.08891062f, 0.1522242f, 0.1698113f);

    private void Start()
    {
        // Get the Hive script.
        hiveScript = FindObjectOfType<Hive>();
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RightHiveButton")
        {
            hiveScript.ChangeButtonColor("RightHiveButton", selectedColor);
        }
        else if (other.gameObject.tag == "LeftHiveButton")
        {
            hiveScript.ChangeButtonColor("LeftHiveButton", selectedColor);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "RightHiveButton")
        {
            hiveScript.ChangeButtonColor("RightHiveButton", originalColor);
        }
        else if (other.gameObject.tag == "LeftHiveButton")
        {
            hiveScript.ChangeButtonColor("LeftHiveButton", originalColor);
        }
    }






















    /*
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
    */
}
