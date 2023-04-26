using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    #region Singleton

    public static TransitionManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    private Animator _anim;
    public GameObject blockCastPanel;
    public bool isChangingScene;
    public bool isIntro;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        isChangingScene = true;
    }

    public void ResetLevel()
    {
        ChangeScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeScene(string name)
    {
        if (isChangingScene)
            return;

        blockCastPanel.SetActive(true);
        StartCoroutine(Transition(name));
    }

    IEnumerator Transition(string name)
    {
        isChangingScene = true;
        _anim.Play("out");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);
    }

    public void EndTranstion()
    {
        isChangingScene = false;
    }
}
