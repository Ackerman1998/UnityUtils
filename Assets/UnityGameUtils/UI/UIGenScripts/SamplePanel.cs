using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 两个UIPanel,一个实现功能，一个用来存放自动化生成的字段
/// </summary>
public partial class SamplePanel : ActionUIBase
{
    public override void Awake()
    {
        base.Awake();
    }

    public void close() {
        UIManager.Instance.CloseUI<SamplePanel>();
    }
}
