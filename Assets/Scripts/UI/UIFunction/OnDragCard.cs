using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnDragCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform dragArea;  // 拖拽区域
    private GameObject dragCopy;    // 用于拖拽的复制体
    private CanvasGroup canvasGroup; // CanvasGroup 组件
    private Transform configPanelTransform;  // ConfigPanel的Transform
    private GraphicRaycaster graphicRaycaster;  // 用于射线检测
    private PointerEventData pointerEventData;  // 用于存储指针事件数据
    private EventSystem eventSystem;  // 用于获取当前的事件系统

    private void OnEnable()
    {
        // 确保原始对象有一个 CanvasGroup 组件
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // 找到ConfigPanel的Transform
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

        // 获取当前的事件系统
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

        // 创建复制体，并将其放置在ConfigPanel中
        dragCopy = Instantiate(gameObject, configPanelTransform);
        //dragCopy.GetComponent<CardData>().unites = gameObject.GetComponent<CardData>().unites;
        dragCopy.name = gameObject.name;

        // 获取复制体的 RectTransform 组件并设置为和原始对象一致
        RectTransform originalRectTransform = GetComponent<RectTransform>();
        RectTransform dragCopyRectTransform = dragCopy.GetComponent<RectTransform>();
        if (dragCopyRectTransform != null)
        {
            dragCopyRectTransform.anchorMin = originalRectTransform.anchorMin;
            dragCopyRectTransform.anchorMax = originalRectTransform.anchorMax;
            dragCopyRectTransform.pivot = originalRectTransform.pivot;
            dragCopyRectTransform.sizeDelta = originalRectTransform.sizeDelta;
        }

        // 将复制体放置在父对象的最前面（确保不会被其他UI元素遮挡）       
        dragCopy.transform.SetAsLastSibling();

        // 获取复制体的 CanvasGroup 组件
        var dragCopyCanvasGroup = dragCopy.GetComponent<CanvasGroup>();
        if (dragCopyCanvasGroup != null)
        {
            dragCopyCanvasGroup.blocksRaycasts = false; // 确保复制体不阻挡射线
            dragCopyCanvasGroup.alpha = 1; // 确保复制体可见
        }

        // 确保复制体可见
        dragCopy.SetActive(true);

        // 隐藏原始对象，通过设置透明度
        canvasGroup.alpha = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragCopy != null)
        {
            // 确保正确的坐标转换
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                configPanelTransform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint);

            dragCopy.transform.localPosition = localPoint;

            // 限制拖拽范围
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
            //将本体置在EquipBar下
            this.transform.SetParent(uiElementUnderMouse.transform);
            if (dragCopy != null)
            {
                Destroy(dragCopy);
            }
            if (uiElementUnderMouse.transform.childCount > 1)
            {
                foreach (Transform item in uiElementUnderMouse.transform)
                {
                    if (item == this.transform) continue; // 跳过当前拖动的对象

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
            // 恢复原始对象，通过恢复透明度
            canvasGroup.alpha = 1;
        }
        else
        {
            // 删除复制体
            if (dragCopy != null)
            {
                Destroy(dragCopy);
            }
            // 恢复原始对象，通过恢复透明度
            canvasGroup.alpha = 1;
        }

    }*/

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag ended");

        GameObject uiElementUnderMouse = GetUIElementUnderMouse();
        if (uiElementUnderMouse != null)
        {
            // 将本体置在EquipBar下
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

            // 恢复原始对象，通过恢复透明度
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
