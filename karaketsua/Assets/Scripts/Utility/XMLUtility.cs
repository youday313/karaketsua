using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;


public class XMLUtility
{
    // シリアライズ
    public static T Seialize<T>(string filename, T data)
    {
        using (StreamWriter stream = new StreamWriter(filename, false, System.Text.Encoding.UTF8))
        {
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, data);
            return data;
        }
        //StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8);
        //using (var stream = new XmlWriter.Create(filename, FileMode.Create))
        //{
        //    var serializer = new XmlSerializer(typeof(T));
        //    serializer.Serialize(stream, data);
        //}
        //return data;
    }

    // デシリアライズ
    public static T Deserialize<T>(string filename)
    {
        using (var stream = new FileStream(filename, FileMode.Open))
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }
    }

    //XMLのファイルパスを返す
    public static string GetFilePath()
    {
        //return Application.persistentDataPath;
        //return Application.streamingAssetsPath;
        var dataPath= Application.dataPath;
        var strParentDir = System.IO.Directory.GetParent(dataPath).FullName;
        strParentDir += "/DataFiles/";
        return strParentDir;

    }
}