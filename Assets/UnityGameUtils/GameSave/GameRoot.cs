using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GameRoot : MonoSingleton<GameRoot>
{
    [SerializeField]
    private ExcelData excelData;
    [SerializeField]
    private LocalData localData;
    private string excelDataPath {
        get { return Application.dataPath + "/UnityGameUtils/GameSave/ExcelData.save"; }  
    }
    private string localDataPath
    {
        get { return Application.persistentDataPath + "/SaveData.txt"; }
    }
   
    public ExcelData ExcelData { get => excelData;}
    public LocalData LocalData { get => localData;}

    public override void Awake()
    {
        base.Awake();
        ReadExcelData();
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    private void ReadExcelData() {
        StartCoroutine(LoadExcel(excelDataPath));
    }
    IEnumerator LoadExcel(string path)
    {
#if UNITY_IOS
        path = "file://"+path;
#endif
        UnityWebRequest webRequest = UnityWebRequest.Get(path);
        yield return webRequest.SendWebRequest();
        if (webRequest.isDone)
        {
            byte[] data = webRequest.downloadHandler.data;
            string context = webRequest.downloadHandler.text;
            excelData = FileTool.Deserialize<ExcelData>(data);
        }
        else
        {
            Debug.LogError("Load Excel Dat:" + path + " Error：" + webRequest.error);
        }
        ReadLocalData();
    }
    /// <summary>
    /// 加载本地存档
    /// </summary>
    private void ReadLocalData() {
        localData = ReadData();
        if (localData == null)
        {
            InitLocalData();
        }
    }
    public int CompareVersion(string newVersion, string oldVersion)
    {
        if (string.IsNullOrEmpty(newVersion) || string.IsNullOrEmpty(oldVersion))
        {
            return 0;
        }
        Version v1 = new Version(newVersion);
        Version v2 = new Version(oldVersion);
        if (v1 > v2) return 1;
        else if (v1 == v2) return 0;
        else return -1;
    }
    void CopyOldData()
    {
        LocalData local = new LocalData();
        local.version = localData.version;
        localData = local;
    }
    /// <summary>
    /// 初始化存档
    /// </summary>
    void InitLocalData() {
        localData = new LocalData();
        localData.firstPlay = true;
        localData.version = "1.0";
        localData.currentMoney = 100;
    }
    public LocalData ReadData()
    {
        if (File.Exists(localDataPath))
        {
            byte[] data = File.ReadAllBytes(localDataPath);
            LocalData localData = FileTool.Deserialize<LocalData>(data);
            return localData;
        }
        return null;
    }
    private void OnDestroy()
    {
        SaveData();
    }
    private void OnApplicationQuit()
    {
        SaveData();
    }
    private bool isSave=false;
    private void SaveData()
    {
        if (isSave) return;
        isSave = true;
        localData.leavegameTime = DateTime.Now;
        Debug.Log("Offline time：" + localData.leavegameTime);
        FileTool.Serialize(localDataPath,localData);
    }
}
