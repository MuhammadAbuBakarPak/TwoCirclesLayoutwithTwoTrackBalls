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

    private readonly ConcurrentQueue<RawInput> inputQueue = new ConcurrentQueue<RawInput>();

    public TMP_InputField inputField;
	public TextMeshProUGUI textField;

    public Transform leftCursor;
    public Transform rightCursor;

	private Vector3 leftCircleCenter;
	private Vector3 rightCircleCenter;

	private const int leftTrackballDeviceID = 65599;
	private const int rightTrackballDeviceID = 65597;

    private const float cursorSpeed = 2.0f;

    [HideInInspector] public GameObject selectedButtonR;
    [HideInInspector] public GameObject selectedButtonL;

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
		textField.text = sentences [currentSentenceIndex];
		inputField.ActivateInputField ();

		leftCircleCenter = leftCursor.localPosition;
		rightCircleCenter = rightCursor.localPosition;
	}


	public void Update()
	{
		Vector2 moveL = Vector2.zero;
		Vector2 moveR = Vector2.zero;

		while (inputQueue.TryDequeue(out var val))
		{
			if (val.Header.Type == RawInputType.Mouse && val.Header.Device.ToInt32() == leftTrackballDeviceID)
			{
				moveL.x += val.Data.Mouse.LastX;
				moveL.y -= val.Data.Mouse.LastY;
			}
			else if (val.Header.Type == RawInputType.Mouse && val.Header.Device.ToInt32() == rightTrackballDeviceID)
			{
				moveR.x += val.Data.Mouse.LastX;
				moveR.y -= val.Data.Mouse.LastY;
			}
		}

		if (moveL != Vector2.zero)
		{
            UpdateCursorPosition(leftCursor, moveL, leftCircleCenter);
        }

        if (moveR != Vector2.zero)
		{
            UpdateCursorPosition(rightCursor, moveR, rightCircleCenter);
        }
	}


    private void UpdateCursorPosition(Transform cursor, Vector2 move, Vector3 cursorCenter)
    {
        Vector3 currentPosition = cursor.transform.localPosition;
        currentPosition.x += move.x * cursorSpeed * Time.deltaTime;
        currentPosition.y -= move.y * cursorSpeed * Time.deltaTime;

		Vector3 offset = currentPosition - cursorCenter;
        currentPosition = cursorCenter + Vector3.ClampMagnitude(offset, 4.5f);
        cursor.transform.localPosition = currentPosition;
    }



    public void SetButtonColor(Color color, GameObject button)
    {
        MeshRenderer[] renderers = button.GetComponents<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material.color = color;
        }
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
            if (selectedButtonL != null)
            {
                TextMeshProUGUI buttonText = selectedButtonL.GetComponentInChildren<TextMeshProUGUI>();
                string character = buttonText.text;
                inputField.text += character;
            }
        }

		if (Input.GetKeyDown(KeyCode.F1))
		{
            if (selectedButtonR != null)
            {
                TextMeshProUGUI buttonText = selectedButtonR.GetComponentInChildren<TextMeshProUGUI>();
                string character = buttonText.text;
                inputField.text += character;
            }
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


	private void OnDestroy()
	{
		if (listener != null)
		{
			listener.Dispose();
			listener = null;
		}
	}


}
