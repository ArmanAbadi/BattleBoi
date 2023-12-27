using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    bool Following = false;
    Vector3 Height = Vector3.zero;
    private void Start()
    {
        Height = transform.position.z*Vector3.forward;
    }
    // Update is called once per frame
    void Update()
    {
        if(PlayerController.Instance != null)
        {
            transform.position = PlayerController.Instance.transform.position + Height;
        }
    }
}
