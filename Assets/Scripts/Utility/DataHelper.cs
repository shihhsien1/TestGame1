using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataHelper
{
    public static void JsonSerializeData<T>(List<T> data, string directoryName, string file) where T : class
    {
        var serializeData = JsonConvert.SerializeObject(data);
        var savePath = Application.dataPath + $"/StreamingAssets/Datas/{directoryName}/{file}.json";
        if (string.IsNullOrEmpty(directoryName) == false)
        {
            savePath = Application.dataPath + $"/StreamingAssets/Datas/{directoryName}/{file}.json";
            var saveDirectoryPath = Application.dataPath + $"/StreamingAssets/Datas/{directoryName}";
            if (File.Exists(saveDirectoryPath) == false)
                Directory.CreateDirectory(saveDirectoryPath);
        }
        else
        {
            savePath = Application.dataPath + $"/StreamingAssets/Datas/{file}.json";
        }
        File.WriteAllText(savePath, serializeData);
    }
    public static List<T> JsonDeserializeData<T>(string file) where T : class
    {
        var loadPath = Application.dataPath + $"/StreamingAssets/Datas/{file}.json";
        if (File.Exists(loadPath) == false) return null;
        var jsonData = File.ReadAllText(loadPath);
        var datas = JsonConvert.DeserializeObject<List<T>>(jsonData);
        return datas;
    }
    public static void WriteByteArrayData(byte[] dataArray, string folderName, string fileName)
    {
        var savePath = Application.dataPath + $"/StreamingAssets/Datas/{folderName}/{fileName}.unity3d";
        var saveDirectoryPath = Application.dataPath + $"/StreamingAssets/Datas/{folderName}";
        if (File.Exists(saveDirectoryPath) == false)
            Directory.CreateDirectory(saveDirectoryPath);
        File.WriteAllBytes(savePath, dataArray);
    }

}
