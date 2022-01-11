using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 带动画的UI
/// </summary>
public abstract class ActionUIBase : UIBase
{
    protected Animator uiAnim;
    public override void Awake()
    {
        base.Awake();
        uiAnim = transform.FindAll("Content").GetComponent<Animator>();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        uiAnim.SetBool("Open", true);
    }
    public override void OnExit()
    {
        uiAnim.SetBool("Open", false);
       
        base.OnExit();
        
    }
}
