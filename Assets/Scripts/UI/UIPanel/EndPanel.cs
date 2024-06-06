using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPanel : BasePanel
{
    static readonly string path = "Prefab/UI/EndPanel";
    public EndPanel() : base(new UItype(path)) { }
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
        canvasGroup.interactable = false;
        //����canvas group��blocksRaycastsΪfalse
        canvasGroup.blocksRaycasts = false;



    }
    public override void OnResume()
    {
        base.OnResume();
        //����дUI�ָ�ʱ���߼�
        canvasGroup.interactable = true;
        //����canvas group��blocksRaycastsΪtrue
        canvasGroup.blocksRaycasts = true;
    }
}
