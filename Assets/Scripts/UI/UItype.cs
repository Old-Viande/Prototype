using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UItype 
{
   public string name { get;private set; }
    public string path { get; private set; }
    public UItype(string Path)
    {        
        this.path = Path;
        ///summary///
        ///��һ����Ϊ�˻�ȡUI�����֣���Ϊ�����ڴ���UI��ʱ�����ǻ��UI�����ֺ�UI��·�����ó�һ���ģ��������ǿ���ͨ��·������ȡUI������
        ///summary/// 
        this.name = Path.Substring(Path.LastIndexOf('/')+1);
    }
}
