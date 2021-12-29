using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class FileTool
{
    /// <summary>
    /// 获取指定目录下所有相同后缀名的文件名(带后缀)
    /// </summary>
    /// <param name="path"></param>
    /// <param name="endName"></param>
    /// <returns></returns>
    public static string[] GetAllFileName(string path,string endName) {
        if (path.Length == 0) return null;
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo [] fileInfos = directoryInfo.GetFiles("*"+endName);
        string[] fileNames =new string[fileInfos.Length];
        for (int i=0;i<fileInfos.Length;i++) {
            fileNames[i] = fileInfos[i].Name;
        }
        return fileNames;
    }
    /// <summary>
    /// 获取指定目录下所有相同后缀名的文件名(不带后缀)
    /// </summary>
    /// <param name="path"></param>
    /// <param name="endName"></param>
    /// <returns></returns>
    public static string[] GetAllFileNameNoEnd(string path, string endName)
    {
        if (path.Length == 0) return null;
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo[] fileInfos = directoryInfo.GetFiles("*" + endName);
        string[] fileNames = new string[fileInfos.Length];
        for (int i = 0; i < fileInfos.Length; i++)
        {
            fileNames[i] = fileInfos[i].Name.Replace('.'+endName,""); ;
        }
        return fileNames;
    }
    public static string[] GetAllFilePath(string path, string endName) {
        if (path.Length == 0) return null;
        return Directory.GetFiles(path,"*"+endName);
    }
    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    public static void Serialize(string path,object data) {
        BinaryFormatter binary = new BinaryFormatter();
        using (FileStream fileStream = File.Open(path,FileMode.OpenOrCreate)) {
            binary.Serialize(fileStream,data);
        }
    }
    public static T Deserialize<T>(byte [] buffer) where T : class
    {
        BinaryFormatter binary = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream(buffer)) {
            return binary.Deserialize(stream) as T;
        }
    }
}
