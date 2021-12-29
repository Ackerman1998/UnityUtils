using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using Excel;
using System.Data;
using System.CodeDom;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
public class ExcelEditor 
{
    private static string excelReadPath = Application.dataPath + "/UnityGameUtils/GameSave/Excel";
    private static string excelClassOutputPath = Application.dataPath + "/UnityGameUtils/GameSave/ExcelClass";
    private static string excelDataOutputPath = Application.dataPath + "/UnityGameUtils/GameSave";
    [MenuItem("UnityUtils/Clear GameSave Data")]
    public static void Clear() {
        if (File.Exists(Application.persistentDataPath + "/SaveData.txt"))
        {
            File.Delete(Application.persistentDataPath + "/SaveData.txt");
            Debug.Log("清除本地存档完毕...");
        }
    }

    [MenuItem("UnityUtils/Gen ExcelData Class")]
    /// <summary>
    /// 生成类
    /// </summary>
    public static void GenerateCSharp() {
        if (!Directory.Exists(excelReadPath)) {
            Debug.LogError("路径:"+ excelReadPath + "不存在..."); return;
        }
        string [] fileNames = FileTool.GetAllFileNameNoEnd(excelReadPath, "xlsx");
        string [] filePathNames = FileTool.GetAllFilePath(excelReadPath, "xlsx");
        for (int i=0;i<fileNames.Length;i++) {
            FileStream fs = new FileStream(filePathNames[i],FileMode.Open);
            IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
            DataSet dataSet = excelDataReader.AsDataSet();
            DataRowCollection rowCollection = dataSet.Tables[0].Rows;//行数
            //生成CS
            CodeTypeDeclaration csharp = new CodeTypeDeclaration(fileNames[i]);
            csharp.IsClass = true;
            csharp.TypeAttributes = System.Reflection.TypeAttributes.Public;
            csharp.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference("System.Serializable")));
            for (int j=0;j<dataSet.Tables[0].Columns.Count;j++) { //列数
                string filedName = rowCollection[0][j].ToString();//第一行：属性名
                string type = rowCollection[2][j].ToString();//第三行：变量名
                if (type.Length==0|| filedName.Length==0) {
                    continue;
                }
                CodeMemberField codeMemberField = new CodeMemberField(GetType(type), filedName);
                codeMemberField.Attributes = MemberAttributes.Public;
                csharp.Members.Add(codeMemberField);
            }
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();    //代码生成风格
            options.BracingStyle = "C";
            options.BlankLinesBetweenMembers = true;
            if (!Directory.Exists(excelClassOutputPath)) {
                Directory.CreateDirectory(excelClassOutputPath);
            }
            string outPath = excelClassOutputPath + @"\" + fileNames[i] + ".cs";
            using (StreamWriter sw = new StreamWriter(outPath))
            {
                provider.GenerateCodeFromType(csharp, sw, options);
            }
            Debug.Log("生成CSharp:"+ fileNames[i] + "完毕...");
            AssetDatabase.Refresh();
        }
    }
    [MenuItem("UnityUtils/Write ExcelData")]
    /// <summary>
    /// 数据写入
    /// </summary>
    public static void WriteExcelData() {
        ExcelData excelData = new ExcelData();
        string[] classNames = FileTool.GetAllFileNameNoEnd(excelReadPath, "xlsx");
        string[] filePathNames = FileTool.GetAllFilePath(excelReadPath, "xlsx");
        for (int i=0;i<filePathNames.Length;i++) {
            FileStream fs = new FileStream(filePathNames[i],FileMode.Open);
            WriteData(classNames[i], fs, excelData);
        }
        string genExcelDataPath = excelDataOutputPath + "/ExcelData.save";
        if (!Directory.Exists(excelDataOutputPath))
        {
            Directory.CreateDirectory(excelDataOutputPath);
        }
        FileTool.Serialize(genExcelDataPath, excelData);
        Debug.Log("写入数据完毕...");
        AssetDatabase.Refresh();
    }
    private static void WriteData(string className, FileStream fileStream, ExcelData data) {
        switch (className)
        {
            case "RoleConfig":
                data.roleConfigs = LoadData<RoleConfig>(fileStream);
                break;
        }
    }
    private static List<T> LoadData<T>(Stream stream) where T : class, new() {
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet dataSet = excelDataReader.AsDataSet();
        if (dataSet.Tables == null) return null;
        DataTable dataTable = dataSet.Tables[0];
        List<T> value = new List<T>();
   
        for (int i=3;i<dataTable.Rows.Count;i++) { //从表格第四行开始遍历
            T instance = new T();
            FieldInfo[] fileInfos = typeof(T).GetFields();
            for (int j=0;j<dataTable.Columns.Count;j++) {
                string fieldType = dataTable.Rows[2][j].ToString();//获取变量类型
                string val = dataTable.Rows[i][j].ToString();
                if (val.Length==0)
                {
                    continue;
                }
                Debug.Log("变量类型:"+ fieldType+ "，变量名:" + fileInfos[j] + "，变量值:" + dataTable.Rows[i][j].ToString());
                fileInfos[j].SetValue(instance, SwitchType(fieldType,dataTable.Rows[i][j]));
            }
            value.Add(instance);
        }
        return value;
    }
    /// <summary>
    /// 获取类型
    /// </summary>
    /// <returns></returns>
    private static Type GetType(string type)
    {
        switch (type)
        {
            case "string":
                return typeof(String);
            case "int":
                return typeof(Int32);
            case "float":
                return typeof(Single);
            case "long":
                return typeof(Int64);
            case "double":
                return typeof(Double);
            default:
                return typeof(String);
        }
    }
    private static object SwitchType(string type, object value)
    {
        string str = value.ToString();
        switch (type)
        {
            case "string":
                return value;
            case "int":
                return int.Parse(str);
            case "float":
                return float.Parse(str);
            case "long":
                return long.Parse(str);
            case "double":
                return double.Parse(str);
            default:
                return value;
        }
    }
}
