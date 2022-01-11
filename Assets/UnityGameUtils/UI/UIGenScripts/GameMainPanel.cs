using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 两个UIPanel,一个实现功能，一个用来存放自动化生成的字段
/// </summary>
public partial class GameMainPanel : UIBase
{
    public override void Awake()
    {
        base.Awake();
        OpenPanel_Button.onClick.AddListener(Open); 
    }

    public void close() {
        UIManager.Instance.CloseUI<GameMainPanel>();
    }
    public void Open() {
        UIManager.Instance.OpenUI<GameTipsPanel>();
    }
}
