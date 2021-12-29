using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏存档
/// </summary>
[System.Serializable]
public class LocalData 
{
    /// <summary>
    /// 版本号
    /// </summary>
    public string version;
    /// <summary>
    /// 离线时间
    /// </summary>
    public DateTime leavegameTime;
    /// <summary>
    /// 第一次进入游戏
    /// </summary>
    public bool firstPlay;
    /// <summary>
    /// 当前金币
    /// </summary>
    public int currentMoney;
    
}
