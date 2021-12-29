using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Excel;
using System.IO;
using System.Data;
using LitJson;
using System.Text.RegularExpressions;
using System;
using System.Text;
using UnityEditor;
public class MultilingualExcel 
{
    public static string excelPath = Application.dataPath + "/UnityGameUtils/Multilingual/GameText.xlsx";
    public static string excelJsonOutputPath = Application.dataPath + "/UnityGameUtils/Multilingual/Resources/GameText.txt";
    [MenuItem("UnityUtils/MultilingualJsonGen")]
    /// <summary>
    /// excel转json 游戏内部UI文字
    /// </summary>
    public static void ExcelToJson_GameText()
    {
        FileStream fs = File.Open(excelPath, FileMode.Open);
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
        DataSet dataSet = excelDataReader.AsDataSet();
        if (dataSet.Tables == null)
        {
            return;
        }
        GameTextAll gameTextAll = new GameTextAll();
        gameTextAll.gameTexts = new List<GameText>();
        DataTable dataTable = dataSet.Tables[0];
        for (int i = 0; i < dataTable.Columns.Count; i++)
        {//遍历列 4
            GameText gameText = new GameText();
            gameText.textDatas = new List<TextData>();
            string language = dataTable.Rows[0][i].ToString();
            gameText.langnuage = language;
            for (int j = 1; j < dataTable.Rows.Count; j++)
            { //遍历行
                string val = dataTable.Rows[j][i].ToString();
                string key = dataTable.Rows[j][0].ToString();
                TextData textData = new TextData();
                textData.key = key;
                textData.value = val;
                gameText.textDatas.Add(textData);
            }
            gameTextAll.gameTexts.Add(gameText);
        }
        string json = JsonMapper.ToJson(gameTextAll);
        //转码
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });

        byte[] buffer = Encoding.UTF8.GetBytes(json);
        //存入到本地文件中
        if (!File.Exists(excelJsonOutputPath))
        {
            FileStream st = File.Create(excelJsonOutputPath);
            st.Close();
        }
        FileStream jsonStream = new FileStream(excelJsonOutputPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        jsonStream.Write(buffer, 0, buffer.Length);
        Debug.Log("转换Json完成:" + json);
    }
    
}
