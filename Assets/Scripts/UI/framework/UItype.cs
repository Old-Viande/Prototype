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
        ///这一段是为了获取UI的名字，因为我们在创建UI的时候，我们会把UI的名字和UI的路径设置成一样的，所以我们可以通过路径来获取UI的名字
        ///summary/// 
        this.name = Path.Substring(Path.LastIndexOf('/')+1);
    }
}
