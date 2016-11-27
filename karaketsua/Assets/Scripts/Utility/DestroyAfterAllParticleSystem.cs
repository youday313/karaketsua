using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// 子のパーティクルを含め全てのパーティクル再生後、削除
public class DestroyAfterAllParticleSystem : MonoBehaviour {

    void Awake()
    {
        var particles = GetComponentsInChildren<ParticleSystem>();
        var maxTime = particles.Max(p => p.startDelay + p.duration);
        Destroy(gameObject, maxTime);
    }
}
