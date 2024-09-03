using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableOnMobile : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(Application.isMobilePlatform);
    }
}
