using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfPanel : MonoBehaviour
{
   public Button button;

    public void Start()
    {
      
        button.onClick.AddListener(CloesP);
    }
    private void CloesP()
    {
        Destroy(this.gameObject);
    }
}
