using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsEvent : MonoBehaviour
{
    [SerializeField]
    GameObject playerWeapon;
    [SerializeField]
    List<AnimationClip> clips;

    private void OnEnable()
    {
        AddEventToAnimationClipsEnd(clips, "DeactiveObjAtTheEndOfClip");
    }

    void AddEventToAnimationClipsEnd(List<AnimationClip> clips, string eventMethodName)
    {
        foreach (AnimationClip clip in clips)
        {
            AnimationEvent animationEvent = new AnimationEvent();
            animationEvent.time = clip.length - 0.75f;
            animationEvent.functionName = eventMethodName;
            clip.AddEvent(animationEvent);
        }
    }

    public void DeactiveObjAtTheEndOfClip()
    {
        playerWeapon.SetActive(false);
    }
}
