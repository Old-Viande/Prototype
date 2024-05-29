using UnityEngine;
using TMPro; // ����TextMeshPro�����ռ�
using UnityEngine.UI; // ����UI�����ռ䣬����Scrollbar

public class TextShow : Singleton<TextShow>
{
    public TMP_Text textField; // ָ�������ͼ�е�TMP Text���
    public RectTransform contentRectTransform; // ָ�������ͼ���ݵ�RectTransform
    public float scrollSpeed = 0.1f;

    private void Start()
    {
        // ����ı���
        //textField.text = "";
    }

    // ������ı����ı���
    public void AddText(string newText)
    {
        textField.text += newText + "\n"; // ׷�����ı���������
        Canvas.ForceUpdateCanvases(); // ��������Canvas����ȷ��������ͼ������ȷ������������
        contentRectTransform.anchoredPosition = new Vector2(0, 0); // �������ı���ײ�

    }

    // �������ı���ײ�


    // ���¹����ٶ�
    public void UpdateScrollSpeed(float newSpeed)
    {
        scrollSpeed = newSpeed;
    }

    private void Update()
    {
       //�����ק������ͼ����
        if (Input.GetMouseButton(0))
        {
            contentRectTransform.anchoredPosition += new Vector2(0, Input.GetAxis("Mouse Y") * scrollSpeed);
        }
    
    }
}
