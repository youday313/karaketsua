using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class JsonManager: DontDestroySingleton<JsonManager>
{

    private string localFilePath;
    void Awake()
    {
        localFilePath = Application.persistentDataPath + "/download/";
        Directory.CreateDirectory(localFilePath);

#if UNITY_IOS && !UNITY_EDITOR
        // temporaryCachePath/downloadをpersistentDataPath/に移動する(ios専用)
        var tempPath = Application.temporaryCachePath + "/download/";
        if(Directory.Exists(tempPath)) {
            Directory.Delete(localFilePath, true);
            Directory.Move(tempPath, localFilePath);
        }
        // icouldへリソースのバックアップを外す
        Device.SetNoBackupFlag( localFilePath );
#endif

        tryCreateMasterDataFileWhenEmpty();

    }



    /// <summary>
    /// 初回起動かデータリセット時に作成
    /// </summary>
    private void tryCreateMasterDataFileWhenEmpty()
    {
        if(!Directory.Exists(localFilePath)) {
            return;
        }
        CopyMasterDataFromStreamingAssets();
    }


    /// <summary>
    /// SteramingAssetsのファイルを全てコピーする
    /// </summary>
    public void CopyMasterDataFromStreamingAssets()
    {
        var streamingPath = Application.streamingAssetsPath;
        DirectoryProcessor.CopyAndReplace(Application.streamingAssetsPath, localFilePath);
    }

    /// <summary>
    /// コールバックでオブジェクトを返す
    /// </summary>
    public void Load<T>(string fileName, Action<T> callback)
    {
        var filePath = localFilePath + fileName + ".json";

        using(var reader = new StreamReader(filePath)) {
            var txt = reader.ReadToEnd();
            var obj = JsonUtility.FromJson<T>(txt);
            callback(obj);
        }
    }
}
