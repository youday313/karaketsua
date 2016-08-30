using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class JsonManager: SingletonBase<JsonManager>
{

    /// <summary>
    /// コールバックでオブジェクトを返す
    /// </summary>
    public void Load<T>(string fileName, Action<T> callback)
    {
        var filePath = fileName + ".json";
        using(var reader = new StreamReader(filePath)) {
            var txt = reader.ReadToEnd();
            var obj = JsonUtility.FromJson<T>(txt);
            callback(obj);
        }
    }

    /// <summary>
    /// オブジェクトデータJson形式でセーブする
    /// </summary>
    public void Save<T>(string fileName, T datas)
    {
        var filePath = fileName + ".json";
        var txt = JsonUtility.ToJson(datas);
        using(var writer = new StreamWriter(filePath)) {
            writer.WriteLine(txt);
        }
    }


}
