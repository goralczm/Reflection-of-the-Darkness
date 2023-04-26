using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    #region Singleton

    public static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Another instance of Dialogue Manager found!");
            Destroy(instance);
        }

        instance = this;
    }

    #endregion

    [HideInInspector] public bool dialogueIsPlaying;

    [SerializeField] private UITweener _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private FPSController _playerMovement;
    [SerializeField] private GameObject[] choices;

    private TextMeshProUGUI[] choicesTexts;
    private Transform _currTarget;
    private Story _currDialogue;

    private bool _isTyping;
    private bool _isChoosing;

    private float _timeElapsedFromStart;
    private float _timeElapsedFromEnd = .1f;

    private void Start()
    {
        _dialoguePanel.gameObject.SetActive(false);

        choicesTexts = new TextMeshProUGUI[choices.Length];
        for (int i = 0; i < choices.Length; i++)
        {
            choicesTexts[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            _timeElapsedFromEnd += Time.deltaTime;
            return;
        }

        if (Input.GetButtonDown("Interact") && _timeElapsedFromStart > .1f)
            NextDialogue();

        _timeElapsedFromStart += Time.deltaTime;

        Vector3 dir = _currTarget.position - _playerMovement.transform.position;
        float targetYRot = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        _playerMovement.yaw = targetYRot;
    }

    private void ShowPanel()
    {
        _dialoguePanel.Show();
    }

    public void EnterDialogueMode(TextAsset newDialogueJSON, Transform newTarget)
    {
        if (_timeElapsedFromEnd < .1f)
            return;

        GameManager.instance.HideNote();
        CancelInvoke();
        StopAllCoroutines();
        _currTarget = newTarget;
        _currDialogue = new Story(newDialogueJSON.text);
        _timeElapsedFromStart = 0;
        _playerMovement.canWalk = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ShowPanel();
        dialogueIsPlaying = true;
        NextDialogue();
    }

    public void ShowInfo(string text, float showTime)
    {
        if (dialogueIsPlaying || _isTyping || _dialoguePanel.gameObject.activeSelf)
        {
            //QUEUE POPUP
            return;
        }

        ShowPanel();
        StartCoroutine(TypeText(text));
        Invoke("ExitDialogueMode", showTime);
    }

    private void ExitDialogueMode()
    {
        Debug.Log("Exiting");
        _dialogueText.SetText("");
        _playerMovement.canWalk = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _timeElapsedFromEnd = 0;
        dialogueIsPlaying = false;
        _dialoguePanel.Hide();
    }

    private void NextDialogue()
    {
        if (_isChoosing)
            return;

        if (_isTyping)
        {
            StopAllCoroutines();
            DisplayChoices();
            _dialogueText.SetText(_currDialogue.currentText);
            _isTyping = false;
            return;
        }

        if (_currDialogue.canContinue)
        {
            if (!_isTyping)
            {
                ResetChoices();
                StartCoroutine(TypeText(_currDialogue.Continue()));
                _isTyping = true;
            }
        }
        else
            ExitDialogueMode();
    }

    private void DisplayChoices()
    {
        Debug.Log("Display choices");
        if (_currDialogue == null)
            return;

        List<Choice> currentChoices = _currDialogue.currentChoices;

        if (currentChoices.Count > choices.Length)
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);

        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].SetActive(false);

            if (i < currentChoices.Count)
            {
                _isChoosing = true;
                choices[i].SetActive(true);
                choicesTexts[i].SetText(currentChoices[i].text);
            }
        }

        StartCoroutine(SelectFirstChoice());
    }

    private void ResetChoices()
    {
        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        _currDialogue.ChooseChoiceIndex(choiceIndex);
        _isChoosing = false;
        NextDialogue();
    }

    IEnumerator TypeText(string newText)
    {
        string outputText = "";
        foreach (char letter in newText)
        {
            outputText += letter;
            _dialogueText.SetText(outputText);
            yield return new WaitForSeconds(.1f);
        }
        DisplayChoices();
        _isTyping = false;
    }

    IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }
}
