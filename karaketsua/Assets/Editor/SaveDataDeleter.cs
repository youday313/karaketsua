using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;


// MasterDataLoaderのSaveDataを削除する
// 
public class SaveDataDeleter: UnityEditor.EditorWindow
{

	[MenuItem("Window/DeleteMaster")]
	private static void DeleteMaster()
	{
		var localFilePath = Application.persistentDataPath + "/download/";

#if UNITY_IOS && !UNITY_EDITOR
        // temporaryCachePath/downloadをpersistentDataPath/に移動する(ios専用)
        var tempPath = Application.temporaryCachePath + "/download/";
        if(Directory.Exists(tempPath)) {
            Directory.Delete(localFilePath, true);
        }
#endif

		// フォルダが存在したら削除
		if(Directory.Exists(localFilePath)) {
			DirectoryProcessor.Delete(localFilePath);
			Debug.Log("Masterを削除しました");
		}
		else {
			Debug.Log("フォルダがありません");
		}
	}
}
