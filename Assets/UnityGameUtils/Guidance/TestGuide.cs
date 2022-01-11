using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGuide : MonoBehaviour
{
    public RectTransform target;
    // Start is called before the first frame update
    void Start()
    {
        RectGuidance.instance.Init(target);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
