using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using System;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
/// <summary>
/// 编辑器快速生成类
/// </summary>
public class CreateScript 
{
    #region ui path setting
    static string outputPath = Application.dataPath + "/UnityGameUtils/UI/UIGenScripts/";
    static string outputUIPrefabPath = Application.dataPath + "/UnityGameUtils/UI/Resources/";
    static string outputPrefabPath = Application.dataPath + "/UnityGameUtils/UI/Resources/";
    private static string SelectName = "UIBase";//当前选择的UIPanel名字
    private static GameObject activeObject = null;//当前选择的游戏物体对象
    #endregion

    #region UI
    [MenuItem("GameObject/UI/生成UI脚本")]
    /// <summary>
    /// 脚本生成
    /// </summary>
    public static void ScriptGenerator() {
        if (Selection.activeObject == null) return;
        GameObject selectobj = Selection.activeObject as GameObject;
       
        //根据模板替换
        string bufferStr = Encoding.UTF8.GetString(File.ReadAllBytes(outputPath + "SamplePanel.cs"));
        string newStr= bufferStr.Replace("SamplePanel", selectobj.name);
        byte[] buffer =Encoding.UTF8.GetBytes(newStr);
        FileStream fs = new FileStream(outputPath + selectobj.name+".cs", FileMode.Create, FileAccess.ReadWrite);
        fs.Write(buffer,0,buffer.Length);
        fs.Close();
        EditorUtility.DisplayDialog("成功", "生成" + selectobj.name + "类成功,路径" + outputPath, "OK");
        AssetDatabase.Refresh();
    }
  
    /// <summary>
    /// UI预制体生成
    /// </summary>
    [MenuItem("GameObject/UI/生成UI预制体")]
    public static void UIPrefabGenerator() {
        if (Selection.activeObject == null) return;
        GameObject selectobj = Selection.activeObject as GameObject;
        
        GameObject obj = GameObject.Instantiate(Selection.activeGameObject as GameObject);
        obj.name = obj.name.Replace("(Clone)", string.Empty);
        PrefabUtility.SaveAsPrefabAsset(obj, outputUIPrefabPath + obj.name + ".prefab");
        Debug.Log("生成UI预制体成功：" + outputUIPrefabPath + obj.name + ".prefab");
        GameObject.DestroyImmediate(obj);
         
        AssetDatabase.Refresh();
       
    }
    /// <summary>
    /// 一键预制体生成(可多选)
    /// </summary>
    [MenuItem("GameObject/生成预制体")]
    public static void PrefabGenerator()
    {
        //if (Selection.activeObject == null) return;
        foreach (System.Object objs in Selection.gameObjects) {
            GameObject selectobj = objs as GameObject;
            GameObject obj = GameObject.Instantiate(selectobj);
            obj.name = obj.name.Replace("(Clone)", string.Empty);
            PrefabUtility.SaveAsPrefabAsset(obj, outputPrefabPath + obj.name + ".prefab");
            Debug.Log("生成预制体成功：" + outputUIPrefabPath + obj.name + ".prefab");
            GameObject.DestroyImmediate(obj);
            AssetDatabase.Refresh();
        }
    }
 
    [MenuItem("GameObject/UI/创建绑定UIPanel类")]
    /// <summary>
    /// 选择要绑定的UIBase
    /// </summary>
    public static void SelectPanel() {
        //1.选择继承了UIBase的UI面板，生成关联此面板的存放关联UI控件的类-类名.Attribute.cs
        if (File.Exists(outputPath + Selection.activeObject.name + ".Attribute.cs")) {
            SelectName = Selection.activeObject.name;
            EditorPrefs.SetString("SelectName", SelectName);
            Debug.Log("当前选择的UIPanel.Name:" + SelectName);;
            return;
        }
        GameObject obj = Selection.activeObject as GameObject;
        if (obj.GetComponent<UIBase>() != null)
        {
            SelectName = Selection.activeObject.name;
            activeObject = obj;
            CodeTypeDeclaration code = new CodeTypeDeclaration(SelectName);
            code.IsClass = true;
            code.TypeAttributes = System.Reflection.TypeAttributes.Public;
            code.IsPartial = true;
            string outPutPath = outputPath + SelectName + ".Attribute.cs";
            CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("CSharp");
            StreamWriter sw = new StreamWriter(outPutPath);
            codeDomProvider.GenerateCodeFromType(code, sw, null);
            sw.Close();
            sw.Dispose();
            AssetDatabase.Refresh();
            Debug.Log("当前选择的UIPanel.Name:"+SelectName);
            EditorPrefs.SetString("SelectName", SelectName);
        }
        else {
            EditorUtility.DisplayDialog("失败", "请先继承UIBase", "OK");
        }
    }
    /*
     完成第一步绑定面板后，选择基于此面板为根节点下的UI控件进行绑定：在存放关联UI控件的类中写入UI控件的字段，并在
     代码编译完成后将目标控件赋值给目标字段
     */
    [MenuItem("GameObject/UI/快速绑定UI/绑定Button")]
    public static void GernerateButton() {
        ComponentGenerator(ComponentType.Button);
    }
    [MenuItem("GameObject/UI/快速绑定UI/绑定Text")]
    public static void GernerateText()
    {
        ComponentGenerator(ComponentType.Text);
    }
    [MenuItem("GameObject/UI/快速绑定UI/绑定Image")]
    public static void GernerateImage()
    {
        ComponentGenerator(ComponentType.Image);
    }
    [MenuItem("GameObject/UI/快速绑定UI/绑定Transform")]
    public static void GernerateTransform()
    {
        ComponentGenerator(ComponentType.Transform);
    }
    /// <summary>
    /// 快速生成控件并绑定到UI面板
    /// </summary>
    /// <param name="componentType"></param>
    public static void ComponentGenerator(ComponentType componentType) {
        //在根节点的UIPanel中添加选中UI的字段并赋值
        SelectName = EditorPrefs.GetString("SelectName");
        //Type.GetType(activeObject.GetComponent<UIBase>().name);
        //若文件已存在
        string path = outputPath + SelectName + ".Attribute.cs";
        if (File.Exists(path))
        {
            //先检测这个控件是否存在了
            string codeText = File.ReadAllText(path);
            StringBuilder typeName = new StringBuilder();
            StringBuilder type = new StringBuilder();
            #region 判断该游戏物体上是否存在目标控件
            switch (componentType)
            {
                case ComponentType.Button:
                    typeName.Append("UI.Button");
                    type.Append("Button");
                    if ((Selection.activeObject as GameObject).GetComponent<Button>() == null)
                    {
                        EditorUtility.DisplayDialog("失败", "对象不存在控件" + type.ToString(), "OK");
                        return;
                    }
                    break;
                case ComponentType.Image:
                    typeName.Append("UI.Image");
                    type.Append("Image");
                    if ((Selection.activeObject as GameObject).GetComponent<Image>() == null)
                    {
                        EditorUtility.DisplayDialog("失败", "对象不存在控件" + type.ToString(), "OK");
                        return;
                    }
                    break;
                case ComponentType.Text:
                    typeName.Append("UI.Text");
                    type.Append("Text");
                    if ((Selection.activeObject as GameObject).GetComponent<Text>() == null)
                    {
                        EditorUtility.DisplayDialog("失败", "对象不存在控件" + type.ToString(), "OK");
                        return;
                    }
                    break;
                case ComponentType.Transform:
                    typeName.Append("Transform");
                    type.Append("Transform");
                    break;
            }
            #endregion
            //判断该游戏物体是否在目标面板节点下
            Transform tt = (GameObject.Find(SelectName)).transform.FindAll(Selection.activeObject.name);
            if (tt == null)
            {
                EditorUtility.DisplayDialog("失败", "请选择在UI面板"+SelectName+"节点下的控件进行绑定", "OK");
                return;
            }
            //判断UI面板是否绑定过该控件
            if (codeText.Contains(Selection.activeObject.name + "_" + type))
            {
                //Debug.Log("控件"+ Selection.activeObject.name +"已存在，请重复绑定...");
                EditorUtility.DisplayDialog("失败", "控件" + Selection.activeObject.name + "_" + type + "已绑定，请勿重复绑定", "OK");
            }
            else {
                System.IO.StreamReader sr = new System.IO.StreamReader(path);
                int iLine = 2;
                string strInsert = "\t" + "public UnityEngine." + typeName.ToString() + " " + Selection.activeObject.name +"_"+ type + ";";
                string strText = ""; //用来存储更改后的文本内容
                int lines = 0; //记录文件行数
                while (!sr.EndOfStream)
                {
                    lines++;
                    if (lines == iLine)
                    {
                        strText += strInsert + "\n";
                    }
                    string str = sr.ReadLine();
                    strText += str + "\n";
                }
                sr.Close();
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(path, false))
                {
                    sw.Write(strText);
                }
                EditorPrefs.SetString("CallBack", "True");//设置回调，当代码重新编译时，则会执行带有DidReloadScripts标记的代码
                EditorPrefs.SetString("SelectComponentName", Selection.activeObject.name);//记录当前选择的控件名字
                EditorPrefs.SetString("ScriptComponentName", Selection.activeObject.name + "_" + type);//记录脚本中的控件名字
                EditorPrefs.SetString("SelectComponentType", type.ToString());//记录当前选择的控件类型 
                EditorPrefs.SetString("SelectPanelName", SelectName);//记录当前选择的控件类型
                AssetDatabase.Refresh();
            }
        }
        else
        {
            //文件不存在
            Debug.LogError("当前未绑定UIPanel类");
        }
    }

    /// <summary>
    /// 绑定按钮回调方法，将目标控件赋值到UI面板上
    /// </summary>
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void BindComponentCallBack() {

        if (EditorPrefs.GetString("CallBack") == "False")
        {
            return;
        }
        else {
          
            SelectName= EditorPrefs.GetString("SelectPanelName");
            //SelectName=面板名字
            string componentName = EditorPrefs.GetString("SelectComponentName");
            string componentType = EditorPrefs.GetString("SelectComponentType");
            string scriptComponentName = EditorPrefs.GetString("ScriptComponentName");
            Component cc;
            GameObject go = GameObject.Find(SelectName);
            if (go == null)
            {
                //EditorUtility.DisplayDialog("失败", "UI面板" + SelectName + "找不到" + componentType + "控件" + scriptComponentName + ",绑定失败...", "OK");
                EditorUtility.DisplayDialog("失败", string.Format("UI面板 '{0}' 上找不到 '{1}' 控件 '{2}' ,绑定失败...", SelectName, componentType, scriptComponentName), "OK");
            }
            else
            {
                Transform tt = go.transform.FindAll(componentName);
                if (tt == null)
                {
                    EditorUtility.DisplayDialog("失败", string.Format("UI面板 '{0}' 上找不到 '{1}' 控件 '{2}' ,绑定失败...", SelectName, componentType, scriptComponentName), "OK");
                }
                else
                {
                    cc = tt.GetComponent(componentType);
                    if (cc != null)
                    {
                        var obj = default(SerializedObject);
                        obj = new SerializedObject(GameObject.Find(SelectName).GetComponent(SelectName));
                        obj.FindProperty(scriptComponentName).objectReferenceValue = cc;
                        obj.ApplyModifiedPropertiesWithoutUndo();
                        Debug.Log("UI面板" + SelectName + "绑定" + componentType + "控件" + scriptComponentName + "成功...");
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("失败", "UI面板" + SelectName + "绑定" + componentType + "控件" + scriptComponentName + "失败...", "OK");
                    }
                }
            }
            ClearEditorPrefs();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());//将当前场景标记为修改过
        }
        
    }
    /// <summary>
    /// 清除编辑器缓存数据
    /// </summary>
    private static void ClearEditorPrefs() {
        EditorPrefs.SetString("CallBack", "False");
        EditorPrefs.DeleteKey("SelectComponentName");
        EditorPrefs.DeleteKey("SelectComponentType");
    }

    #endregion
    
    [System.Obsolete]
    static void Gen() {
        #region CodeTypeDeclaration生成方式1
        //CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(selectobj.name);
        //codeTypeDeclaration.IsClass = true;   
        //codeTypeDeclaration.TypeAttributes = System.Reflection.TypeAttributes.Public;
        //codeTypeDeclaration.BaseTypes.Add("UIBase");
        //CodeDomProvider codeDom = CodeDomProvider.CreateProvider("CSharp");
        //if (!Directory.Exists(outputPath)) {
        //     Directory.CreateDirectory(outputPath); 
        //} 
        //CodeGeneratorOptions options = new CodeGeneratorOptions(); 
        //options.BracingStyle = "C";
        //options.BlankLinesBetweenMembers = true;
        ////添加引用
        //CodeNamespace codeNamespace = new CodeNamespace();
        //codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
        //codeNamespace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
        //codeNamespace.Imports.Add(new CodeNamespaceImport("System.Collections"));
        //codeNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
        //using (StreamWriter streamWriter = new StreamWriter(outputPath+ Selection.activeObject.name+".cs")) {
        //    codeDom.GenerateCodeFromNamespace(codeNamespace, streamWriter, options);
        //    codeDom.GenerateCodeFromType(codeTypeDeclaration, streamWriter, options);
        //} 

        #endregion
    }
}
/// <summary>
/// 控件类型
/// </summary>
public enum ComponentType
{
    Button,
    Text,
    Image,
    Transform
}
