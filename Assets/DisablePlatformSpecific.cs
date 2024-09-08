using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlatformSpecific : MonoBehaviour
{
    public bool DisableIfMobile;
    public bool DisableIfNotMobile;
    private void Start()
    {
        if (Application.isMobilePlatform)
        {
            if (DisableIfMobile) gameObject.SetActive(false);
        }
        else
        {
            if (DisableIfNotMobile) gameObject.SetActive(false);
        }
    }
}
