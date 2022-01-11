using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// C#拓展
/// </summary>
public static partial class CSharpExtension
{
    /// <summary>
    /// 查找节点下的子物体
    /// 名字相同时返回第一个-Transform
    /// </summary>
    /// <param name="root"></param>
    /// <param name="objName"></param>
    /// <returns></returns>
    public static Transform FindAll(this Transform root, string objName) {
        if (root == null) return null;
        Transform target = root.Find(objName);
        if (target != null) return target;
        for (int i = 0; i < root.childCount; i++) {
            Transform tt = root.GetChild(i).FindAll(objName);
            if (tt != null) return tt;
        }
        return target;
    }
    /// <summary>
    /// 查找节点下的子物体
    /// 名字相同时返回第一个-GameObject
    /// </summary>
    /// <param name="root"></param>
    /// <param name="objName"></param>
    /// <returns></returns>
    public static Transform FindAll(this GameObject root, string objName)
    {
        if (root == null) return null;
        Transform target = root.transform.Find(objName);
        if (target != null) return target;
        for (int i = 0; i < root.transform.childCount; i++)
        {
            Transform tt = root.transform.GetChild(i).FindAll(objName);
            if (tt != null) return tt;
        }
        return target;
    }
   
    public static void SetActive(this Transform root, bool active) {
        if (root == null) return;
        root.gameObject.SetActive(active);
    }
    
}
