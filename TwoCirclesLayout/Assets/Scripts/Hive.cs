using System;
using System.Collections.Concurrent;
using MultiInput.Internal.Platforms.Windows;
using MultiInput.Internal.Platforms.Windows.PInvokeNet;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Windows;

public class Hive : MonoBehaviour
{
    private MyWMListener listener;

    public enum Keyname
	{
		KeyA, KeyB, KeyC, KeyD, KeyE, KeyF, KeyG, KeyH, KeyI, KeyJ, KeyK, KeyL, KeyM, KeyN, KeyO, KeyP, KeyQ, KeyR, KeyS
	}

	public TMP_InputField inputField;
	public TextMeshProUGUI textField;
	public GameObject[] buttonsL;
	public GameObject[] buttonsR;

	private Dictionary<Keyname, ArrayList> neighMapL = new Dictionary<Keyname, ArrayList>();
	private Dictionary<Keyname, ArrayList> neighMapR= new Dictionary<Keyname, ArrayList>();
    private ConcurrentDictionary<IntPtr, RawInputDeviceType> connectedDevices;

    private const float moveThreshold = 1.0e-10f;
	private const float defaultSelectionTime = 0.3f;
	private float lastSelectionTime = defaultSelectionTime;

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
        connectedDevices = new ConcurrentDictionary<IntPtr, RawInputDeviceType>();
        listener = new MyWMListener(OnInput, OnDeviceAdded, OnDeviceRemoved);
    }

    private void OnDestroy()
    {
        if (listener != null)
        {
            listener.Dispose();
            listener = null;
        }
    }

    private bool OnInput(RawInput input)
    {
        return true;
    }

    private void OnDeviceAdded(RawInputDevicesListItem device)
    {
        connectedDevices.TryAdd(device.hDevice, device.Type);
    }

    private void OnDeviceRemoved(RawInputDevicesListItem device)
    {
        connectedDevices.TryRemove(device.hDevice, out _);
    }



    private void Start()
	{
		selectedButtonL = Keyname.KeyA; 
		selectedButtonR = Keyname.KeyA; 
		SetButtonColor(buttonsL[(int)Keyname.KeyA], selectedColor);
		SetButtonColor(buttonsR[(int)Keyname.KeyA], selectedColor);
		textField.text = sentences[currentSentenceIndex];
		//inputField.ActivateInputField();

		// Construct Left neighborhood arrays
		neighMapL.Add(Keyname.KeyA, new ArrayList(){Keyname.KeyD,Keyname.KeyC,Keyname.KeyB,Keyname.KeyG,Keyname.KeyF,Keyname.KeyE});
		neighMapL.Add(Keyname.KeyB, new ArrayList(){Keyname.KeyC,Keyname.KeyJ,Keyname.KeyI,Keyname.KeyH,Keyname.KeyG,Keyname.KeyA});
		neighMapL.Add(Keyname.KeyC, new ArrayList(){Keyname.KeyL,Keyname.KeyK,Keyname.KeyJ,Keyname.KeyB,Keyname.KeyA,Keyname.KeyD});
		neighMapL.Add(Keyname.KeyD, new ArrayList(){Keyname.KeyM,Keyname.KeyL,Keyname.KeyC,Keyname.KeyA,Keyname.KeyE,Keyname.KeyN});
		neighMapL.Add(Keyname.KeyE, new ArrayList(){Keyname.KeyN,Keyname.KeyD,Keyname.KeyA,Keyname.KeyF,Keyname.KeyP,Keyname.KeyO});
		neighMapL.Add(Keyname.KeyF, new ArrayList(){Keyname.KeyE,Keyname.KeyA,Keyname.KeyG,Keyname.KeyR,Keyname.KeyQ,Keyname.KeyP});
		neighMapL.Add(Keyname.KeyG, new ArrayList(){Keyname.KeyA,Keyname.KeyB,Keyname.KeyH,Keyname.KeyS,Keyname.KeyR,Keyname.KeyF});
		neighMapL.Add(Keyname.KeyH, new ArrayList(){Keyname.KeyB,Keyname.KeyI,Keyname.KeyP,Keyname.KeyL,Keyname.KeyS,Keyname.KeyG});
		neighMapL.Add(Keyname.KeyI, new ArrayList(){Keyname.KeyJ,Keyname.KeyS,Keyname.KeyO,Keyname.KeyK,Keyname.KeyH,Keyname.KeyB});
		neighMapL.Add(Keyname.KeyJ, new ArrayList(){Keyname.KeyK,Keyname.KeyR,Keyname.KeyN,Keyname.KeyI,Keyname.KeyB,Keyname.KeyC});
		neighMapL.Add(Keyname.KeyK, new ArrayList(){Keyname.KeyI,Keyname.KeyQ,Keyname.KeyM,Keyname.KeyJ,Keyname.KeyC,Keyname.KeyL});
		neighMapL.Add(Keyname.KeyL, new ArrayList(){Keyname.KeyH,Keyname.KeyP,Keyname.KeyK,Keyname.KeyC,Keyname.KeyD,Keyname.KeyM});
		neighMapL.Add(Keyname.KeyM, new ArrayList(){Keyname.KeyS,Keyname.KeyO,Keyname.KeyL,Keyname.KeyD,Keyname.KeyN,Keyname.KeyK});
		neighMapL.Add(Keyname.KeyN, new ArrayList(){Keyname.KeyR,Keyname.KeyM,Keyname.KeyD,Keyname.KeyE,Keyname.KeyO,Keyname.KeyJ});
		neighMapL.Add(Keyname.KeyO, new ArrayList(){Keyname.KeyQ,Keyname.KeyN,Keyname.KeyE,Keyname.KeyP,Keyname.KeyM,Keyname.KeyI});
		neighMapL.Add(Keyname.KeyP, new ArrayList(){Keyname.KeyO,Keyname.KeyE,Keyname.KeyF,Keyname.KeyQ,Keyname.KeyL,Keyname.KeyH});
		neighMapL.Add(Keyname.KeyQ, new ArrayList(){Keyname.KeyP,Keyname.KeyF,Keyname.KeyR,Keyname.KeyO,Keyname.KeyK,Keyname.KeyS});
		neighMapL.Add(Keyname.KeyR, new ArrayList(){Keyname.KeyF,Keyname.KeyG,Keyname.KeyS,Keyname.KeyN,Keyname.KeyJ,Keyname.KeyQ});
		neighMapL.Add(Keyname.KeyS, new ArrayList(){Keyname.KeyG,Keyname.KeyH,Keyname.KeyQ,Keyname.KeyM,Keyname.KeyI,Keyname.KeyR});


		// Construct Right neighborhood arrays
		neighMapR.Add(Keyname.KeyA, new ArrayList(){Keyname.KeyD,Keyname.KeyC,Keyname.KeyB,Keyname.KeyG,Keyname.KeyF,Keyname.KeyE});
		neighMapR.Add(Keyname.KeyB, new ArrayList(){Keyname.KeyC,Keyname.KeyJ,Keyname.KeyI,Keyname.KeyH,Keyname.KeyG,Keyname.KeyA});
		neighMapR.Add(Keyname.KeyC, new ArrayList(){Keyname.KeyL,Keyname.KeyK,Keyname.KeyJ,Keyname.KeyB,Keyname.KeyA,Keyname.KeyD});
		neighMapR.Add(Keyname.KeyD, new ArrayList(){Keyname.KeyM,Keyname.KeyL,Keyname.KeyC,Keyname.KeyA,Keyname.KeyE,Keyname.KeyN});
		neighMapR.Add(Keyname.KeyE, new ArrayList(){Keyname.KeyN,Keyname.KeyD,Keyname.KeyA,Keyname.KeyF,Keyname.KeyP,Keyname.KeyO});
		neighMapR.Add(Keyname.KeyF, new ArrayList(){Keyname.KeyE,Keyname.KeyA,Keyname.KeyG,Keyname.KeyR,Keyname.KeyQ,Keyname.KeyP});
		neighMapR.Add(Keyname.KeyG, new ArrayList(){Keyname.KeyA,Keyname.KeyB,Keyname.KeyH,Keyname.KeyS,Keyname.KeyR,Keyname.KeyF});
		neighMapR.Add(Keyname.KeyH, new ArrayList(){Keyname.KeyB,Keyname.KeyI,Keyname.KeyP,Keyname.KeyL,Keyname.KeyS,Keyname.KeyG});
		neighMapR.Add(Keyname.KeyI, new ArrayList(){Keyname.KeyJ,Keyname.KeyS,Keyname.KeyO,Keyname.KeyK,Keyname.KeyH,Keyname.KeyB});
		neighMapR.Add(Keyname.KeyJ, new ArrayList(){Keyname.KeyK,Keyname.KeyR,Keyname.KeyN,Keyname.KeyI,Keyname.KeyB,Keyname.KeyC});
		neighMapR.Add(Keyname.KeyK, new ArrayList(){Keyname.KeyI,Keyname.KeyQ,Keyname.KeyM,Keyname.KeyJ,Keyname.KeyC,Keyname.KeyL});
		neighMapR.Add(Keyname.KeyL, new ArrayList(){Keyname.KeyH,Keyname.KeyP,Keyname.KeyK,Keyname.KeyC,Keyname.KeyD,Keyname.KeyM});
		neighMapR.Add(Keyname.KeyM, new ArrayList(){Keyname.KeyS,Keyname.KeyO,Keyname.KeyL,Keyname.KeyD,Keyname.KeyN,Keyname.KeyK});
		neighMapR.Add(Keyname.KeyN, new ArrayList(){Keyname.KeyR,Keyname.KeyM,Keyname.KeyD,Keyname.KeyE,Keyname.KeyO,Keyname.KeyJ});
		neighMapR.Add(Keyname.KeyO, new ArrayList(){Keyname.KeyQ,Keyname.KeyN,Keyname.KeyE,Keyname.KeyP,Keyname.KeyM,Keyname.KeyI});
		neighMapR.Add(Keyname.KeyP, new ArrayList(){Keyname.KeyO,Keyname.KeyE,Keyname.KeyF,Keyname.KeyQ,Keyname.KeyL,Keyname.KeyH});
		neighMapR.Add(Keyname.KeyQ, new ArrayList(){Keyname.KeyP,Keyname.KeyF,Keyname.KeyR,Keyname.KeyO,Keyname.KeyK,Keyname.KeyS});
		neighMapR.Add(Keyname.KeyR, new ArrayList(){Keyname.KeyF,Keyname.KeyG,Keyname.KeyS,Keyname.KeyN,Keyname.KeyJ,Keyname.KeyQ});
		neighMapR.Add(Keyname.KeyS, new ArrayList(){Keyname.KeyG,Keyname.KeyH,Keyname.KeyQ,Keyname.KeyM,Keyname.KeyI,Keyname.KeyR});
	}


	public void Update()
	{
		// Update the selection cooldown
		lastSelectionTime -= Time.deltaTime;

        if (connectedDevices.ContainsKey(input.Header.Device))
        {
            if (input.Header.Type == RawInputType.Mouse && input.Header.Device.ToInt32() == 65599)
            {
                var leftTrackball = input.Data.Mouse;
                Debug.Log($"MouseX of Left Trackball is: {leftTrackball.LastX}");
                Debug.Log($"MouseY of Left Trackball is: {leftTrackball.LastY}");
            }
            else if (input.Header.Type == RawInputType.Mouse && input.Header.Device.ToInt32() == 65597)
            {
                var rightTrackball = input.Data.Mouse;
                Debug.Log($"MouseX of Right Trackball is: {rightTrackball.LastX}");
                Debug.Log($"MouseY of Right Trackball is: {rightTrackball.LastY}");
            }
        }


        if (lastSelectionTime <= 0.0f && sqrLength > moveThreshold) 
		{
			SelectionChangeL(angle);
			SelectionChangeR(angle);
		}


	}


	private void LateUpdate()
	{
		ProcessKeyPress();
		//inputField.MoveToEndOfLine(false, false);
	}




	private void ProcessKeyPress()
	{
		if (Input.anyKeyDown && inputField.text.Length == 0 && startTime == 0.0f)
		{
			// Start the timer for text entry
			startTime = Time.time;
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



	private void SetButtonColor(GameObject button, Color color)
	{
		MeshRenderer[] renderers = button.GetComponents<MeshRenderer>();
		foreach (MeshRenderer renderer in renderers)
		{
			renderer.material.color = color;
		}
	}

	// Change selection Left
	private void SelectionChangeL(float angle)
	{
		ArrayList neighArray = neighMapL[selectedButtonL];

		if(neighArray != null)
		{
			SetButtonColor(buttonsL[(int)selectedButtonL], originalColor); 

			if (angle > 30.0f && angle <= 90.0f) 
			{
				selectedButtonL = (Keyname)neighArray[0];
			}
			else if (angle > 90.0f &&  angle <= 150.0f)
			{
				selectedButtonL = (Keyname)neighArray[1];
			}
			else if (angle > 150.0f &&  angle <= 210.0f)
			{
				selectedButtonL = (Keyname)neighArray[2];
			}
			else if (angle > 210.0f &&  angle <= 270.0f)
			{
				selectedButtonL = (Keyname)neighArray[3];
			}
			else if (angle > 270.0f &&  angle <= 330.0f)
			{
				selectedButtonL = (Keyname)neighArray[4];
			}
			else //if ((angle > 0.0f &&  angle <= 30.0f) || (angle > 330.0f &&  angle <= 360.0f))
			{
				selectedButtonL = (Keyname)neighArray[5];
			}

			lastSelectionTime = defaultSelectionTime; 
			SetButtonColor(buttonsL[(int)selectedButtonL], selectedColor);
		}
	}	


	// Change selection Right
	private void SelectionChangeR(float angle)
	{
		ArrayList neighArray = neighMapR[selectedButtonR];

		if(neighArray != null)
		{
			SetButtonColor(buttonsR[(int)selectedButtonR], originalColor); 

			if (angle > 30.0f && angle <= 90.0f) 
			{
				selectedButtonR = (Keyname)neighArray[0];
			}
			else if (angle > 90.0f &&  angle <= 150.0f)
			{
				selectedButtonR = (Keyname)neighArray[1];
			}
			else if (angle > 150.0f &&  angle <= 210.0f)
			{
				selectedButtonR = (Keyname)neighArray[2];
			}
			else if (angle > 210.0f &&  angle <= 270.0f)
			{
				selectedButtonR = (Keyname)neighArray[3];
			}
			else if (angle > 270.0f &&  angle <= 330.0f)
			{
				selectedButtonR = (Keyname)neighArray[4];
			}
			else //if ((angle > 0.0f &&  angle <= 30.0f) || (angle > 330.0f &&  angle <= 360.0f))
			{
				selectedButtonR = (Keyname)neighArray[5];
			}

			lastSelectionTime = defaultSelectionTime; 
			SetButtonColor(buttonsR[(int)selectedButtonR], selectedColor);
		}
	}


}






