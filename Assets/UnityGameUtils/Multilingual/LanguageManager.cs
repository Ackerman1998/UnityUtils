using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 多语言管理器
/// </summary>
public class LanguageManager : MonoSingleton<LanguageManager>
{
    public GameTextAll gameTexts;
    /// <summary>
    /// 当前使用的语言
    /// </summary>
    public LanguageType languageType;
    public override void Awake()
    {
        base.Awake();
        //读取当前语言
        if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
        {
            languageType = LanguageType.Chinese;
        }
        else if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
        {
            languageType = LanguageType.TraditionalChinese;
        }
        else if (Application.systemLanguage == SystemLanguage.English)
        {
            languageType = LanguageType.English;
        }
        else {
            languageType = LanguageType.English;
        }
        string json = ResourceManager.Instance.LoadByResName<TextAsset>("GameText").text;
        print(json);
        GameTextAll game = JsonMapper.ToObject<GameTextAll>(json);
        foreach (GameText gameTextAll in game.gameTexts) {
            gameTexts.gameTexts.Add(gameTextAll);
        }
    }
    /// <summary>
    /// 返回对应的语言文字
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string GetString(string text) {
        if (gameTexts.gameTexts.Find((x) => (x.langnuage == languageType.ToString())) != null) {
            if (gameTexts.gameTexts.Find((x) => (x.langnuage == languageType.ToString())).textDatas.Find((x)=>(x.key==text))!=null) {
                return gameTexts.gameTexts.Find((x) => (x.langnuage == languageType.ToString())).textDatas.Find((x) => (x.key == text)).value;
            }
        }
        return "";
    }
}
/// <summary>
/// 语言类型
/// </summary>
public enum LanguageType{
    English,
    Chinese,
    TraditionalChinese,
    Korean
}
