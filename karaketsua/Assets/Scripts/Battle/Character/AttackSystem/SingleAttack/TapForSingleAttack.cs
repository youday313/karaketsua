using UnityEngine;
using System.Collections;

public class TapForSingleAttack : MonoBehaviour {

    //GameObject attackMakerPrefab;
    Vector3 popupPositionInScreen;
    float startTime;
    float leftTime;
    bool isTapDetect = false;
    GameObject nowMaker;

    SingleActionParameter singleActionParameter;
	// Use this for initialization
	void Start () {

        StartCoroutine("StartAction");
	}
    public void Init(SingleActionParameter actionParam,Vector3 popupPos)
    {
        singleActionParameter.judgeTime = actionParam.judgeTime;
        singleActionParameter.startInterval= actionParam.startInterval;
        singleActionParameter.attackEffectKind = actionParam.attackEffectKind;
        popupPositionInScreen=CSTransform.CopyVector3(popupPos);
    }

    public IEnumerator StartAction()
    {
        //マーカー出現までの待ち時間
        yield return new WaitForSeconds(singleActionParameter.startInterval);
        nowMaker = Instantiate(Resources.Load<GameObject>("AttackMaker"), popupPositionInScreen, Quaternion.identity) as GameObject;
        nowMaker.transform.SetParent(GameObject.FindGameObjectWithTag("EffectCanvas").transform);

        //マーカー縮小
        iTween.ScaleTo(nowMaker, iTween.Hash("scale", new Vector3(0.1f, 0.1f, 1.0f), "time", singleActionParameter.judgeTime));
        
        //タップ判定
        startTime = Time.time;
        //タップできなかったら最大時間
        leftTime = 0;
        IT_Gesture.onShortTapE += OnTapForAttack;
        yield return new WaitForSeconds(singleActionParameter.judgeTime);
        IT_Gesture.onShortTapE -= OnTapForAttack;
        isTapDetect = false;
        if (nowMaker != null)
        {
            Destroy(nowMaker);

        }
        yield return null;

    }
    //タイミングを合わせたタップ
    void OnTapForAttack(Vector2 pos)
    {
        if (isTapDetect == true) return;
        isTapDetect = true;
        //残り時間
        var nowTime = Time.time - startTime;
        leftTime = Mathf.Clamp(nowTime, 0, singleActionParameter.judgeTime);
        Destroy(nowMaker);
        nowMaker = null;
        var attackEffect = Instantiate(GetEffectPrefabFromResouce(), popupPositionInScreen, Quaternion.identity) as GameObject;
        attackEffect.transform.SetParent(GameObject.FindGameObjectWithTag("EffectCanvas").transform);
    }

    GameObject GetEffectPrefabFromResouce()
    {
        if(singleActionParameter.attackEffectKind==AttackEffectKind.sword){
        return Resources.Load<GameObject>("Effect/HitSwordEffect");
        }
        else if(singleActionParameter.attackEffectKind==AttackEffectKind.bullet){
            return Resources.Load<GameObject>("Effect/HitBulletEffect");
        }
        return null;
    }
}
