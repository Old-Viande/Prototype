using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartPanelFun : MonoBehaviour
{
    public Button startBtn;
    public Button exitBtn;
    // Start is called before the first frame update
    void Start()
    {
        startBtn.onClick.AddListener(OnStartBtnClick);
        exitBtn.onClick.AddListener(OnExitBtnClick);
    }
    public void OnStartBtnClick()
    {
        StartManager.Instance.ClosePanel();
        StartManager.Instance.OpenLevel();
    }
    private void OnExitBtnClick()
    {
        Application.Quit();
    }
}