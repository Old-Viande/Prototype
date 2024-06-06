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
        //这里写UI关闭时的逻辑
        /* canvasGroup = null;*/
        UITool.Instance.activepanel.SetActive(false);
    }
    public override void OnPause()
    {
        base.OnPause();
        //这里写UI暂停时的逻辑       
        canvasGroup.interactable = false;
        //设置canvas group的blocksRaycasts为false
        canvasGroup.blocksRaycasts = false;



    }
    public override void OnResume()
    {
        base.OnResume();
        //这里写UI恢复时的逻辑
        canvasGroup.interactable = true;
        //设置canvas group的blocksRaycasts为true
        canvasGroup.blocksRaycasts = true;
    }
}
