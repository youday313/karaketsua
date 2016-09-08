using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerGameData : DontDestroySingleton<PlayerGameData> {


    public List<int> selectPlayerChatacterIds = new List<int>();
    public Dictionary<int, IntVect2D> battlePlayerPosition = new Dictionary<int, IntVect2D>();

    public List<int> selectEnemyCharacterIds = new List<int>();
    public Dictionary<int, IntVect2D> battleEnemyPosition = new Dictionary<int, IntVect2D>();

    void Awake()
    {
        battlePlayerPosition.Add(1, new IntVect2D(0,0));
        battleEnemyPosition.Add(1, new IntVect2D(0, -2));

    }

}
