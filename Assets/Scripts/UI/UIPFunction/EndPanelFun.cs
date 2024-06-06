using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndPanelFun : Singleton<EndPanelFun>
{
    public TextMeshProUGUI endText;
    public GameObject atkIcon;
    public GameObject defIcon;
    public Button exitBtn;
    // Start is called before the first frame update
    void Start()
    {
        exitBtn.onClick.AddListener(OnExitBtnClick);
    }

    public void SetEndText(bool atkWin)
    {
        int atkNum = GameManager.Instance.atkPawnDic.Count;
        int atkDie = GameManager.Instance.atkPawnGrave.Count;
        int defNum= GameManager.Instance.defPawnDic.Count;
        int defDie= GameManager.Instance.defPawnGrave.Count;
        if (atkWin)
        {
            atkIcon.SetActive(true);
            defIcon.SetActive(false);
            endText.text = "Attacker Wins!" + "\n" + "In this game, you have deployed a total of " + "<color=#FFF602>" + atkNum + "</color>" + "\n"
                + "and lost " + "<color=#FFF602>" + atkDie + "</color>" + " units." + "\n" + "You defeated a total of " + "<color=#FFF602>" + defDie + "</color>" + "\n" +
                "Well done.";
        }
        else
        {
            atkIcon.SetActive(false);
            defIcon.SetActive(true);
            endText.text = "Defender Wins!" + "\n" + "In this game, you have deployed a total of" + "<color=#FFF602>"+defNum+"</color>" + "\n"
                + "and lost " + "<color=#FFF602>"+defDie+"</color>" + " units." + "\n" + "You defeated a total of " + "<color=#FFF602>" + atkDie + "</color>" + "\n" +
                "Well done.";
        }     
    }
    private void OnExitBtnClick()
    {
        //重启整个场景
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }


}
