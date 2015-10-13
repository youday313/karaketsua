using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class CharacterManager : Singleton<CharacterManager> {


    [System.NonSerialized]public List<Character> characters=new List<Character>();
    
	// Use this for initialization
	void Start () {


        foreach(var chara in StageData.Instance.playerCharacters)
        {
            var cha = Instantiate(chara.prefab) as Character;
            cha.Init(new IntVect2D((int)chara.position.x, (int)chara.position.y),false);
            characters.Add(cha);
            cha.transform.SetParent(transform);
        }
        foreach(var chara in StageData.Instance.enemyCharacters)
        {
            var cha = Instantiate(chara.prefab) as Character;
            //向き変更
            cha.Init(new IntVect2D((int)chara.position.x, (int)chara.position.y),true);
            characters.Add(cha);
            cha.transform.SetParent(transform);
        }

	}
	
    public void DestroyCharacter(Character chara)
    {
        characters.Remove(chara);
        Destroy(chara.gameObject);
        var count = characters.Where(t => t.isEnemy == true).Count();
        Debug.Log(count);
        if (count == 0)
        {
            SceneManager.Instance.SetNextScene("ResultScene");
        }
    }


	// Update is called once per frame
	void Update () {
	
	}
}
