using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

using BattleScene;

namespace BattleScene
{
    // 体力、気力の表示用UI
    public class BCharacterStateUI: MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        // 表示までの長押し時間
        private const float WatiTimeForShow = 1f;

        // 長押しで実行
        private UnityEvent onHold = new UnityEvent();

        [SerializeField]
        private Image faceImage;
        [SerializeField]
        private Slider hpbar;
        [SerializeField]
        private Slider skillBar;
        [SerializeField]
        private CharacterDetailStateUI detailState;

        private bool isHold;

        public void Initialize(BCharacterBase character)
        {
            // イベント登録
            character.OnDeathE += delete;
            character.OnStatusUpdateE += updateUi;

            onHold.AddListener(() => {
                detailState.Show(character.characterParameter);
            });

            faceImage.sprite = Resources.Load<Sprite>(ResourcesPath.CharacterStatusIcon + character.characterParameter.charaName);
            hpbar.maxValue = character.characterParameter.hp;
            skillBar.maxValue = character.characterParameter.skillPoint;
            updateUi(character);
        }

        private void delete(BCharacterBase chara)
        {
            Destroy(this.gameObject);
        }

        private void updateUi(BCharacterBase character)
        {
            hpbar.value = character.characterParameter.hp;
            skillBar.value = character.characterParameter.skillPoint;
        }
            
        // 押し始め
        // Event
        public void OnPointerDown(PointerEventData eventData)
        {
            isHold = true;
            StopAllCoroutines();
            StartCoroutine(wait());
        }
            
        // ロング判定
        private IEnumerator wait()
        {
            yield return new WaitForSeconds(WatiTimeForShow);

            // 長押ししていたら実行
            if(isHold) {
                isHold = false;
                onHold.Invoke();
            }
        }

        // 離した
        // Event
        public void OnPointerUp(PointerEventData eventData)
        {
            isHold = false;
        }
    }
}