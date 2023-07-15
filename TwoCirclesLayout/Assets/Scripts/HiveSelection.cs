using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class HiveSelection : MonoBehaviour
{
	public enum Keyname
	{
		KeyA, KeyB, KeyC, KeyD, KeyE, KeyF, KeyG, KeyH, KeyI, KeyJ, KeyK, KeyL, KeyM, KeyN, KeyO, KeyP, KeyQ, KeyR, KeyS
	}

	public TMP_InputField inputField;
	public TextMeshProUGUI textField;
	public GameObject[] buttons;

	//private Dictionary<Keyname, List<Keyname>> neighMap = new Dictionary<Keyname, List<Keyname>>();
	private Dictionary<Keyname, ArrayList> neighMap = new Dictionary<Keyname, ArrayList>();


	private const float moveThreshold = 1.0e-10f;
	private const float defaultSelectionTime = 0.25f;
	private float lastSelectionTime = defaultSelectionTime;

	private Color selectedColor = new Color(0.055f, 0.561f, 0.243f);
	private Color originalColor;
	private Keyname selectedButton;

	private float startTime = 0.0f;

	private int currentSentenceIndex = 0;	
	private string[] sentences = {
		"a bad fig jam",
		"ben can hang a bag",
		"ann had a mad camel",
		"ben can bake a cake",
		"hank feeding an eagle"
	};



	private void Start()
	{
		selectedButton = Keyname.KeyA; 	
		SetButtonColor(buttons[(int)Keyname.KeyA], selectedColor);
		textField.text = sentences[currentSentenceIndex];
		//inputField.ActivateInputField();

		// Construct neighborhood arrays
		neighMap.Add(Keyname.KeyA, new ArrayList(){Keyname.KeyD,Keyname.KeyC,Keyname.KeyB,Keyname.KeyG,Keyname.KeyF,Keyname.KeyE});
		neighMap.Add(Keyname.KeyB, new ArrayList(){Keyname.KeyC,Keyname.KeyJ,Keyname.KeyI,Keyname.KeyH,Keyname.KeyG,Keyname.KeyA});
		neighMap.Add(Keyname.KeyC, new ArrayList(){Keyname.KeyL,Keyname.KeyK,Keyname.KeyJ,Keyname.KeyB,Keyname.KeyA,Keyname.KeyD});
		neighMap.Add(Keyname.KeyD, new ArrayList(){Keyname.KeyM,Keyname.KeyL,Keyname.KeyC,Keyname.KeyA,Keyname.KeyE,Keyname.KeyN});
		neighMap.Add(Keyname.KeyE, new ArrayList(){Keyname.KeyN,Keyname.KeyD,Keyname.KeyA,Keyname.KeyF,Keyname.KeyP,Keyname.KeyO});
		neighMap.Add(Keyname.KeyF, new ArrayList(){Keyname.KeyE,Keyname.KeyA,Keyname.KeyG,Keyname.KeyR,Keyname.KeyQ,Keyname.KeyP});
		neighMap.Add(Keyname.KeyG, new ArrayList(){Keyname.KeyA,Keyname.KeyB,Keyname.KeyH,Keyname.KeyS,Keyname.KeyR,Keyname.KeyF});
		neighMap.Add(Keyname.KeyH, new ArrayList(){Keyname.KeyB,Keyname.KeyI,Keyname.KeyP,Keyname.KeyL,Keyname.KeyS,Keyname.KeyG});
		neighMap.Add(Keyname.KeyI, new ArrayList(){Keyname.KeyJ,Keyname.KeyS,Keyname.KeyO,Keyname.KeyK,Keyname.KeyH,Keyname.KeyB});
		neighMap.Add(Keyname.KeyJ, new ArrayList(){Keyname.KeyK,Keyname.KeyR,Keyname.KeyN,Keyname.KeyI,Keyname.KeyB,Keyname.KeyC});
		neighMap.Add(Keyname.KeyK, new ArrayList(){Keyname.KeyI,Keyname.KeyQ,Keyname.KeyM,Keyname.KeyJ,Keyname.KeyC,Keyname.KeyL});
		neighMap.Add(Keyname.KeyL, new ArrayList(){Keyname.KeyH,Keyname.KeyP,Keyname.KeyK,Keyname.KeyC,Keyname.KeyD,Keyname.KeyM});
		neighMap.Add(Keyname.KeyM, new ArrayList(){Keyname.KeyS,Keyname.KeyO,Keyname.KeyL,Keyname.KeyD,Keyname.KeyN,Keyname.KeyK});
		neighMap.Add(Keyname.KeyN, new ArrayList(){Keyname.KeyR,Keyname.KeyM,Keyname.KeyD,Keyname.KeyE,Keyname.KeyO,Keyname.KeyJ});
		neighMap.Add(Keyname.KeyO, new ArrayList(){Keyname.KeyQ,Keyname.KeyN,Keyname.KeyE,Keyname.KeyP,Keyname.KeyM,Keyname.KeyI});
		neighMap.Add(Keyname.KeyP, new ArrayList(){Keyname.KeyO,Keyname.KeyE,Keyname.KeyF,Keyname.KeyQ,Keyname.KeyL,Keyname.KeyH});
		neighMap.Add(Keyname.KeyQ, new ArrayList(){Keyname.KeyP,Keyname.KeyF,Keyname.KeyR,Keyname.KeyO,Keyname.KeyK,Keyname.KeyS});
		neighMap.Add(Keyname.KeyR, new ArrayList(){Keyname.KeyF,Keyname.KeyG,Keyname.KeyS,Keyname.KeyN,Keyname.KeyJ,Keyname.KeyQ});
		neighMap.Add(Keyname.KeyS, new ArrayList(){Keyname.KeyG,Keyname.KeyH,Keyname.KeyQ,Keyname.KeyM,Keyname.KeyI,Keyname.KeyR});
	}


	public void Update()
	{
		// Update the selection cooldown
		lastSelectionTime -= Time.deltaTime;

		float mouseX = Input.GetAxis ("Mouse X");
		float mouseY = Input.GetAxis ("Mouse Y");
		float sqrLength = mouseX * mouseX + mouseY * mouseY;

		float angle = Mathf.Atan2 (mouseY, mouseX) * Mathf.Rad2Deg;
		if (angle < 0)
			angle += 360;


		if (lastSelectionTime <= 0.0f && sqrLength > moveThreshold) 
		{
			SelectionChange(angle);
		}

	}


	private void LateUpdate()
	{
		ProcessKeyPress();
		//inputField.MoveToEndOfLine(false, false);
	}



	private void SetButtonColor(GameObject button, Color color)
	{
		MeshRenderer[] renderers = button.GetComponents<MeshRenderer>();
		foreach (MeshRenderer renderer in renderers)
		{
			renderer.material.color = color;
		}
	}




	private void ProcessKeyPress()
	{
		if (Input.anyKeyDown && inputField.text.Length == 0 && startTime == 0.0f)
		{
			// Start the timer for text entry
			startTime = Time.time;
		}

		if (selectedButton != null && Input.GetKeyDown(KeyCode.F1))
		{
			TextMeshProUGUI buttonText = buttons[(int)selectedButton].GetComponentInChildren<TextMeshProUGUI>();
			string character = buttonText.text;
			inputField.text += character.ToString();
		}

		// Handle backspace key
		if (Input.GetKeyDown(KeyCode.F2))
		{
			if (inputField != null && inputField.text.Length > 0)
			{
				inputField.text = inputField.text.Remove(inputField.text.Length - 1);
			}
		}

		// Handle space key
		if (Input.GetKeyDown(KeyCode.F3))
		{
			inputField.text += ' ';
		}

		// Handle the "Enter" key press
		if (Input.GetKeyDown(KeyCode.F4))
		{
			EnterKeyFunctionality();

			//WPM Calculation
			//float endTime = Time.time;
			// Calculate the text entry speed for the current sentence
			float elapsedTime = Time.time - startTime;
			float wordsPerMinute = (inputField.text.Length - 1) / elapsedTime * 60.0f * 0.2f;
			Debug.LogFormat("Text Entry Speed (Sentence {0}): {1} WPM", currentSentenceIndex, wordsPerMinute);

			// Reset start time
			startTime = 0.0f;
		}

	}




	//Perform Enter Key Functionality
	private void EnterKeyFunctionality()
	{
		if (textField != null)
		{

			currentSentenceIndex++;
			// Update the text field with the next sentence
			if (currentSentenceIndex  < sentences.Length)
			{
				textField.text = sentences[currentSentenceIndex];

				// Clear the input field
				inputField.text = string.Empty;
			}
			else
			{
				// Reset sentence index and display a message
				currentSentenceIndex = 0;
				textField.text = "Done";
			}
		}
	}


	// Change selection
	private void SelectionChange(float angle)
	{
		//ArrayList<Keyname> neighArray = neighMap[selectedButton];
		ArrayList neighArray = neighMap[selectedButton];

		if(neighArray != null)
		{
			SetButtonColor(buttons[(int)selectedButton], originalColor); 

			if (angle > 30.0f && angle <= 90.0f) 
			{
				selectedButton = (Keyname)neighArray[0];
			}
			else if (angle > 90.0f &&  angle <= 150.0f)
			{
				selectedButton = (Keyname)neighArray[1];
			}
			else if (angle > 150.0f &&  angle <= 210.0f)
			{
				selectedButton = (Keyname)neighArray[2];
			}
			else if (angle > 210.0f &&  angle <= 270.0f)
			{
				selectedButton = (Keyname)neighArray[3];
			}
			else if (angle > 270.0f &&  angle <= 330.0f)
			{
				selectedButton = (Keyname)neighArray[4];
			}
			else //if ((angle > 0.0f &&  angle <= 30.0f) || (angle > 330.0f &&  angle <= 360.0f))
			{
				selectedButton = (Keyname)neighArray[5];
			}

			lastSelectionTime = defaultSelectionTime; 
			SetButtonColor(buttons[(int)selectedButton], selectedColor);
		}
	}		

}






