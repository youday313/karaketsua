using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using BattleScene;

namespace BattleScene
{
    // 詳細ステータス表示用UI
    // 削除、生成せず使いまわす
    public class CharacterDetailStateUI: MonoBehaviour
    {
        // ステータス
        [SerializeField]
        private Image bodyImage;
        [SerializeField]
        private Text name;
        [SerializeField]
        private Text power;
        [SerializeField]
        private Text deffence;
        [SerializeField]
        private Text activity;
        [SerializeField]
        private Text skillPoint;
        [SerializeField]
        private Text hp;
        [SerializeField]
        private Text intellisense;

        // 機能
        [SerializeField]
        private Button close;

        // 表示
        public void Show(CharacterMasterParameter param)
        {
            bodyImage.sprite = Resources.Load<Sprite>(ResourcesPath.CharacterBodyImage + param.charaName);
            name.text = param.charaName;
            power.text = param.power.ToString();
            deffence.text = param.deffence.ToString();
            activity.text = param.activeSpeed.ToString();
            skillPoint.text = param.skillPoint.ToString();
            hp.text = param.hp.ToString();
            //intellisense = param.

            close.onClick.AddListener(hide);

            gameObject.SetActive(true);
        }

        private void hide()
        {
            gameObject.SetActive(false);
        }

    }
}