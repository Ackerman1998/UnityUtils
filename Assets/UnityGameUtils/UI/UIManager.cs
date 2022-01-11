using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ui管理器
/// </summary>
public class UIManager : MonoSingleton<UIManager>
{
    private UIManager() {

    }
    private Dictionary<string, UIBase> uiDict = new Dictionary<string, UIBase>();
    public MoveList<UIBase> uiMoveList = new MoveList<UIBase>();
    /// <summary>
    /// 游戏UI Canvas
    /// </summary>
    public Transform UICanvasTran {
        get {
            GameObject uiCan = GameObject.Find("GameUI");
            if (uiCan == null) {
                uiCan = new GameObject("GameUI");
                uiCan.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                uiCan.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                uiCan.GetComponent<CanvasScaler>().referenceResolution = new Vector2(640, 1136);
                uiCan.AddComponent<GraphicRaycaster>();
            }
            return uiCan.transform;
        }
    }
    public RectTransform UICanvasRect {
        get
        {
            return UICanvasTran.GetComponent<RectTransform>();
        }

    }
    public void AddUI(UIBase uIBase) {
        string typeName = uIBase.GetType().Name;
        if (uiDict.ContainsKey(typeName)) {
            return;
        }
        uiDict.Add(typeName, uIBase);
        //GameLog.PrintCom("添加UI:"+typeName+"成功...");
    }
    public void RemoveUI(UIBase uIBase) {
        string typeName = uIBase.GetType().Name;
        if (uiDict.ContainsKey(typeName))
        {
            uiDict.Remove(typeName);
        }
    }
    /// <summary>
    /// 打开一个面板，并激活
    /// </summary>
    /// <param name="uiname"></param>
    /// <returns></returns>
    private UIBase OpenUI(string uiname) {
        Transform uiCan = UICanvasRect;
        if (uiDict.ContainsKey(uiname)) {
            if (uiDict[uiname] != null)
            {
                uiDict[uiname].transform.SetAsLastSibling();
                if (uiMoveList.Contains(uiDict[uiname]))
                { //已添加,交换
                    if (uiMoveList.Count >= 2) uiMoveList[uiMoveList.Count - 1].OnPasue();
                    uiMoveList.Move(uiDict[uiname], uiMoveList.Count);
                }
                else {
                    if (uiMoveList.Count >= 1) uiMoveList[uiMoveList.Count - 1].OnPasue();
                    uiMoveList.Add(uiDict[uiname]);
                }
                uiDict[uiname].OnEnter();
                return uiDict[uiname];
            }
            else {
                uiDict.Remove(uiname);
            }
        }
        //GameObject resource = GameObjectFactory.Instance.CreateRes<GameObject>(uiname, uiCan);
        GameObject resource = GameObjectGen.Instance.CreateGameObject(uiname, UICanvasTran);
        //resource.transform.parent = UICanvasTran;
        //GameObject resource =null;
        UIBase uIBase = resource.GetComponent<UIBase>();
        uIBase.transform.SetAsLastSibling();
        AddUI(uIBase);
        if (uiMoveList.Contains(uiDict[uiname]))
        { //已添加,交换
            if (uiMoveList.Count >= 2) uiMoveList[uiMoveList.Count - 1].OnPasue();
            uiMoveList.Move(uiDict[uiname], uiMoveList.Count);
        }
        else
        {
            if (uiMoveList.Count >= 1) uiMoveList[uiMoveList.Count - 1].OnPasue();
            uiMoveList.Add(uiDict[uiname]);
        }
        uIBase.OnEnter();
        return uIBase;
    }
    /// <summary>
    /// 生成一个面板，不激活，默认放在ui最后面
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    private UIBase OpenUIForGet(string uiname) {
        Transform uiCan = UICanvasRect;
        if (!uiDict.ContainsKey(uiname)) {
            //GameObject resource = GameObjectFactory.Instance.CreateRes<GameObject>(uiname, uiCan);
            //GameObject resource = null;
            GameObject resource = GameObjectGen.Instance.CreateGameObject(uiname, UICanvasTran);
            //resource.transform.parent = UICanvasTran;
            UIBase uIBase = resource.GetComponent<UIBase>();
            uIBase.transform.SetAsFirstSibling();
            resource.gameObject.SetActive(false);
            AddUI(uIBase);
            uiMoveList.Add(uiDict[uiname]);
            if (uiMoveList.Count >= 2) uiMoveList.Move(uiDict[uiname], uiMoveList.Count - 2);
            return uIBase;
        }
        return null;
    }
    /// <summary>
    /// 打开指定UI面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T OpenUI<T>() where T : UIBase {
        string uiName = typeof(T).Name;
        if (string.IsNullOrEmpty(uiName) || uiName.Length == 0) {
            Debug.Log("UI:" + uiName + "不存在...");
            return null;
        }
        return OpenUI(uiName) as T;
    }
    /// <summary>
    /// 得到指定UI面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetUI<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        if (uiDict.ContainsKey(uiName)) {//面板已经生成了
            return uiDict[uiName] as T;
        }
        //面板还没生成,先生成
        T ui = OpenUIForGet(uiName) as T;
        return ui;
    }
    /// <summary>
    /// 关闭面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void CloseUI<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        CloseUI(uiName);
    }
    public void CloseUI(string uiName) {
        if (uiDict.ContainsKey(uiName)) {
            uiDict[uiName].OnExit();
            if (uiMoveList.Count > 0) {
                uiMoveList.RemoveAt(uiMoveList.Count - 1);
                if (uiMoveList.Count > 0) {
                    uiMoveList[uiMoveList.Count - 1].OnResume();
                    //CoroutineMgr.Instance.Open(() => {
                    //    uiMoveList[uiMoveList.Count - 1].transform.SetAsLastSibling();
                    //}, 0.3f);

                }
            }

            // uiMoveList[uiMoveList.Count - 1].transform.SetAsLastSibling();
            //print("close panel"+ uiName+",open panel"+ uiMoveList[uiMoveList.Count - 1].name);
            //OpenUI(uiMoveList[uiMoveList.Count - 1].name);
        }
    }

    public void CloseUI(UIBase uIBase) {
        CloseUI(uIBase.GetType().Name);
    }
    /// <summary>
    /// 关闭所有面板
    /// </summary>
    public void CloseAllUI() {
        for (int i = uiMoveList.Count - 1; i >= 0; i++) {
            uiMoveList[i].OnExit();
            uiMoveList.Remove(uiMoveList[i]);
        }
    }

 
}
