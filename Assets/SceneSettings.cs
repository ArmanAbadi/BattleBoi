using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSettings : MonoBehaviour
{
    public ScreenOrientation ScreenOrientation = ScreenOrientation.Portrait;
    // Start is called before the first frame update
    void Awake()
    {
        Screen.orientation = ScreenOrientation;
    }
}
