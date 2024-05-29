using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnDragCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform dragArea;  // ��ק����
    private GameObject dragCopy;    // ������ק�ĸ�����
    private CanvasGroup canvasGroup; // CanvasGroup ���
    private Transform configPanelTransform;  // ConfigPanel��Transform
    private GraphicRaycaster graphicRaycaster;  // �������߼��
    private PointerEventData pointerEventData;  // ���ڴ洢ָ���¼�����
    private EventSystem eventSystem;  // ���ڻ�ȡ��ǰ���¼�ϵͳ

    private void OnEnable()
    {
        // ȷ��ԭʼ������һ�� CanvasGroup ���
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // �ҵ�ConfigPanel��Transform
        GameObject configPanelObject = GameObject.Find("Canvas");
        if (configPanelObject != null)
        {
            configPanelTransform = configPanelObject.transform;
        }
        else
        {
            Debug.LogError("ConfigPanel not found in the scene.");
        }
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            graphicRaycaster = parentCanvas.GetComponent<GraphicRaycaster>();
            if (graphicRaycaster == null)
            {
                Debug.LogError("GraphicRaycaster component not found on parent Canvas.");
            }
        }
        else
        {
            Debug.LogError("Parent Canvas not found.");
        }

        // ��ȡ��ǰ���¼�ϵͳ
        eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            Debug.LogError("EventSystem not found.");
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (configPanelTransform == null)
        {
            Debug.LogError("ConfigPanel is not set. Cannot begin drag.");
            return;
        }

        Debug.Log("Drag started");

        // ���������壬�����������ConfigPanel��
        dragCopy = Instantiate(gameObject, configPanelTransform);
        //dragCopy.GetComponent<CardData>().unites = gameObject.GetComponent<CardData>().unites;
        dragCopy.name = gameObject.name;

        // ��ȡ������� RectTransform ���������Ϊ��ԭʼ����һ��
        RectTransform originalRectTransform = GetComponent<RectTransform>();
        RectTransform dragCopyRectTransform = dragCopy.GetComponent<RectTransform>();
        if (dragCopyRectTransform != null)
        {
            dragCopyRectTransform.anchorMin = originalRectTransform.anchorMin;
            dragCopyRectTransform.anchorMax = originalRectTransform.anchorMax;
            dragCopyRectTransform.pivot = originalRectTransform.pivot;
            dragCopyRectTransform.sizeDelta = originalRectTransform.sizeDelta;
        }

        // ������������ڸ��������ǰ�棨ȷ�����ᱻ����UIԪ���ڵ���       
        dragCopy.transform.SetAsLastSibling();

        // ��ȡ������� CanvasGroup ���
        var dragCopyCanvasGroup = dragCopy.GetComponent<CanvasGroup>();
        if (dragCopyCanvasGroup != null)
        {
            dragCopyCanvasGroup.blocksRaycasts = false; // ȷ�������岻�赲����
            dragCopyCanvasGroup.alpha = 1; // ȷ��������ɼ�
        }

        // ȷ��������ɼ�
        dragCopy.SetActive(true);

        // ����ԭʼ����ͨ������͸����
        canvasGroup.alpha = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragCopy != null)
        {
            // ȷ����ȷ������ת��
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                configPanelTransform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint);

            dragCopy.transform.localPosition = localPoint;

            // ������ק��Χ
            if (dragArea != null)
            {
                Vector3[] corners = new Vector3[4];
                dragArea.GetWorldCorners(corners);

                Vector3 position = dragCopy.transform.position;
                position.x = Mathf.Clamp(position.x, corners[0].x, corners[2].x);
                position.y = Mathf.Clamp(position.y, corners[0].y, corners[2].y);

                dragCopy.transform.position = position;
            }
        }
    }

    /*public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag ended");

        GameObject uiElementUnderMouse = GetUIElementUnderMouse();
        if (uiElementUnderMouse != null)
        {
            //����������EquipBar��
            this.transform.SetParent(uiElementUnderMouse.transform);
            if (dragCopy != null)
            {
                Destroy(dragCopy);
            }
            if (uiElementUnderMouse.transform.childCount > 1)
            {
                foreach (Transform item in uiElementUnderMouse.transform)
                {
                    if (item == this.transform) continue; // ������ǰ�϶��Ķ���

                    if (item.gameObject.GetComponent<CardData>().unites.GetType() == this.GetComponent<CardData>().unites.GetType())
                    {
                        if (item.gameObject.GetComponent<CardData>().unites is Weapon)
                        {
                            item.gameObject.transform.SetParent(UITool.Instance.FindDeepChild("UContent").transform);
                        }
                        else if (item.gameObject.GetComponent<CardData>().unites is Armor)
                        {
                            item.gameObject.transform.SetParent(UITool.Instance.FindDeepChild("AContent").transform);
                        }
                        break;
                    }

                }

            }
            // �ָ�ԭʼ����ͨ���ָ�͸����
            canvasGroup.alpha = 1;
        }
        else
        {
            // ɾ��������
            if (dragCopy != null)
            {
                Destroy(dragCopy);
            }
            // �ָ�ԭʼ����ͨ���ָ�͸����
            canvasGroup.alpha = 1;
        }

    }*/

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag ended");

        GameObject uiElementUnderMouse = GetUIElementUnderMouse();
        if (uiElementUnderMouse != null)
        {
            // ����������EquipBar��
            this.transform.SetParent(uiElementUnderMouse.transform);
            if (dragCopy != null) Destroy(dragCopy);

            foreach (Transform item in uiElementUnderMouse.transform)
            {
                if (item != this.transform && item.gameObject.GetComponent<CardData>().unites.GetType() == this.GetComponent<CardData>().unites.GetType())
                {
                    string targetParent = item.gameObject.GetComponent<CardData>().unites is Weapon ? "UContent" : "AContent";
                    item.gameObject.transform.SetParent(UITool.Instance.FindDeepChild(targetParent).transform);
                    break;
                }
            }

            // �ָ�ԭʼ����ͨ���ָ�͸����
            canvasGroup.alpha = 1;
        }
        else
        {
            if (dragCopy != null) Destroy(dragCopy);
            canvasGroup.alpha = 1;
        }
    }

    private GameObject GetUIElementUnderMouse()
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                Debug.Log("UI element under mouse: " + result.gameObject.name);
                if (result.gameObject.name == "EquipBar")
                {
                    return result.gameObject;
                }
            }
        }
        return null;
    }


}
