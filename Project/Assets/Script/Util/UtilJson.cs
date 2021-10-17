using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;

public class UtilJson
{
    private static string GetJsonString(string path, string fileName)
    {
        string retFilePath = string.Format("{0}/{1}.json", path, fileName);
        return retFilePath;
    }

    public static void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(GetJsonString(createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    public static void RemoveJsonFile(string removePath, string fileName)
    {
        string loadFilePath = GetJsonString(removePath, fileName);
        if(File.Exists(loadFilePath))
        {
            File.Delete(loadFilePath);
        }
    }

    public static string LoadJsonFile(string loadPath, string fileName)
    {
        string loadFilePath = GetJsonString(loadPath, fileName);
        if (File.Exists(loadFilePath))
        {
            FileStream fileStream = new FileStream(loadFilePath, FileMode.Open);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            string jsonData = Encoding.UTF8.GetString(data);
            return jsonData;
        }
        else
        {
            return string.Empty;
        }
    }
}
