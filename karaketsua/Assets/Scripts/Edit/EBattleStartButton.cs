using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using EditScene;

namespace EditScene
{

    public class EBattleStartButton : MonoBehaviour
    {
        List<ECharacterIcon> characters=new List<ECharacterIcon>();
        Button button;
        // Use this for initialization
        void Start()
        {
            characters = GameObject.FindGameObjectsWithTag("Edit/Character").Select(x => x.GetComponent<ECharacterIcon>()).ToList();
            button = GetComponent<Button>();
            button.interactable = false;
        }

        // Update is called once per frame
        void Update()
        {
            //ボタン有効無効
            button.interactable = characters.Where(x => IntVect2D.IsNull(x.vect2D)==true).Count() == 0;

        }

        public void SetNextScene()
        {
            //BattleScene.BStageData.Instance.SetCharacterData();
            SceneManager.Instance.SetNextScene("MainScene");
            
        }


    }
}