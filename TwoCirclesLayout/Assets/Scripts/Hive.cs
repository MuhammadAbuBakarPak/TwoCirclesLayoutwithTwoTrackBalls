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
		KeyA, KeyB, KeyC, KeyD, KeyE, KeyF, KeyG, KeyH, KeyI, KeyJ, KeyK, KeyL, KeyM, KeyN, KeyO, KeyP, KeyQ, KeyR, KeyS, KeyT, KeyU, KeyV, KeyW, KeyX, KeyY,KeyZ, KeySpace1,KeySpace2, KeySpace3, KeySpace4, KeySpace5,KeySpace6, KeySpace7, KeySpace8, KeyNum,
		KeyBackSpace, KeyEnter, KeyShift
	}

	public TMP_InputField inputField;
	public TextMeshProUGUI textField;
	public GameObject[] buttons;

	private Dictionary<Keyname, ArrayList> neighMap = new Dictionary<Keyname, ArrayList>();

	private readonly ConcurrentQueue<RawInput> inputQueue = new ConcurrentQueue<RawInput>();

	private const int leftTrackballDeviceID = 65599;
	private const int rightTrackballDeviceID = 65597;

	private const float moveThreshold = 6.0f;
	private const float defaultSelectionTime = 0.25f;

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
		selectedButtonL = Keyname.KeyD; 
		selectedButtonR = Keyname.KeyK; 
		SetButtonColor(buttons[(int)Keyname.KeyD], selectedColor);
		SetButtonColor(buttons[(int)Keyname.KeyK], selectedColor);
		textField.text = sentences[currentSentenceIndex];
		//inputField.ActivateInputField();

		// Construct neighborhood arrays
		neighMap.Add(Keyname.KeyA, new ArrayList(){Keyname.KeyW,Keyname.KeyNum,Keyname.KeyG,Keyname.KeyQ,Keyname.KeyZ,Keyname.KeyS});
		neighMap.Add(Keyname.KeyB, new ArrayList(){Keyname.KeyJ,Keyname.KeyH,Keyname.KeyEnter,Keyname.KeySpace8,Keyname.KeySpace3,Keyname.KeyN});
		neighMap.Add(Keyname.KeyC, new ArrayList(){Keyname.KeyF,Keyname.KeyD,Keyname.KeyX,Keyname.KeySpace1,Keyname.KeySpace2,Keyname.KeyV});
		neighMap.Add(Keyname.KeyD, new ArrayList(){Keyname.KeyR,Keyname.KeyE,Keyname.KeyS,Keyname.KeyX,Keyname.KeyC,Keyname.KeyF});
		neighMap.Add(Keyname.KeyE, new ArrayList(){Keyname.KeySpace5,Keyname.KeyQ,Keyname.KeyW,Keyname.KeyS,Keyname.KeyD,Keyname.KeyR});
		neighMap.Add(Keyname.KeyF, new ArrayList(){Keyname.KeyT,Keyname.KeyR,Keyname.KeyD,Keyname.KeyC,Keyname.KeyV,Keyname.KeyG});
		neighMap.Add(Keyname.KeyG, new ArrayList(){Keyname.KeySpace2,Keyname.KeyT,Keyname.KeyF,Keyname.KeyV,Keyname.KeySpace6,Keyname.KeyA});
		neighMap.Add(Keyname.KeyH, new ArrayList(){Keyname.KeyY,Keyname.KeySpace3,Keyname.KeyBackSpace,Keyname.KeySpace7,Keyname.KeyB,Keyname.KeyJ});
		neighMap.Add(Keyname.KeyI, new ArrayList(){Keyname.KeyP,Keyname.KeySpace8,Keyname.KeyU,Keyname.KeyK,Keyname.KeyL,Keyname.KeyO});
		neighMap.Add(Keyname.KeyJ, new ArrayList(){Keyname.KeyU,Keyname.KeyY,Keyname.KeyH,Keyname.KeyB,Keyname.KeyN,Keyname.KeyK});
		neighMap.Add(Keyname.KeyK, new ArrayList(){Keyname.KeyI,Keyname.KeyU,Keyname.KeyJ,Keyname.KeyN,Keyname.KeyM,Keyname.KeyL});
		neighMap.Add(Keyname.KeyL, new ArrayList(){Keyname.KeyO,Keyname.KeyI,Keyname.KeyK,Keyname.KeyM,Keyname.KeyEnter,Keyname.KeyBackSpace});
		neighMap.Add(Keyname.KeyM, new ArrayList(){Keyname.KeyL,Keyname.KeyK,Keyname.KeyN,Keyname.KeySpace4,Keyname.KeyShift,Keyname.KeyEnter});
		neighMap.Add(Keyname.KeyN, new ArrayList(){Keyname.KeyK,Keyname.KeyJ,Keyname.KeyB,Keyname.KeySpace3,Keyname.KeySpace4,Keyname.KeyM});
		neighMap.Add(Keyname.KeyO, new ArrayList(){Keyname.KeySpace4,Keyname.KeyP,Keyname.KeyI,Keyname.KeyL,Keyname.KeyBackSpace,Keyname.KeyY});
		neighMap.Add(Keyname.KeyP, new ArrayList(){Keyname.KeySpace3,Keyname.KeyBackSpace,Keyname.KeySpace8,Keyname.KeyI,Keyname.KeyO,Keyname.KeySpace7});
		neighMap.Add(Keyname.KeyQ, new ArrayList(){Keyname.KeyA,Keyname.KeySpace2,Keyname.KeySpace6,Keyname.KeyW,Keyname.KeyE,Keyname.KeySpace5});
		neighMap.Add(Keyname.KeyR, new ArrayList(){Keyname.KeySpace6,Keyname.KeySpace5,Keyname.KeyE,Keyname.KeyD,Keyname.KeyF,Keyname.KeyT});
		neighMap.Add(Keyname.KeyS, new ArrayList(){Keyname.KeyE,Keyname.KeyW,Keyname.KeyA,Keyname.KeyZ,Keyname.KeyX,Keyname.KeyD});
		neighMap.Add(Keyname.KeyT, new ArrayList(){Keyname.KeySpace1,Keyname.KeySpace6,Keyname.KeyR,Keyname.KeyF,Keyname.KeyG,Keyname.KeyW});
		neighMap.Add(Keyname.KeyU, new ArrayList(){Keyname.KeySpace8,Keyname.KeySpace7,Keyname.KeyY,Keyname.KeyJ,Keyname.KeyK,Keyname.KeyI});
		neighMap.Add(Keyname.KeyV, new ArrayList(){Keyname.KeyG,Keyname.KeyF,Keyname.KeyC,Keyname.KeySpace2,Keyname.KeySpace5,Keyname.KeyZ});
		neighMap.Add(Keyname.KeyW, new ArrayList(){Keyname.KeyQ,Keyname.KeySpace1,Keyname.KeyT,Keyname.KeyA,Keyname.KeyS,Keyname.KeyE});
		neighMap.Add(Keyname.KeyX, new ArrayList(){Keyname.KeyD,Keyname.KeyS,Keyname.KeyZ,Keyname.KeyNum,Keyname.KeySpace1,Keyname.KeyC});
		neighMap.Add(Keyname.KeyY, new ArrayList(){Keyname.KeySpace7,Keyname.KeySpace4,Keyname.KeyO,Keyname.KeyH,Keyname.KeyJ,Keyname.KeyU});
		neighMap.Add(Keyname.KeyZ, new ArrayList(){Keyname.KeyS,Keyname.KeyA,Keyname.KeyV,Keyname.KeySpace5,Keyname.KeyNum,Keyname.KeyX});
		neighMap.Add(Keyname.KeySpace1, new ArrayList(){Keyname.KeyC,Keyname.KeyX,Keyname.KeyNum,Keyname.KeyT,Keyname.KeyW,Keyname.KeySpace2});
		neighMap.Add(Keyname.KeySpace2, new ArrayList(){Keyname.KeyV,Keyname.KeyC,Keyname.KeySpace1,Keyname.KeyG,Keyname.KeyQ,Keyname.KeyNum});
		neighMap.Add(Keyname.KeySpace3, new ArrayList(){Keyname.KeyN,Keyname.KeyB,Keyname.KeyShift,Keyname.KeyP,Keyname.KeyH,Keyname.KeySpace4});
		neighMap.Add(Keyname.KeySpace4, new ArrayList(){Keyname.KeyM,Keyname.KeyN,Keyname.KeySpace3,Keyname.KeyO,Keyname.KeyY,Keyname.KeyShift});
		neighMap.Add(Keyname.KeySpace5, new ArrayList(){Keyname.KeyZ,Keyname.KeyV,Keyname.KeyQ,Keyname.KeyE,Keyname.KeyR,Keyname.KeySpace6});
		neighMap.Add(Keyname.KeySpace6, new ArrayList(){Keyname.KeyNum,Keyname.KeyG,Keyname.KeySpace5,Keyname.KeyR,Keyname.KeyT,Keyname.KeyQ});
		neighMap.Add(Keyname.KeySpace7, new ArrayList(){Keyname.KeyH,Keyname.KeyShift,Keyname.KeyP,Keyname.KeyY,Keyname.KeyU,Keyname.KeySpace8});
		neighMap.Add(Keyname.KeySpace8, new ArrayList(){Keyname.KeyB,Keyname.KeyEnter,Keyname.KeySpace7,Keyname.KeyU,Keyname.KeyI,Keyname.KeyP});
		neighMap.Add(Keyname.KeyNum, new ArrayList(){Keyname.KeyX,Keyname.KeyZ,Keyname.KeySpace2,Keyname.KeySpace6,Keyname.KeyA,Keyname.KeySpace1});
		neighMap.Add(Keyname.KeyBackSpace, new ArrayList(){Keyname.KeyShift,Keyname.KeyO,Keyname.KeyL,Keyname.KeyEnter,Keyname.KeyP,Keyname.KeyH});
		neighMap.Add(Keyname.KeyEnter, new ArrayList(){Keyname.KeyBackSpace,Keyname.KeyL,Keyname.KeyM,Keyname.KeyShift,Keyname.KeySpace8,Keyname.KeyB});
		neighMap.Add(Keyname.KeyShift, new ArrayList(){Keyname.KeyEnter,Keyname.KeyM,Keyname.KeySpace4,Keyname.KeyBackSpace,Keyname.KeySpace7,Keyname.KeySpace3});
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
				float trackballSqrLength, trackballAngle;
				GetTrackBallInfo(out trackballSqrLength, out trackballAngle, val.Data.Mouse);

				//Debug.Log($"Left Trackball Angle is: {leftTrackballAngle}");
				if (lastSelectionTimeL <= 0.0f && trackballSqrLength > moveThreshold) 
				{
					SelectionChange(ref selectedButtonL, trackballAngle);
					lastSelectionTimeL = defaultSelectionTime; 
				}
			}
			else if (val.Header.Type == RawInputType.Mouse && val.Header.Device.ToInt32() == rightTrackballDeviceID)
			{
				float trackballSqrLength, trackballAngle;
				GetTrackBallInfo(out trackballSqrLength, out trackballAngle, val.Data.Mouse);

				//Debug.Log($"Right Trackball Angle is: {rightTrackballAngle}");
				if (lastSelectionTimeR <= 0.0f && trackballSqrLength > moveThreshold) 
				{
					SelectionChange(ref selectedButtonR, trackballAngle);
					lastSelectionTimeR = defaultSelectionTime; 
				}
			}
		}
	}

	private void GetTrackBallInfo(out float sqrLength, out float angle, RawMouse trackball)
 	{
		float X = trackball.LastX;
		float Y = -trackball.LastY;
		sqrLength = X * X + Y * Y;
	
		angle = Mathf.Atan2(Y, X) * Mathf.Rad2Deg;
		if (angle < 0)
			angle += 360;  
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

		if (Input.GetKeyDown(KeyCode.F5))
		{
			TextMeshProUGUI buttonText = buttons[(int)selectedButtonL].GetComponentInChildren<TextMeshProUGUI>();
			string character = buttonText.text;
			inputField.text += character.ToString();
		}

		if (Input.GetKeyDown(KeyCode.F1))
		{
			TextMeshProUGUI buttonText = buttons[(int)selectedButtonR].GetComponentInChildren<TextMeshProUGUI>();
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

	private void OnDestroy()
	{
		if (listener != null)
		{
			listener.Dispose();
			listener = null;
		}
	}


}
