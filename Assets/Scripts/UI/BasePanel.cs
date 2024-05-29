using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有UI面板的基类
/// </summary>
public class BasePanel
{
    public UItype UIType { get; private set; }  
    public CanvasGroup canvasGroup;
    public BasePanel(UItype uiType)
    {
        UIType = uiType;
    }
    
    public virtual void OnEnter()
    {
        canvasGroup = UITool.Instance.GetorAddComponent<CanvasGroup>();
        Debug.Log("OnEnter"+canvasGroup);
    }
    public virtual void OnPause()
    {
        
    }
    public virtual void OnResume()
    {
        
    }

    public virtual void OnExit()
    {
        
    }


}
