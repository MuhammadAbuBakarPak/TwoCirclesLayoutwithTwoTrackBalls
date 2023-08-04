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
            hiveScript.SetButtonColor(selectedColor, other.gameObject);
            hiveScript.selectedButtonR = other.gameObject;
        }
        else if (other.gameObject.tag == "LeftHiveButton")
        {
            hiveScript.SetButtonColor(selectedColor, other.gameObject);
            hiveScript.selectedButtonL = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == hiveScript.selectedButtonR)
        {
            hiveScript.SetButtonColor(originalColor, hiveScript.selectedButtonR);
            hiveScript.selectedButtonR = null;
        }
        else if (other.gameObject == hiveScript.selectedButtonL)
        {
            hiveScript.SetButtonColor(originalColor, hiveScript.selectedButtonL);
            hiveScript.selectedButtonL = null;
        }
    }

}
