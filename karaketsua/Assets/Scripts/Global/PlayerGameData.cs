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
    public Dictionary<int, IntVect2D> BattlePlayerPosition;
    [SerializeField]
    private List<IntVect2D> editorPlayerPositions = new List<IntVect2D>();

    public List<int> SelectEnemyCharacterIds = new List<int>();
    public Dictionary<int, IntVect2D> BattleEnemyPosition;
    [SerializeField]
    private List<IntVect2D> editorEnemyPositions = new List<IntVect2D>();

    protected override void create()
    {
        if(isPositionFormEditor) {
            Debug.Log("FormEditor");
            BattlePlayerPosition = new Dictionary<int, IntVect2D>();
            BattleEnemyPosition = new Dictionary<int, IntVect2D>();
            for(var i = 0; i < SelectPlayerChatacterIds.Count; i++) {
                BattlePlayerPosition.Add(SelectPlayerChatacterIds[i], editorPlayerPositions[i]);
            }
            for(var i = 0; i < SelectEnemyCharacterIds.Count; i++) {
                BattleEnemyPosition.Add(SelectEnemyCharacterIds[i], editorEnemyPositions[i]);
            }
        }
    }

    public void ResetPositionList()
    {
        BattlePlayerPosition.Clear();
        BattleEnemyPosition.Clear();
    }

}
