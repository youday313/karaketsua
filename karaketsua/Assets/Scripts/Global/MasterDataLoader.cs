using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using BattleScene;
using System.IO;

public class MasterDataLoader: DontDestroySingleton<MasterDataLoader>
{


    private string localFilePath;

    private List<CharacterMasterParameter> playerCharacterCache;
    private List<CharacterMasterParameter> enemyCharacterCache;

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
    /// 全プレイヤーキャラを取得する
    /// </summary>
    public List<CharacterMasterParameter> LoadPlayerCharacters()
    {
        if(playerCharacterCache == null) {
            var filePath = localFilePath + "m_player_character";
            JsonManager.Instance.Load<List<CharacterMasterParameter>>(filePath, obj => {
                playerCharacterCache = obj;
            });
        }
        return playerCharacterCache;
    }

    /// <summary>
    /// プレイヤーキャラを保存する
    /// </summary>
    public void SavePlayerCharacter(List<CharacterMasterParameter> datas)
    {
        var filePath = localFilePath + "m_player_character";
        JsonManager.Instance.Save(filePath, datas);
    }

    /// <summary>
    /// 全敵キャラデータを取得する
    /// </summary>
    public List<CharacterMasterParameter> LoadEnemyCharacters()
    {
        if(enemyCharacterCache == null) {
            var filePath = localFilePath + "m_enemy_character";
            JsonManager.Instance.Load<List<CharacterMasterParameter>>(filePath, obj => {
                enemyCharacterCache = obj;
            });
        }
        return enemyCharacterCache;
    }


}
