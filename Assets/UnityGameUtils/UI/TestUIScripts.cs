using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIScripts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.OpenUI<GameMainPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
