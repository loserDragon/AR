using UnityEngine;

public delegate void AnimatorFinish();
public class JT_AnimatorPlayFinish : MonoBehaviour
{
    public Animator m_Animator;
    private RuntimeAnimatorController m_runtimeAnimatorController = null;
    private AnimationClip[] clips = null;
    public  AnimatorFinish m_Finish_OpenDoor;
    public AnimatorFinish m_Finish_OpenDoor_2;

    public AnimatorFinish m_Finish_CloseDoor;
    public AnimatorFinish m_Finish_CloseDoor_2;
    /// <summary>
    /// 动画完成一半时事件
    /// </summary>
    public static AnimatorFinish m_Middle_Dakaisuo;
    private void Start()
    {
        m_runtimeAnimatorController = m_Animator.runtimeAnimatorController;
        AnimatorEventInit();
    }


    private void AnimatorEventInit()
    {
        m_runtimeAnimatorController = m_Animator.runtimeAnimatorController;
        clips = m_runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i].events.Length == 0)
            {
                switch (clips[i].name)
                {
                    case "Take 001":
                        AnimationEvent m_OpenDoor_End = new AnimationEvent();
                        m_OpenDoor_End.functionName = "Finish_OpenDoor";
                        m_OpenDoor_End.time = clips[i].length;
                        clips[i].AddEvent(m_OpenDoor_End);
                        //   clips[i].frameRate
                        break;
                    case "Idle":
                        AnimationEvent m_CloseDoor_End = new AnimationEvent();
                        m_CloseDoor_End.functionName = "Finish_CloseDoor";
                        m_CloseDoor_End.time = clips[i].length;
                        clips[i].AddEvent(m_CloseDoor_End);
                        //   clips[i].frameRate
                        break;
                    default:
                        break;
                }
            }
        }

        //重新绑定动画器的所有动画的属性和网格数据。  
        m_Animator.Rebind();
    }


    private void Finish_OpenDoor()
    {
        if (m_Finish_OpenDoor != null)
        {
            m_Finish_OpenDoor();
        }
        if(m_Finish_OpenDoor_2 != null){
            m_Finish_OpenDoor_2();
        }
        Debug.Log("Finish");
    }

    private void Finish_CloseDoor()
    {
        if (m_Finish_CloseDoor != null)
        {
            m_Finish_CloseDoor();
        }
        if (m_Finish_CloseDoor_2 != null)
        {
            m_Finish_CloseDoor_2();
        }
        Debug.Log("Finish");
    }

}
