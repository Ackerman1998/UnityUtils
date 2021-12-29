
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Resource资源加载(Prefab,Sprite,文本文件,UI,材质球)默认不添加Texture2D,只添加Sprite
/// </summary>
public class ResourceManager : MonoSingleton<ResourceManager>
{
    bool load = false;
    private Dictionary<string, Object> resDictContainer = new Dictionary<string, Object>();
    public override void Awake()
    {
        base.Awake();
        var start = System.DateTime.Now;
        LoadAllRes();
    }
    /// <summary>
    /// 加载所有资源
    /// </summary>
    private void LoadAllRes() {
        if (load)
        {
            return;
        }
        load = true;
        Object [] objs = Resources.LoadAll("");
        for (int i=0;i<objs.Length;i++) {
            if (objs[i].GetType()==typeof(UnityEngine.Texture2D)) {
                //Texture2D不添加，只添加Sprite
                continue;
            }
            if (resDictContainer.ContainsKey(objs[i].name))
            {
                Debug.LogError("添加Resources资源失败,已存在同名资源:" + objs[i].name + "");
            }
            else
            {
                resDictContainer.Add(objs[i].name, objs[i]);
            }
        }
    }
    /// <summary>
    /// 加载通过资源名，特殊:加载图片Sprite,通过转化Texture2D为sprite
    /// </summary>
    /// <param name="name"></param>
    public T LoadByResName<T>(string name) where T :Object {
        if (!load) {
            //没加载，先加载
            LoadAllRes();
        }
        if (resDictContainer.ContainsKey(name))
        {
            return (T)(resDictContainer[name]);
        }
        else {
            return default(T);
        }
    }
}
