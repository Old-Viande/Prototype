
using UnityEngine;
using Spine.Unity;
using Spine;
using System.Collections.Generic;

/*public class Spine2DSkinList : MonoBehaviour
{
    [SpineSkin]
    public string[] skins;

    [SpineAnimation]
    public string[] tracks;
    private SkeletonAnimation skeletonObj;
    private Skin combinedSkin;

    // Start is called before the first frame update

    void Start()
    {
        skeletonObj = this.GetComponent<SkeletonAnimation>();
         combinedSkin = new Skin("combinedSkin");
         for (int i = 0; i < skins.Length; i++)
         {
             var skinName = skins[i];
             combinedSkin.AddSkin(skeletonObj.Skeleton.Data.FindSkin(skinName));
         }
         skeletonObj.Skeleton.SetSkin(combinedSkin);
         skeletonObj.Skeleton.SetSlotsToSetupPose();
        SetFaceDir(skeletonObj);
        SetSkins(skins);
        SetAnimation(tracks);
    }

    void OnValidate()
    {
        SetSkins(skins);
        SetAnimation(tracks);
    }

    public void SetFaceDir(SkeletonAnimation skeletonObj)
    {
        var cheack = TurnBaseFSM.Instance.currentStateType == States.AttackPlacement ? true : false;
        if (cheack)
        {
            skeletonObj.initialFlipX = false;

        }
        else
        {
            skeletonObj.initialFlipX = true;
            skeletonObj.Skeleton.ScaleX = skeletonObj.initialFlipX ? 1 : -1;
            skeletonObj.Skeleton.UpdateWorldTransform();
        }
    }

    public void SetSkins(string[] newskins)
    {
        combinedSkin = new Skin("combinedSkin");
        if (skeletonObj != null && combinedSkin != null)
        {
            for (int i = 0; i < newskins.Length; i++)
            {
                var skinName = newskins[i];
                combinedSkin.AddSkin(skeletonObj.Skeleton.Data.FindSkin(skinName));
            }
            skeletonObj.Skeleton.SetSkin(combinedSkin);
            skeletonObj.Skeleton.SetSlotsToSetupPose();
        }
    }

    public void SetAnimation(string[] tracks)
    {
        if (skeletonObj == null || skeletonObj.AnimationState == null)
        {
            Debug.Log("SkeletonAnimation or AnimationState not found.");
            return;
        }

        if (tracks == null || tracks.Length == 0)
        {
            Debug.LogWarning("Tracks array is null or empty.");
            return;
        }

        // 设置第一个动画
        skeletonObj.AnimationState.SetAnimation(0, tracks[0], false);

        // 将剩余的动画依次添加到队列中
        for (int i = 1; i < tracks.Length; i++)
        {
            skeletonObj.AnimationState.AddAnimation(0, tracks[i], false, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}*/
public class Spine2DSkinList : MonoBehaviour
{
    [SpineSkin]
    public string[] skins;

    [SpineAnimation]
    public List<string> tracks = new List<string>();

    private SkeletonAnimation skeletonObj;
    private Skin combinedSkin;

    void Start()
    {
        if (skeletonObj == null)
        {
            skeletonObj = GetComponent<SkeletonAnimation>();
        }
        // 初始化和设置
        SetFaceDir(skeletonObj);
        SetSkins(skins);
        SetAnimation(tracks.ToArray(), false);
        
    }
    void OnEnable()
    {
        if (skeletonObj == null)
        {
            skeletonObj = GetComponent<SkeletonAnimation>();
        }
        // 初始化和设置
        SetFaceDir(skeletonObj);
        SetSkins(skins);
        SetAnimation(tracks.ToArray(), false);
        skeletonObj.AnimationState.Complete += delegate (TrackEntry trackEntry)
        {
            string[] idel = { "Idel/Idel" };
            SetAnimation(idel, true);
        };
    }  

    public void SetFaceDir(SkeletonAnimation skeletonObj)
    {
        // 检查状态并设置翻转方向
        bool check;
        if(TurnBaseFSM.Instance.currentStateType == States.AttackPlacement|| TurnBaseFSM.Instance.currentStateType == States.AttackReinforce)
        {
            check = true;
        }
        else
        {
            check = false;
        }
        skeletonObj.initialFlipX = check;
        // 确保 Skeleton 已初始化
        if (skeletonObj.Skeleton == null)
        {
            Debug.LogError("Skeleton is not initialized yet.");
            return;
        }     
        skeletonObj.Skeleton.ScaleX = skeletonObj.initialFlipX ? 1 : -1;      
    }
    
    public void SetSkins(string[] newskins)
    {
        // 创建一个新的皮肤组合
        combinedSkin = new Skin("combinedSkin");
        if (skeletonObj != null && combinedSkin != null)
        {
            for (int i = 0; i < newskins.Length; i++)
            {
                var skinName = newskins[i];
                Skin skin = skeletonObj.Skeleton.Data.FindSkin(skinName);
                if (skin != null)
                {
                    combinedSkin.AddSkin(skin);
                }
                else
                {
                    Debug.LogWarning($"Skin {skinName} not found.");
                }
            }
            skeletonObj.Skeleton.SetSkin(combinedSkin);
            skeletonObj.Skeleton.SetSlotsToSetupPose();
        }
        else
        {
            Debug.LogError("SkeletonAnimation or combinedSkin is null.");
        }
    }
    public void SetAnimaList(string rtracks)//添加动画到队列，但并不播放
    {
        tracks.Add(rtracks);
    }
    public void PlayAnimation()//将动画队列开始播放
    {
        SetAnimation(tracks.ToArray(), false);
        tracks.Clear();
    }
    public void SetAnimation(string[] rtracks,bool loop)
    {
        if (skeletonObj == null || skeletonObj.AnimationState == null)
        {
            Debug.Log("SkeletonAnimation or AnimationState not found.");
            return;
        }

        if (rtracks == null || rtracks.Length == 0)//确保没有动画在组内时，不会报错
        {
            Debug.Log("Tracks array is null or empty.");
            return;
        }

        // 设置第一个动画
        skeletonObj.AnimationState.SetAnimation(0, rtracks[0], loop);
       // Debug.Log(tracks);
        if(rtracks.Length == 1) return; // 如果只有一个动画，直接返回（不需要添加到队列中）
        // 将剩余的动画依次添加到队列中
        for (int i = 1; i < rtracks.Length; i++)
        {
            skeletonObj.AnimationState.AddAnimation(0, rtracks[i], loop, 0);
        }
    }

    void Update()
    {
        // 如果有需要在每帧更新的逻辑，可以放在这里
    }
}
