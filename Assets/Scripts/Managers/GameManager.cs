using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Another instance of GameManager found!");
            Destroy(instance);
        }

        instance = this;
    }
    #endregion

    [Header("Note Display")]
    [SerializeField] private UITweener notePanel;
    [SerializeField] private Sprite normalNoteBg;
    [SerializeField] private Sprite mirrorNoteBg;
    [SerializeField] private TextMeshProUGUI noteText;
    [SerializeField] private Image noteBackground;

    [Header("Player")]
    public FPSController playerController;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            HideNote();
    }

    public void DisplayNote(string noteDescription, bool isMirror)
    {
        if (isMirror)
            noteBackground.sprite = mirrorNoteBg;
        else
            noteBackground.sprite = normalNoteBg;

        playerController.canWalk = false;
        noteText.SetText(noteDescription);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        notePanel.Show();
    }

    public void HideNote()
    {
        playerController.canWalk = true;
        notePanel.Hide();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
