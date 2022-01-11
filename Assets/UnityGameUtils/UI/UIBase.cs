using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI基类
/// </summary>
public abstract class UIBase : MonoBehaviour
{
    [HideInInspector]
    public CanvasGroup mCanvasGroup;
    public UIRuningState RuningState;
    public virtual void Awake()
    {
        mCanvasGroup = GetComponent<CanvasGroup>();
    }
    public virtual void Close() {
        UIManager.Instance.CloseUI(this);
    }
    /// <summary>
    /// 进入UI面板
    /// </summary>
    public virtual void OnEnter()
    {
        this.gameObject.SetActive(true);
        RuningState = UIRuningState.Runing;
    }
    /// <summary>
    /// 暂停UI面板
    /// </summary>
    public virtual void OnPasue()
    {
        RuningState = UIRuningState.DisRuning;
    }
    /// <summary>
    /// 继续UI面板
    /// </summary>
    public virtual void OnResume()
    {
        RuningState = UIRuningState.Runing;
    }
    /// <summary>
    /// 退出UI面板
    /// </summary>
    public virtual void OnExit()
    {
        this.gameObject.SetActive(false);
        RuningState = UIRuningState.DisRuning;
    }
    /// <summary>
    /// 刷新UI面板
    /// </summary>
    public virtual void OnUpdate()
    {

    }
    /// <summary>
    /// 处理消息
    /// </summary>
    /// <param name="eventCode">事件码</param>
    /// <param name="message">消息</param>
    public virtual void Execute(int eventCode, object message)
    {

    }
}
public enum UIRuningState
{
    DisRuning,
    Runing
}