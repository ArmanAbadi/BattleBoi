using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlatformSpecific : MonoBehaviour
{
    public bool EnableIfMobile;
    public bool EnableIfDesktop;

    public GameObject Target;
    private void Start()
    {
        Target.SetActive(Application.isMobilePlatform && EnableIfMobile);
        Target.SetActive(!Application.isMobilePlatform && EnableIfDesktop);
    }
}
