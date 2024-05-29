using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager
{   //������һ��ջ�������洢panel
    //���������������panel�ģ�������ʾ�����أ��л���
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

         //���ջ����panel����ͣ��ǰpanel        
         if(stackpanel.Count > 0)
         {
              currentpanel = stackpanel.Peek();
             currentpanel.OnPause();
         }
         //��ʾ��panel
         stackpanel.Push(nextPanel); 
         GameObject panel = uimanager.GetUI(nextPanel.UIType);
     }*/

    public void PushPanel(BasePanel nextPanel)
    {
        GameObject panel = null;
        //���ջ����panel����ͣ��ǰpanel 
        if (stackpanel.Count > 0)
        {
            currentpanel = stackpanel.Peek();
            currentpanel.OnPause();
        }
        //��ʾ��panel
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
            return; // ��ǰ���أ�����ʹ��null��panel
        }

        UITool.Instance.SetActivePanel(panel);
        nextPanel.OnEnter(); // ���� OnEnter
    }

    public void PopPanel() 
    {
        //���ջ����panel���˳���ǰpanel
       if(stackpanel.Count > 0)
        {
          stackpanel.Peek().OnExit();
          stackpanel.Pop();
        }
       //���ջ�л���panel���ָ��Ǹ�panel
       if(stackpanel.Count > 0)
        {          
            stackpanel.Peek().OnResume();
        }
    }
       
}