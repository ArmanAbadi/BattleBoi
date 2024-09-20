using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableIfPlatformSpecific : MonoBehaviour
{
    public bool EnableIfMobile;
    public bool EnableIfDesktop;

    public GameObject Target;
    private void Start()
    {
        if(Application.isMobilePlatform && EnableIfMobile) Target.SetActive(true);
        if(!Application.isMobilePlatform && EnableIfDesktop) Target.SetActive(true);
    }
}
