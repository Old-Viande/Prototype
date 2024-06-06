using UnityEngine;
using UnityEngine.UI;


public class CanvasTop : MonoBehaviour
{
    public Slider bloodbar;
    public Slider bloodFloor;
    public float FloorSpeed = 1;
    public PawnData pawn;


    private void Start()
    {
        pawn = this.transform.parent.GetComponent<PawnData>();
        EventManager.BloodBarChange += OnHpChange;
    }
    public void OnHpChange()
    {
        bloodbar.value = (float)pawn.Defence / (float)pawn.Unites.Defence;
    }
    private void Update()
    {
       
    }

    private void FaceAway()
    {
        Quaternion cameraRotation = Camera.main.transform.rotation;    
        transform.rotation = cameraRotation;
    }

    private void LateUpdate()
    {
        FaceAway();       
    }


}

