using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Windows.Speech;
using System.Text;
using System.Linq;

public class VoiceInteraction : MonoBehaviour
{
    public TMP_InputField chatInputField; // Assign the Chat Input Field in Inspector
    public Button toggleVoiceButton; // Assign the Toggle Button in Inspector
    private DictationRecognizer dictationRecognizer;
    private bool isListening = false;

    void Start()
    {
        // Initialize Dictation Recognizer
        dictationRecognizer = new DictationRecognizer();

        // Event handlers for speech recognition
        dictationRecognizer.DictationResult += OnDictationResult;
        dictationRecognizer.DictationComplete += OnDictationComplete;
        dictationRecognizer.DictationError += OnDictationError;

        // Assign button click event
        toggleVoiceButton.onClick.AddListener(ToggleVoiceInput);
    }

    void ToggleVoiceInput()
    {
        if (!isListening)
        {
            StartListening();
        }
        else
        {
            StopListening();
        }
    }

    void StartListening()
    {
        if (dictationRecognizer.Status == SpeechSystemStatus.Running) return;

        dictationRecognizer.Start();
        isListening = true;
        toggleVoiceButton.GetComponentInChildren<TMP_Text>().text = "Stop Listening";
        Debug.Log("Voice recognition started...");
    }

    void StopListening()
    {
        if (dictationRecognizer.Status == SpeechSystemStatus.Stopped) return;

        dictationRecognizer.Stop();
        isListening = false;
        toggleVoiceButton.GetComponentInChildren<TMP_Text>().text = "Start Voice Input";
        Debug.Log("Voice recognition stopped.");
    }

    void OnDictationResult(string text, ConfidenceLevel confidence)
    {
        chatInputField.text = text; // Set recognized speech in the input field
    }

    void OnDictationComplete(DictationCompletionCause cause)
    {
        Debug.Log("Dictation completed: " + cause);
        StopListening();
    }

    void OnDictationError(string error, int hresult)
    {
        Debug.LogError($"Dictation Error: {error}");
        StopListening();
    }

    void OnDestroy()
    {
        dictationRecognizer.Dispose();
    }
}
