using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMultilingual : MonoBehaviour
{
    private void Start()
    {
        ChangeUIText();
    }
    private void ChangeUIText() {
        transform.Find("Bg/TestText").GetComponent<Text>().text = LanguageManager.Instance.GetString("开始");
    }
    public void Btn_Chinese() {
        LanguageManager.Instance.languageType = LanguageType.Chinese;
        ChangeUIText();
    }
    public void Btn_English()
    {
        LanguageManager.Instance.languageType = LanguageType.English;
        ChangeUIText();
    }
    public void Btn_TraditionalChinese()
    {
        LanguageManager.Instance.languageType = LanguageType.TraditionalChinese;
        ChangeUIText();
    }
}
