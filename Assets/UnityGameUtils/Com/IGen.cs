using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 生成接口-常规
/// </summary>
public interface IGen 
{
    T Create<T>();
}
/// <summary>
/// 生成接口-带参数
/// </summary>
public interface SGen
{
    T Create<T>(string objName) where T : Object;
}