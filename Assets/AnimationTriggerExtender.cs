using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationTriggerExtender : MonoBehaviour
{
    public UnityEvent OnAnimationEnd;
    public UnityEvent OnAnimationStart;

    public void AnimationEnd()
    {
        OnAnimationEnd.Invoke();
    }
    public void AnimationStart()
    {
        OnAnimationStart.Invoke();
    }
    public void DestroyThyself()
    {
        if (GetComponent<PhotonView>())
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
