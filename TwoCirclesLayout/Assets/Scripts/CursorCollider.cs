using UnityEngine;

public class CursorCollider : MonoBehaviour
{
    private Hive hiveScript;

    private Color selectedColor = new Color(0.055f, 0.561f, 0.243f);
    private Color originalColor = new Color(0.08891062f, 0.1522242f, 0.1698113f);

    private void Start()
    {
        hiveScript = FindObjectOfType<Hive>();
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RightHiveButton")
        {
            hiveScript.SetButtonColor( selectedColor, "RightHiveButton");
        }
        else if (other.gameObject.tag == "LeftHiveButton")
        {
            hiveScript.SetButtonColor( selectedColor, "LeftHiveButton");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "RightHiveButton")
        {
            hiveScript.SetButtonColor(originalColor, "RightHiveButton");
        }
        else if (other.gameObject.tag == "LeftHiveButton")
        {
            hiveScript.SetButtonColor(originalColor, "LeftHiveButton");
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
