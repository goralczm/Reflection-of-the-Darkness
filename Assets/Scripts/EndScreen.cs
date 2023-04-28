using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    #region Singleton

    public static EndScreen instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public UITweener endScreenPanel;

    [SerializeField] private UITweener panel;

    public void ShowEndscreen()
    {
        panel.Show();
    }
}
