using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPanel : BasePanel
{
    static readonly string path = "Prefab/UI/LevelPanel";
    public LevelPanel() : base(new UItype(path)) { }
    public override void OnExit()
    {
        base.OnExit();
        //����дUI�ر�ʱ���߼�
        /* canvasGroup = null;*/
        UITool.Instance.activepanel.SetActive(false);
    }
    public override void OnPause()
    {
        base.OnPause();
        //����дUI��ͣʱ���߼�       
        //����canvas group��interactableΪfalse
        canvasGroup.interactable = false;
        //����canvas group��blocksRaycastsΪfalse
        canvasGroup.blocksRaycasts = false;



    }
    public override void OnResume()
    {
        base.OnResume();
        //����дUI�ָ�ʱ���߼�
        ////��ȡcanvas group���
        //����canvas group��interactableΪtrue
        canvasGroup.interactable = true;
        //����canvas group��blocksRaycastsΪtrue
        canvasGroup.blocksRaycasts = true;
    }
}