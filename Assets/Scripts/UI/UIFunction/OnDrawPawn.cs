using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class OnDrawPawn : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform dragArea;  // 拖拽区域
    private GameObject dragCopy;    // 用于拖拽的复制体
    private GameObject InfPanel;
    private CanvasGroup canvasGroup; // CanvasGroup 组件
    private Transform canvasTransform;  // ConfigPanel的Transform
    private GraphicRaycaster graphicRaycaster;  // 用于射线检测
    private PointerEventData pointerEventData;  // 用于存储指针事件数据
    private EventSystem eventSystem;  // 用于获取当前的事件系统
    private Camera mainCamera;  // 主相机

    private void OnEnable()
    {
        // 确保原始对象有一个 CanvasGroup 组件
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // 找到ConfigPanel的Transform
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
        InfPanel = Resources.Load<GameObject>("Prefab/UI/InfPanel");
        // 获取当前的事件系统
        eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            Debug.LogError("EventSystem not found.");
        }

        mainCamera = Camera.main;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvasTransform == null)
        {
            Debug.LogError("ConfigPanel is not set. Cannot begin drag.");
            return;
        }

        Debug.Log("Drag started");


        dragCopy = Instantiate(gameObject, canvasTransform);
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
                canvasTransform as RectTransform,
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



    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragCopy != null) Destroy(dragCopy);
        canvasGroup.alpha = 1;
        GameObject obj = LodManager.Instance.LoadResource(this.GetComponent<PawnData>().Name);
        if (GetObjectUnderMouse() != null)
        {
            Vector3 point = GetObjectUnderMouse().transform.position;
            if (GameManager.Instance.unitesGridMap.GetValue(point) == null)
                PawnDrag(obj, point);
        }

    }

    private void PawnDrag(GameObject obj, Vector3 point)
    {
        GameManager.Instance.floorGridMap.GetGridXZ(point, out int x, out int z);
        if (TurnBaseFSM.Instance.currentStateType == States.AttackPlacement)
        {

            if (x <= 3 && z <= 2)
            {
                point = GameManager.Instance.floorGridMap.GetGridCenter(x, z);
                GameObject pawn = Instantiate(obj, point, Quaternion.identity);
                PawnSet(pawn);
                //这个字典的key是世界坐标值，value是对应的单位
                GameManager.Instance.unitesGridMap.SetValue(pawn.transform.position, pawn);
                GameManager.Instance.AttackPawnPoolSave(this.name, this.gameObject);
            }
            else
            {
                GameObject objp;
                objp = Instantiate(InfPanel, canvasTransform);
                UITool.Instance.FindDeepChild(objp, "Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Out of range, please place units within the four columns on the left.";
            }

        }
        else if (TurnBaseFSM.Instance.currentStateType == States.DefencePlacement)
        {

            if (x > 6 && z <= 2)
            {
                point = GameManager.Instance.floorGridMap.GetGridCenter(x, z);
                GameObject pawn = Instantiate(obj, point, Quaternion.identity);
                PawnSet(pawn);
                GameManager.Instance.unitesGridMap.SetValue(pawn.transform.position, pawn);
                GameManager.Instance.DefencePawnPoolSave(this.name, this.gameObject);
            }
            else
            {
                GameObject objp;
                objp = Instantiate(InfPanel, canvasTransform);
                UITool.Instance.FindDeepChild(objp, "Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Out of range, please place units within the four columns on the right.";
            }
        }
    }

    public void PawnSet(GameObject pawn)//到具体场景中的单位配置
    {
        pawn.AddComponent<PawnData>();
        /* pawn.GetComponent<PawnData>().Unites = this.GetComponent<PawnData>().Unites;
         pawn.GetComponent<PawnData>().Name = this.GetComponent<PawnData>().Unites.Name;*/
        PawnData pad = pawn.GetComponent<PawnData>();
        pad.Unites = this.GetComponent<PawnData>().Unites;
        pad.Name = this.GetComponent<PawnData>().Name;
        pad.Weaopn = this.GetComponent<PawnData>().Weaopn;
        pad.Armor = this.GetComponent<PawnData>().Armor;
        pad.Shield = this.GetComponent<PawnData>().Shield;
        Type type = GameManager.Instance.PawnTypeCheck(pawn.GetComponent<PawnData>().Name);
        PawnAnimationSet(pawn, this.GetComponent<PawnData>());
        pawn.GetComponent<PawnData>().PawnSet();//设置脚本中的防御值和单位名称
        GameManager.Instance.AddComponent(pawn, type);
        pawn.name = this.name;
    }
    private void PawnAnimationSet(GameObject Pawn, PawnData unite)
    {
        string[] setskins = new string[3];
        setskins[0] = AnimationName(unite.Weaopn.Name);
        setskins[1] = AnimationName(unite.Armor.Name);
        setskins[2] = "Shield/Cape";
        Pawn.GetComponent<Spine2DSkinList>().skins = setskins;
    }

    public string AnimationName(string name)
    {
        switch (name)
        {
            case "Sword":
                return "Characters/Squire";
            case "Spear":
                return "Characters/Knight";
            case "Halberd":
                return "Characters/Bishop";
            case "GreatSword":
                return "Characters/Queen";
            case "LongBow":
                return "Characters/King";
            case "CrossBow":
                return "Characters/Rook";
            case "Cape":
                return "Armor/Default_A";
            case "Breastplate":
                return "Armor/Light_A";
            case "PlateArmor":
                return "Armor/Heavy_A";
        }
        return null;
    }
    private GameObject GetObjectUnderMouse()
    {

        // 检测场景物体
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hit = Physics.RaycastAll(ray);
        foreach (RaycastHit h in hit)
        {
            Debug.Log(h.collider.gameObject.name);
            if (h.collider.gameObject.name == "Plane(Clone)")
            {
                return h.collider.gameObject;
            }
        }
        return null;
    }

}

