using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// IPool
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPool<T>
{
    /// <summary>
    /// 回收对象
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    void Recyle(T obj);
    /// <summary>
    /// 分配对象
    /// </summary>
    /// <returns></returns>
    T Allocate();
    /// <summary>
    /// 分配对象
    /// </summary>
    /// <returns></returns>
    T Allocate(string objName);
}
/// <summary>
/// 常规对象池
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class NormalPool<T> : IPool<T>
{
    protected IGen factory = null;
    protected readonly Stack<T> cacheStack = new Stack<T>();
    public virtual T Allocate(string objName)
    {
        return default(T);
    }
    public virtual T Allocate()
    {
        return default(T);
    }
    public abstract void Recyle(T obj);
}
/// <summary>
/// 特殊对象池(指定参数)
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SpecialPool<T> : IPool<T>
{
    protected SGen factory = null;
    protected readonly Stack<T> cacheStack = new Stack<T>();
    public virtual T Allocate(string objName)
    {
        return default(T);
    }
    public abstract void Recyle(T obj);

    public virtual T Allocate()
    {
        return default(T);
    }
}
public class GameObjectPool<T> : SpecialPool<T> where T : Object
{
    object objs = new object();
    public GameObjectPool(SGen factory)
    {
        this.factory = factory;
    }
    public override T Allocate(string objName)
    {
        lock (objs)
        {
            return cacheStack.Count == 0 ? factory.Create<T>(objName) : cacheStack.Pop();
        }
    }
    public override void Recyle(T obj)
    {
        lock (objs)
        {
            if (!cacheStack.Contains(obj))
            {
                cacheStack.Push(obj);
            }
        }
    }
}