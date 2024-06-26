using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : BasePanel
{
    static readonly string path = "Prefab/UI/StartPanel";
    public StartPanel() : base(new UItype(path)) { }
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
        //设置canvas group的interactable为false
        canvasGroup.interactable = false;
        //设置canvas group的blocksRaycasts为false
        canvasGroup.blocksRaycasts = false;



    }
    public override void OnResume()
    {
        base.OnResume();
        //这里写UI恢复时的逻辑
        ////获取canvas group组件
        //设置canvas group的interactable为true
        canvasGroup.interactable = true;
        //设置canvas group的blocksRaycasts为true
        canvasGroup.blocksRaycasts = true;
    }
}
