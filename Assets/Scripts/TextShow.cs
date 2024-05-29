using UnityEngine;
using TMPro; // 引用TextMeshPro命名空间
using UnityEngine.UI; // 引用UI命名空间，用于Scrollbar

public class TextShow : Singleton<TextShow>
{
    public TMP_Text textField; // 指向滚动视图中的TMP Text组件
    public RectTransform contentRectTransform; // 指向滚动视图内容的RectTransform
    public float scrollSpeed = 0.1f;

    private void Start()
    {
        // 清空文本框
        //textField.text = "";
    }

    // 添加新文本到文本框
    public void AddText(string newText)
    {
        textField.text += newText + "\n"; // 追加新文本，并换行
        Canvas.ForceUpdateCanvases(); // 立即更新Canvas，以确保滚动视图可以正确滚动到新内容
        contentRectTransform.anchoredPosition = new Vector2(0, 0); // 滚动到文本框底部

    }

    // 滚动到文本框底部


    // 更新滚动速度
    public void UpdateScrollSpeed(float newSpeed)
    {
        scrollSpeed = newSpeed;
    }

    private void Update()
    {
       //鼠标拖拽滚动视图内容
        if (Input.GetMouseButton(0))
        {
            contentRectTransform.anchoredPosition += new Vector2(0, Input.GetAxis("Mouse Y") * scrollSpeed);
        }
    
    }
}
