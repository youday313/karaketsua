//DestroyedAfterParticleSystem
//作成日
//<summary>
//パーティクルシステムが終了したら削除
//親パーティクルの終了で削除に注意
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
[RequireComponent(typeof(ParticleSystem))]


public class DestroyedAfterParticleSystem : MonoBehaviour
{
    void Awake()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        Destroy(this.gameObject, particleSystem.duration);
    }
}