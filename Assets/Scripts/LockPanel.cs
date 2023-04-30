using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class LockPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _codeText;
    [SerializeField] private string[] _codes;
    [SerializeField] private UnityEvent _onUnlockEvent;

    private string _outputCode = "";

    public void LockMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ClickCode(int number)
    {
        _outputCode += number;
        _codeText.SetText(_outputCode);
        foreach (string code in _codes)
        {
            if (_outputCode == code)
            {
                if (_onUnlockEvent != null)
                    _onUnlockEvent.Invoke();
                Destroy(this);
                return;
            }
        }
    }

    public void ResetCode()
    {
        _outputCode = "";
        _codeText.SetText(_outputCode);
    }
}
