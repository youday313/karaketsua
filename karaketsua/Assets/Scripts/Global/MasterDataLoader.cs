using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using BattleScene;
using System.IO;

public class MasterDataLoader: DontDestroySingleton<MasterDataLoader>
{

    private string localFilePath;

    [SerializeField]
    public class MasterParameterList
    {
        public List<CharacterMasterParameter> lists;
    }
    private List<CharacterMasterParameter> playerCharacterCache;
    private List<CharacterMasterParameter> enemyCharacterCache;

    protected override void create()
    {
        localFilePath = Application.persistentDataPath + "/download/";

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
        if(Directory.Exists(localFilePath)) {
            return;
        }
        Debug.Log("Masterを作成します");
        Directory.CreateDirectory(localFilePath);
        CopyMasterDataFromStreamingAssets();
    }


    /// <summary>
    /// SteramingAssetsのファイルを全てコピーする
    /// </summary>
    public void CopyMasterDataFromStreamingAssets()
    {
        var streamingPath = Application.streamingAssetsPath;
        DirectoryProcessor.CopyAndReplace(Application.streamingAssetsPath, localFilePath);
        Debug.Log("作成完了");
    }

    /// <summary>
    /// 全プレイヤーキャラを取得する
    /// </summary>
    public List<CharacterMasterParameter> LoadPlayerCharacters()
    {
        if(playerCharacterCache == null) {
            var filePath = localFilePath + "m_player_character";
            JsonManager.Instance.Load<MasterParameterList>(filePath, obj => {
                playerCharacterCache = obj.lists;
            });
        }
        return playerCharacterCache;
    }

    /// <summary>
    /// プレイヤーキャラを保存する
    /// </summary>
    public void SavePlayerCharacter(List<CharacterMasterParameter> datas)
    {
        Debug.Log("Save");
        var filePath = localFilePath + "m_player_character";
        var data = new MasterParameterList();
        data.lists = datas;
        JsonManager.Instance.Save(filePath, data);
    }

    /// <summary>
    /// 全敵キャラデータを取得する
    /// </summary>
    public List<CharacterMasterParameter> LoadEnemyCharacters()
    {
        if(enemyCharacterCache == null) {
            var filePath = localFilePath + "m_enemy_character";
            JsonManager.Instance.Load<MasterParameterList>(filePath, obj => {
                enemyCharacterCache = obj.lists;
            });
        }
        return enemyCharacterCache;
    }


}
