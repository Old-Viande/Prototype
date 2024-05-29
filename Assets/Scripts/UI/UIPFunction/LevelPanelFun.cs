using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanelFun : MonoBehaviour
{
    public Button btnTutor;
    public Button btnPVE;

    public void Start()
    {
        btnTutor.onClick.AddListener(OnTutorClick);
        btnPVE.onClick.AddListener(OnPVEClick);
    }

    private void OnPVEClick()
    {
        throw new NotImplementedException();
    }

    private void OnTutorClick()
    {        
        TurnBaseFSM.Instance.ChangeState(States.AttackDrawPile);
    }
}

