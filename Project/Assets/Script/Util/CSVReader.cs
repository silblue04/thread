using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System;
using System.Text;
using Newtonsoft.Json;


public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, string>> Read(string file)
    {
#if UNITY_EDITOR
        List<Dictionary<string, string>> list = null;
        string source = string.Empty;
        string path = Application.dataPath + "/../../GameData/Datas/" + file + ".csv";

        StreamReader sr = new StreamReader(path);
        source = sr.ReadToEnd();
        sr.Close();

        var lines = Regex.Split(source, LINE_SPLIT_RE);
        if (lines.Length <= 1) return list;

        list = new List<Dictionary<string, string>>();
        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, string>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                entry[header[j]] = value;
            }
            list.Add(entry);
        }

        return list;
#else
        string path = string.Format("Metas/{0}", file);
        TextAsset data = Resources.Load(path) as TextAsset;

        Blowfish blowFish = new Blowfish("MafGamesMergeVillage");
        string decodedText = blowFish.Decipher(data.text);

        return JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(decodedText);
#endif
    }
}