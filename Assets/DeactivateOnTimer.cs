using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOnTimer : MonoBehaviour
{
    public float TimeTillDeactivate = 3f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Deactivate", TimeTillDeactivate);
    }
    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
