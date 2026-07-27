//
//クサカレールのうら

using System.Runtime.CompilerServices;
using UnityEngine;

public class karehamanager : MonoBehaviour
{
    [SerializeField] private ParticleSystem sprayParticles;
    [Header("噴射タイミング")]
    [SerializeField] private float delayBeforeSpray = 1.0f;

    [Header("消滅の設定")]
    [SerializeField] private float minDuration = 5.0f;
    [SerializeField] private float maxDuration = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(StartSpraying), delayBeforeSpray);
        
        float activeTime = Random.Range(minDuration, maxDuration);
        Invoke(nameof(SelfDestroy), activeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartSpraying()
    {
        if (sprayParticles != null && !sprayParticles.isPlaying)
        {
            sprayParticles.Play();
        }
    }

    public void StopSpraying()
    {
        if (sprayParticles != null && sprayParticles.isPlaying)
        {
            sprayParticles.Stop();
        }
    }
    private void SelfDestroy()
    {
        StopSpraying();
        // パーティクルが消え切るまで1秒待ってからオブジェクトを削除
        Destroy(gameObject, 1.0f);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage(40);
            }
        }
    }

}
