using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileDependant : MonoBehaviour
{
    public bool EnableIfMobile;
    public bool DisableIfMobile;
    public bool EnableIfNotMobile;
    public bool DisableIfNotMobile;
    private void Start()
    {
        if (Application.isMobilePlatform)
        {
            if (EnableIfMobile) gameObject.SetActive(true);
            if (DisableIfMobile) gameObject.SetActive(false);
        }
        else
        {
            if (EnableIfNotMobile) gameObject.SetActive(true);
            if (DisableIfNotMobile) gameObject.SetActive(false);
        }
    }
}
