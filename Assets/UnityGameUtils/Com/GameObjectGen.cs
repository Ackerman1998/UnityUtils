using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏物体生成器-GameObject类型
/// 要加载的Prefab一律放在Resources文件夹下
/// </summary>
public class GameObjectGen : MonoSingleton<GameObjectGen>, SGen
{
    public override void Awake()
    {
        
    }
    private Dictionary<string, IPool<GameObject>> dictPool = new Dictionary<string, IPool<GameObject>>();
    int index = 0;
    public T Create<T>(string objName) where T : Object
    {
        GameObject mm = Instantiate(CreateGameObjectByRes(objName));
        return mm as T;
    }
    /// <summary>
    /// 创建重复游戏对象
    /// </summary>
    public GameObject CreateGameObjectRepeat(string objName,Transform parent) {
        //创建一个没有创建过的对象，创建一个新的对象池，
        IPool<GameObject> pool;
        if (!dictPool.ContainsKey(objName))
        {
            pool = new GameObjectPool<GameObject>(this);
            dictPool.Add(objName, pool);
        }
        else {
            pool = dictPool[objName];
        }
        GameObject obj = pool.Allocate(objName);
        if (parent!=null) {
            obj.transform.parent = parent;
        }
        obj.gameObject.SetActive(true);
        return obj;
    }
    /// <summary>
    /// 创建对象,常规，非重复
    /// </summary>
    public GameObject CreateGameObject(string name) {
        GameObject mm = Instantiate(CreateGameObjectByRes(name));
        mm.name = name;
        return mm;
    }
    public GameObject CreateGameObject(string name,Transform parent)
    {
        GameObject mm = Instantiate(CreateGameObjectByRes(name), parent);
        mm.name = name;
        return mm;
    }
    /// <summary>
    /// 创建游戏，通过加载resource资源
    /// </summary>
    private GameObject CreateGameObjectByRes(string name) {
        return ResourceManager.Instance.LoadByResName<GameObject>(name);
    }
    /// <summary>
    /// 回收游戏物体
    /// </summary>
    /// <param name="recyleObj"></param>
    public void RecycleGameObject(GameObject recyleObj,string objName) {
        if (dictPool.ContainsKey(objName))
        {
            //回收
            dictPool[objName].Recyle(recyleObj);
            recyleObj.gameObject.SetActive(false);
        }
        else { 
            //游戏物体没有对应的池子
        }
    }
    public void RecycleGameObject(GameObject recyleObj)
    {
        if (dictPool.ContainsKey(recyleObj.name))
        {
            //回收
            dictPool[recyleObj.name].Recyle(recyleObj);
        }
        else
        {
            //游戏物体没有对应的池子
        }
    }
   
}
public enum EffectType { 
    /// <summary>
    /// 球碎开的特效
    /// </summary>
    BallBroken,
    LightBallBomb,
    Yanhua
}