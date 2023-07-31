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
		KeyA, KeyB, KeyC, KeyD, KeyE, KeyF, KeyG, KeyH, KeyI, KeyJ, KeyK, KeyL, KeyM, KeyN, KeyO, KeyP, KeyQ, KeyR, KeyS, KeyT, KeyU, KeyV, KeyW, KeyX, KeyY, KeyZ, KeySpace1, KeySpace2, KeySpace3, KeySpace4, KeySpace5, KeySpace6, KeySpace7, KeySpace8, KeyNum,
		KeyBackSpace, KeyEnter, KeyShift
	}

	public TMP_InputField inputField;
	public TextMeshProUGUI textField;
	public GameObject[] buttons;

	public GameObject leftCursor;
	public GameObject rightCursor;

	private readonly ConcurrentQueue<RawInput> inputQueue = new ConcurrentQueue<RawInput>();

	private const int leftTrackballDeviceID = 65599;
	private const int rightTrackballDeviceID = 65597;

	private const float cursorSpeed = 0.05f;

	private Color selectedColor = new Color(0.055f, 0.561f, 0.243f);
	private Color originalColor;

	private Keyname selectedButtonL;
	private Keyname selectedButtonR;

	private float startTime = 0.0f;

	private int currentSentenceIndex = 0;
	private string[] sentences = {
		"the future is here",
		"i love sunshine",
		"more power to you",
		"give me a pen",
		"typing with trackball is easy"
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
		textField.text = sentences[currentSentenceIndex];
		inputField.ActivateInputField();
	}


	public void Update()
	{
		while (inputQueue.TryDequeue(out var val))
		{
			if (val.Header.Type == RawInputType.Mouse && val.Header.Device.ToInt32() == leftTrackballDeviceID)
			{
				float mouseX = val.Data.Mouse.LastX;
				float mouseY = -val.Data.Mouse.LastY;
				UpdateCursorPosition(leftCursor, mouseX, mouseY);
			}
			else if (val.Header.Type == RawInputType.Mouse && val.Header.Device.ToInt32() == rightTrackballDeviceID)
			{
				float mouseX = val.Data.Mouse.LastX;
				float mouseY = -val.Data.Mouse.LastY;
				UpdateCursorPosition(rightCursor, mouseX, mouseY);
			}
		}

	}

	private void UpdateCursorPosition(GameObject cursor, float x, float y)
	{
		Vector3 currentPosition = cursor.transform.position;
		currentPosition.x += x * cursorSpeed * Time.deltaTime;
		currentPosition.y -= y * cursorSpeed * Time.deltaTime;
		cursor.transform.position = currentPosition;

	}


	private void LateUpdate()
	{
		ProcessKeyPress();
		inputField.MoveToEndOfLine(false, false);
	}


	private void ProcessKeyPress()
	{
		int T = inputField.text.Length;
		if (Input.anyKeyDown && T == 0 && startTime == 0.0f)
		{
			startTime = Time.time;  // Start the timer for text entry
		}

		if (Input.GetKeyDown(KeyCode.F6))
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
			if (currentSentenceIndex < sentences.Length)
			{
				textField.text = sentences[currentSentenceIndex];
				inputField.text = string.Empty;     // Clear the input field
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



	private void OnDestroy()
	{
		if (listener != null)
		{
			listener.Dispose();
			listener = null;
		}
	}


}
