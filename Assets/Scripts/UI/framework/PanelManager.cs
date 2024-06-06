using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager
{   //这里是一个栈，用来存储panel
    //这个类是用来管理panel的，包括显示，隐藏，切换等
    private Stack<BasePanel> stackpanel = new Stack<BasePanel>();
    private Dictionary<string, GameObject> panelDict = new Dictionary<string, GameObject>();
    private UIManager uimanager;
    private BasePanel currentpanel;
    public PanelManager()
    {
        uimanager = new UIManager();
        stackpanel = new Stack<BasePanel>();
    }

    /* public void PushPanel(BasePanel nextPanel)
     {   

         //如果栈中有panel，暂停当前panel        
         if(stackpanel.Count > 0)
         {
              currentpanel = stackpanel.Peek();
             currentpanel.OnPause();
         }
         //显示新panel
         stackpanel.Push(nextPanel); 
         GameObject panel = uimanager.GetUI(nextPanel.UIType);
     }*/

    public void PushPanel(BasePanel nextPanel)
    {
        GameObject panel = null;
        //如果栈中有panel，暂停当前panel 
        if (stackpanel.Count > 0)
        {
            currentpanel = stackpanel.Peek();
            currentpanel.OnPause();
        }
        //显示新panel
        if(panelDict.ContainsKey(nextPanel.UIType.name))
        {
            stackpanel.Push(nextPanel);
            panelDict[nextPanel.UIType.name].SetActive(true);
             panel= panelDict[nextPanel.UIType.name];
        }
        else
        {
            stackpanel.Push(nextPanel);
            panel = uimanager.GetUI(nextPanel.UIType);
            panelDict.Add(nextPanel.UIType.name, panel);
        }       
        if (panel == null)
        {
            Debug.LogError("Failed to retrieve the panel for " + nextPanel.UIType.name);
            return; // 提前返回，避免使用null的panel
        }

        UITool.Instance.SetActivePanel(panel);
        nextPanel.OnEnter(); // 调用 OnEnter
    }

    public void PopPanel() 
    {
        //如果栈中有panel，退出当前panel
       if(stackpanel.Count > 0)
        {
          stackpanel.Peek().OnExit();
          stackpanel.Pop();
        }
       //如果栈中还有panel，恢复那个panel
       if(stackpanel.Count > 0)
        {          
            stackpanel.Peek().OnResume();
        }
    }
       
}