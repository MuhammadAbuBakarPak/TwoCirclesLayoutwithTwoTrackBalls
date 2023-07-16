using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Collections.Concurrent;
using MultiInput.Internal.Platforms.Windows;
using MultiInput.Internal.Platforms.Windows.PInvokeNet;

public class Hive : MonoBehaviour
{
	private MyWMListener listener;

	public enum Keyname
	{
		KeyA, KeyB, KeyC, KeyD, KeyE, KeyF, KeyG, KeyH, KeyI, KeyJ, KeyK, KeyL, KeyM, KeyN, KeyO, KeyP, KeyQ, KeyR, KeyS
	}

/*	public enum KeynameR
	{
		KeyA, KeyB, KeyC, KeyD, KeyE, KeyF, KeyG, KeyH, KeyI, KeyJ, KeyK, KeyL, KeyM, KeyN, KeyO, KeyP, KeyQ, KeyR, KeyS
	}
*/
	public TMP_InputField inputField;
	public TextMeshProUGUI textField;
	public GameObject[] buttons;
//	public GameObject[] buttonsR;

	private Dictionary<KeynameL, ArrayList> neighMap = new Dictionary<KeynameL, ArrayList>();
//	private Dictionary<KeynameR, ArrayList> neighMapR= new Dictionary<KeynameR, ArrayList>();

	private readonly ConcurrentQueue<RawInput> inputQueue = new ConcurrentQueue<RawInput>();

	private const int leftTrackballDeviceID = 65599;
	private const int rightTrackballDeviceID = 65597;
 
	private const float moveThreshold = 1.0e-10f;
	private const float defaultSelectionTime = 0.3f;

 	private float lastSelectionTimeL = defaultSelectionTime;
	private float lastSelectionTimeR = defaultSelectionTime;

	private Color selectedColor = new Color(0.055f, 0.561f, 0.243f);
	private Color originalColor;
 
	private Keyname selectedButtonL;
	private Keyname selectedButtonR;

	private float startTime = 0.0f;

	private int currentSentenceIndex = 0;	
	private string[] sentences = {
		"a bad fig jam",
		"ben can hang a bag",
		"ann had a mad camel",
		"ben can bake a cake",
		"hank feeding an eagle"
	};

	private void Awake()
	{
		listener = new MyWMListener(OnInput, OnDeviceAdded, OnDeviceRemoved);
	}
 
	private bool OnInput(RawInput input)
	{
		inputQueue.Enqueue(input);
		return true;
	}
 
	private void OnDeviceAdded(RawInputDevicesListItem device)
	{
	}
	
 	private void OnDeviceRemoved(RawInputDevicesListItem device)
	{
	}
		
	private void Start()
	{
		selectedButtonL = KeynameL.KeyA; 
		selectedButtonR = KeynameR.KeyA; 
		SetButtonColor(buttonsL[(int)KeynameL.KeyA], selectedColor);
		SetButtonColor(buttonsR[(int)KeynameR.KeyA], selectedColor);
		textField.text = sentences[currentSentenceIndex];
		//inputField.ActivateInputField();

		// Construct Left neighborhood arrays
		neighMapL.Add(KeynameL.KeyA, new ArrayList(){KeynameL.KeyD,KeynameL.KeyC,KeynameL.KeyB,KeynameL.KeyG,KeynameL.KeyF,KeynameL.KeyE});
		neighMapL.Add(KeynameL.KeyB, new ArrayList(){KeynameL.KeyC,KeynameL.KeyJ,KeynameL.KeyI,KeynameL.KeyH,KeynameL.KeyG,KeynameL.KeyA});
		neighMapL.Add(KeynameL.KeyC, new ArrayList(){KeynameL.KeyL,KeynameL.KeyK,KeynameL.KeyJ,KeynameL.KeyB,KeynameL.KeyA,KeynameL.KeyD});
		neighMapL.Add(KeynameL.KeyD, new ArrayList(){KeynameL.KeyM,KeynameL.KeyL,KeynameL.KeyC,KeynameL.KeyA,KeynameL.KeyE,KeynameL.KeyN});
		neighMapL.Add(KeynameL.KeyE, new ArrayList(){KeynameL.KeyN,KeynameL.KeyD,KeynameL.KeyA,KeynameL.KeyF,KeynameL.KeyP,KeynameL.KeyO});
		neighMapL.Add(KeynameL.KeyF, new ArrayList(){KeynameL.KeyE,KeynameL.KeyA,KeynameL.KeyG,KeynameL.KeyR,KeynameL.KeyQ,KeynameL.KeyP});
		neighMapL.Add(KeynameL.KeyG, new ArrayList(){KeynameL.KeyA,KeynameL.KeyB,KeynameL.KeyH,KeynameL.KeyS,KeynameL.KeyR,KeynameL.KeyF});
		neighMapL.Add(KeynameL.KeyH, new ArrayList(){KeynameL.KeyB,KeynameL.KeyI,KeynameL.KeyP,KeynameL.KeyL,KeynameL.KeyS,KeynameL.KeyG});
		neighMapL.Add(KeynameL.KeyI, new ArrayList(){KeynameL.KeyJ,KeynameL.KeyS,KeynameL.KeyO,KeynameL.KeyK,KeynameL.KeyH,KeynameL.KeyB});
		neighMapL.Add(KeynameL.KeyJ, new ArrayList(){KeynameL.KeyK,KeynameL.KeyR,KeynameL.KeyN,KeynameL.KeyI,KeynameL.KeyB,KeynameL.KeyC});
		neighMapL.Add(KeynameL.KeyK, new ArrayList(){KeynameL.KeyI,KeynameL.KeyQ,KeynameL.KeyM,KeynameL.KeyJ,KeynameL.KeyC,KeynameL.KeyL});
		neighMapL.Add(KeynameL.KeyL, new ArrayList(){KeynameL.KeyH,KeynameL.KeyP,KeynameL.KeyK,KeynameL.KeyC,KeynameL.KeyD,KeynameL.KeyM});
		neighMapL.Add(KeynameL.KeyM, new ArrayList(){KeynameL.KeyS,KeynameL.KeyO,KeynameL.KeyL,KeynameL.KeyD,KeynameL.KeyN,KeynameL.KeyK});
		neighMapL.Add(KeynameL.KeyN, new ArrayList(){KeynameL.KeyR,KeynameL.KeyM,KeynameL.KeyD,KeynameL.KeyE,KeynameL.KeyO,KeynameL.KeyJ});
		neighMapL.Add(KeynameL.KeyO, new ArrayList(){KeynameL.KeyQ,KeynameL.KeyN,KeynameL.KeyE,KeynameL.KeyP,KeynameL.KeyM,KeynameL.KeyI});
		neighMapL.Add(KeynameL.KeyP, new ArrayList(){KeynameL.KeyO,KeynameL.KeyE,KeynameL.KeyF,KeynameL.KeyQ,KeynameL.KeyL,KeynameL.KeyH});
		neighMapL.Add(KeynameL.KeyQ, new ArrayList(){KeynameL.KeyP,KeynameL.KeyF,KeynameL.KeyR,KeynameL.KeyO,KeynameL.KeyK,KeynameL.KeyS});
		neighMapL.Add(KeynameL.KeyR, new ArrayList(){KeynameL.KeyF,KeynameL.KeyG,KeynameL.KeyS,KeynameL.KeyN,KeynameL.KeyJ,KeynameL.KeyQ});
		neighMapL.Add(KeynameL.KeyS, new ArrayList(){KeynameL.KeyG,KeynameL.KeyH,KeynameL.KeyQ,KeynameL.KeyM,KeynameL.KeyI,KeynameL.KeyR});
  
		// Construct Right neighborhood arrays
		neighMapR.Add(KeynameR.KeyA, new ArrayList(){KeynameR.KeyD,KeynameR.KeyC,KeynameR.KeyB,KeynameR.KeyG,KeynameR.KeyF,KeynameR.KeyE});
		neighMapR.Add(KeynameR.KeyB, new ArrayList(){KeynameR.KeyC,KeynameR.KeyJ,KeynameR.KeyI,KeynameR.KeyH,KeynameR.KeyG,KeynameR.KeyA});
		neighMapR.Add(KeynameR.KeyC, new ArrayList(){KeynameR.KeyL,KeynameR.KeyK,KeynameR.KeyJ,KeynameR.KeyB,KeynameR.KeyA,KeynameR.KeyD});
		neighMapR.Add(KeynameR.KeyD, new ArrayList(){KeynameR.KeyM,KeynameR.KeyL,KeynameR.KeyC,KeynameR.KeyA,KeynameR.KeyE,KeynameR.KeyN});
		neighMapR.Add(KeynameR.KeyE, new ArrayList(){KeynameR.KeyN,KeynameR.KeyD,KeynameR.KeyA,KeynameR.KeyF,KeynameR.KeyP,KeynameR.KeyO});
		neighMapR.Add(KeynameR.KeyF, new ArrayList(){KeynameR.KeyE,KeynameR.KeyA,KeynameR.KeyG,KeynameR.KeyR,KeynameR.KeyQ,KeynameR.KeyP});
		neighMapR.Add(KeynameR.KeyG, new ArrayList(){KeynameR.KeyA,KeynameR.KeyB,KeynameR.KeyH,KeynameR.KeyS,KeynameR.KeyR,KeynameR.KeyF});
		neighMapR.Add(KeynameR.KeyH, new ArrayList(){KeynameR.KeyB,KeynameR.KeyI,KeynameR.KeyP,KeynameR.KeyL,KeynameR.KeyS,KeynameR.KeyG});
		neighMapR.Add(KeynameR.KeyI, new ArrayList(){KeynameR.KeyJ,KeynameR.KeyS,KeynameR.KeyO,KeynameR.KeyK,KeynameR.KeyH,KeynameR.KeyB});
		neighMapR.Add(KeynameR.KeyJ, new ArrayList(){KeynameR.KeyK,KeynameR.KeyR,KeynameR.KeyN,KeynameR.KeyI,KeynameR.KeyB,KeynameR.KeyC});
		neighMapR.Add(KeynameR.KeyK, new ArrayList(){KeynameR.KeyI,KeynameR.KeyQ,KeynameR.KeyM,KeynameR.KeyJ,KeynameR.KeyC,KeynameR.KeyL});
		neighMapR.Add(KeynameR.KeyL, new ArrayList(){KeynameR.KeyH,KeynameR.KeyP,KeynameR.KeyK,KeynameR.KeyC,KeynameR.KeyD,KeynameR.KeyM});
		neighMapR.Add(KeynameR.KeyM, new ArrayList(){KeynameR.KeyS,KeynameR.KeyO,KeynameR.KeyL,KeynameR.KeyD,KeynameR.KeyN,KeynameR.KeyK});
		neighMapR.Add(KeynameR.KeyN, new ArrayList(){KeynameR.KeyR,KeynameR.KeyM,KeynameR.KeyD,KeynameR.KeyE,KeynameR.KeyO,KeynameR.KeyJ});
		neighMapR.Add(KeynameR.KeyO, new ArrayList(){KeynameR.KeyQ,KeynameR.KeyN,KeynameR.KeyE,KeynameR.KeyP,KeynameR.KeyM,KeynameR.KeyI});
		neighMapR.Add(KeynameR.KeyP, new ArrayList(){KeynameR.KeyO,KeynameR.KeyE,KeynameR.KeyF,KeynameR.KeyQ,KeynameR.KeyL,KeynameR.KeyH});
		neighMapR.Add(KeynameR.KeyQ, new ArrayList(){KeynameR.KeyP,KeynameR.KeyF,KeynameR.KeyR,KeynameR.KeyO,KeynameR.KeyK,KeynameR.KeyS});
		neighMapR.Add(KeynameR.KeyR, new ArrayList(){KeynameR.KeyF,KeynameR.KeyG,KeynameR.KeyS,KeynameR.KeyN,KeynameR.KeyJ,KeynameR.KeyQ});
		neighMapR.Add(KeynameR.KeyS, new ArrayList(){KeynameR.KeyG,KeynameR.KeyH,KeynameR.KeyQ,KeynameR.KeyM,KeynameR.KeyI,KeynameR.KeyR});
	}


	public void Update()
	{
		// Update the selection cooldown
		lastSelectionTimeL -= Time.deltaTime;
		lastSelectionTimeR -= Time.deltaTime;

		if (inputQueue.TryDequeue(out var val))
		{
			if (val.Header.Type == RawInputType.Mouse && val.Header.Device.ToInt32() == leftTrackballDeviceID)
			{
				var trackball = val.Data.Mouse;
				float trackballX = trackball.LastX;
				float trackballY = -trackball.LastY;
				float trackballSqrLength = trackballX * trackballX + trackballY * trackballY;
    
				float trackballAngle = Mathf.Atan2(trackballY, trackballX) * Mathf.Rad2Deg;
				if (trackballAngle < 0)
					trackballAngle += 360;
     
				//Debug.Log($"Left Trackball Angle is: {leftTrackballAngle}");
				if (lastSelectionTimeL <= 0.0f && trackballSqrLength > moveThreshold) 
				{
					SelectionChange(ref selectedButtonL, trackballAngle);
					lastSelectionTimeL = defaultSelectionTime; 
				}
			}
			else if (val.Header.Type == RawInputType.Mouse && val.Header.Device.ToInt32() == rightTrackballDeviceID)
			{
				var trackball = val.Data.Mouse;
				float trackballX = trackball.LastX;
				float trackballY = -trackball.LastY;
				float trackballSqrLength = trackballX * trackballX + trackballY * trackballY;
    
				float trackballAngle = Mathf.Atan2(trackballY, trackballX) * Mathf.Rad2Deg;
				if (trackballAngle < 0)
					trackballAngle += 360;

				//Debug.Log($"Right Trackball Angle is: {rightTrackballAngle}");
				if (lastSelectionTimeR <= 0.0f && trackballSqrLength > moveThreshold) 
				{
					SelectionChange(ref selectedButtonR, trackballAngle);
					lastSelectionTimeR = defaultSelectionTime; 
				}
			}
		}
	}


	private void LateUpdate()
	{
		ProcessKeyPress();
		//inputField.MoveToEndOfLine(false, false);
	}


	private void ProcessKeyPress()
	{
		int T = inputField.text.Length;
		if (Input.anyKeyDown && T == 0 && startTime == 0.0f)
		{
			startTime = Time.time; 	// Start the timer for text entry
		}

		if (Input.GetKeyDown(KeyCode.F1))
		{
			TextMeshProUGUI buttonText = buttonsL[(int)selectedButtonL].GetComponentInChildren<TextMeshProUGUI>();
			string character = buttonText.text;
			inputField.text += character.ToString();
		}

		if (Input.GetKeyDown(KeyCode.F5))
		{
			TextMeshProUGUI buttonText = buttonsR[(int)selectedButtonR].GetComponentInChildren<TextMeshProUGUI>();
			string character = buttonText.text;
			inputField.text += character.ToString();
		}

		// Handle backspace key
		if (Input.GetKeyDown(KeyCode.F2))
		{
			if (inputField != null && T > 0)
			{
				inputField.text = inputField.text.Remove(T - 1);
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
			float wordsPerMinute = (T - 1) / elapsedTime * 60.0f * 0.2f;
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
				inputField.text = string.Empty; 	// Clear the input field
			}
			else
			{
				// Reset sentence index and display a message
				currentSentenceIndex = 0;
				textField.text = "Done";
			}
		}
	}



	private void SetButtonColor(GameObject button, Color color)
	{
		MeshRenderer[] renderers = button.GetComponents<MeshRenderer>();
		foreach (MeshRenderer renderer in renderers)
		{
			renderer.material.color = color;
		}
	}

	// Change selection
	private void SelectionChange(ref Keyname button, float angle)
	{
		ArrayList neighArray = neighMap[button];

		if(neighArray != null)
		{
			SetButtonColor(buttons[(int)button], originalColor); 

			if (angle > 30.0f && angle <= 90.0f) 
			{
				button = (Keyname) neighArray[0];
			}
			else if (angle > 90.0f &&  angle <= 150.0f)
			{
				button = (Keyname) neighArray[1];
			}
			else if (angle > 150.0f &&  angle <= 210.0f)
			{
				button = (Keyname) neighArray[2];
			}
			else if (angle > 210.0f &&  angle <= 270.0f)
			{
				button = (Keyname) neighArray[3];
			}
			else if (angle > 270.0f &&  angle <= 330.0f)
			{
				button = (Keyname) neighArray[4];
			}
			else //if ((angle > 0.0f &&  angle <= 30.0f) || (angle > 330.0f &&  angle <= 360.0f))
			{
				button = (Keyname) neighArray[5];
			}

//			lastSelectionTime = defaultSelectionTime; 
			SetButtonColor(buttons[(int)button], selectedColor);
		}
	}	


	// Change selection Right
/*	private void SelectionChangeR(float angle)
	{
		ArrayList neighArray = neighMapR[selectedButtonR];

		if(neighArray != null)
		{
			SetButtonColor(buttonsR[(int)selectedButtonR], originalColor); 

			if (angle > 30.0f && angle <= 90.0f) 
			{
				selectedButtonR = (KeynameR)neighArray[0];
			}
			else if (angle > 90.0f &&  angle <= 150.0f)
			{
				selectedButtonR = (KeynameR)neighArray[1];
			}
			else if (angle > 150.0f &&  angle <= 210.0f)
			{
				selectedButtonR = (KeynameR)neighArray[2];
			}
			else if (angle > 210.0f &&  angle <= 270.0f)
			{
				selectedButtonR = (KeynameR)neighArray[3];
			}
			else if (angle > 270.0f &&  angle <= 330.0f)
			{
				selectedButtonR = (KeynameR)neighArray[4];
			}
			else //if ((angle > 0.0f &&  angle <= 30.0f) || (angle > 330.0f &&  angle <= 360.0f))
			{
				selectedButtonR = (KeynameR)neighArray[5];
			}

			lastSelectionTime = defaultSelectionTime; 
			SetButtonColor(buttonsR[(int)selectedButtonR], selectedColor);
		}
	}
*/

	private void OnDestroy()
	{
		if (listener != null)
		{
			listener.Dispose();
			listener = null;
		}
	}


}






