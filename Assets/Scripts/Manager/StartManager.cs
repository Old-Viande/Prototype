using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartManager :Singleton<StartManager>
{
    public PanelManager PanelManager;
    public Transform canvasTransform;  // ConfigPanel��Transform
    public GameObject InfPanel;
    // Start is called before the first frame update
    private new void Awake()
    {
        base.Awake();
        PanelManager = new PanelManager();
    }
    void Start()
    {      
        GameManager.Instance.CardPealInit();
        InfPanel = Resources.Load<GameObject>("Prefab/UI/InfPanel");
        // �ҵ�ConfigPanel��Transform
        GameObject canvasObject = GameObject.Find("Canvas");
        if (canvasObject != null)
        {
            canvasTransform = canvasObject.transform;
        }
        else
        {
            Debug.LogError("canvas not found in the scene.");
        }
        Canvas parentCanvas = GetComponentInParent<Canvas>();    
        //�Ѿ�֤ʵ����
        /*foreach (var item in GameManager.Instance.UniteTypeSave)
        {
          Debug.Log("card name: "+item.Key);
        }*/
    }
    public void OpenStartPanel()
    {
        PanelManager.PushPanel(new StartPanel());
    }
    public void OpenConfig()
    {
        PanelManager.PushPanel(new ConfigPanel());
    }
    public void OpenDraw()
    {
        PanelManager.PushPanel(new DrawPanel());
    }
    public void OpenLevel()
    {
        PanelManager.PushPanel(new LevelPanel());
    }
    public void OpenMainPanel()
    {
        PanelManager.PushPanel(new PawnPanel());
    }
    
    public void SetInfPanel(string text)
    {
        GameObject objp;
        objp = Instantiate(InfPanel, canvasTransform);
        //��objp���ڸ������������
        UITool.Instance.FindDeepChild(objp, "Text (TMP)").GetComponent<TextMeshProUGUI>().text = text;
        objp.transform.SetAsLastSibling();
    }
    public void ClosePanel()
    {
        PanelManager.PopPanel();
    }
}
