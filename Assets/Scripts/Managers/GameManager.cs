using System.Collections;
using System.Collections.Generic;
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

    public UITweener notePanel;
    public TextMeshProUGUI noteText;
    public FPSController playerController;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            HideNote();
    }

    public void DisplayNote(string noteDescription)
    {
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
