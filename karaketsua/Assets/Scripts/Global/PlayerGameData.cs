using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// シーン間で保存するバトルとユニット情報を保存する
/// </summary>
public class PlayerGameData : DontDestroySingleton<PlayerGameData> {

    [SerializeField] private bool isPositionFormEditor;

    public List<int> SelectPlayerChatacterIds = new List<int>();
    public Dictionary<int, IntVect2D> BattlePlayerPosition = new Dictionary<int, IntVect2D>();

    public List<int> SelectEnemyCharacterIds = new List<int>();
    public Dictionary<int, IntVect2D> BattleEnemyPosition = new Dictionary<int, IntVect2D>();

    void Awake()
    {
        foreach(var id in SelectPlayerChatacterIds) {
            Debug.Log(id);
        }

        if(isPositionFormEditor) {
            Debug.Log("FormEditor");
            BattlePlayerPosition.Add(1, new IntVect2D(0, -2));
            BattleEnemyPosition.Add(1, new IntVect2D(0, 2));
        }
    }

    public void ResetPositionList()
    {
        BattlePlayerPosition.Clear();
        BattleEnemyPosition.Clear();
    }

}
