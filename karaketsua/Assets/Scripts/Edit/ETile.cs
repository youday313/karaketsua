using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using EditScene;

namespace EditScene
{

    public class ETile : MonoBehaviour
    {

        public IntVect2D vect;
        public bool isAttachable = false;
        ETileCreater tileCreater;
        // Use this for initialization
        void Start()
        {


        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(IntVect2D v, bool _isAttachable, ETileCreater _tileCreater)
        {
            vect = v;
            tileCreater = _tileCreater;
            isAttachable = _isAttachable;
            if (isAttachable == true)
            {
                var sprite = GetComponent<Image>();
                sprite.sprite = Resources.Load<Sprite>("EditScene/mass_attack_sprite");
            }
        }
        //public void OnDrop(PointerEventData e){
        //    if (isAttachable == false)
        //        return;
        //    ECharacterIcon.obj.position = transform.position;
        //    tileCreater.gameObject.SetActive (false);

        //}
    }
}