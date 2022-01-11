using UnityEngine;
/// <summary>
/// 常规生成器（不带参）
/// </summary>
public interface IFactory
{
    T Create<T>() where T : Object;
    void Reset();
}
/// <summary>
/// 特殊生成器（带参）
/// </summary>
public interface SFactory
{
    T Create<T>(string objName,Transform target=null) where T : Object;
    void Reset();
}