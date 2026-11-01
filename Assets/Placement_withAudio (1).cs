using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class Placement : MonoBehaviour
{
    public TrackableType trackableTypes = TrackableType.AllTypes;
    
    [SerializeField]
    private GameObject m_objectPrefab = null;
    
    private ARRaycastManager m_raycastManager;
    private List<ARRaycastHit> m_hitResults = new List<ARRaycastHit>();

    public AudioClip placementSound; // 配置時のサウンド
    private AudioSource audioSource; // 音を再生するためのAudioSource

    private float cooldownTime = 10f; // クールダウン時間（60秒）
    private float lastPlacementTime = 0f; // 最後にオブジェクトを配置した時刻[
    private GameObject m_spawnedObject = null;

    private void Start()
    {
        // AR Raycast Managerの取得
        m_raycastManager = GetComponent<ARRaycastManager>();

        // AudioSource の追加
        audioSource = gameObject.AddComponent<AudioSource>();

        // placementSoundが設定されていればセット
        if (placementSound != null)
        {
            audioSource.clip = placementSound;
        }
        else
        {
            Debug.LogWarning("placementSound が設定されていません。サウンドは再生されません。");
        }
    }

    private void Update()
    {
        // 画面のタッチを監視
        if (Input.touchCount > 0 && m_spawnedObject == null)
        {
            // if (Time.time - lastPlacementTime < cooldownTime)
            // {
            //     // クールダウン時間内なら配置しない
            //     return;
            // }

            // タッチ位置からレイキャスト
            if (m_raycastManager.Raycast(Input.GetTouch(0).position, m_hitResults, trackableTypes))
            {
                m_spawnedObject = Instantiate(m_objectPrefab, m_hitResults[0].pose.position, m_hitResults[0].pose.rotation * Quaternion.Euler(0,180,0));
                
                // ヒットした位置にオブジェクトを配置

                // サウンドを再生（placementSoundがある場合のみ）
                if (placementSound != null)
                {
                    audioSource.Play();
                }

                // 最後にオブジェクトを配置した時刻を更新
                lastPlacementTime = Time.time;
            }
        }
    }
}