using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GamePlay
{
    public class CheckPoint : MonoBehaviour
    {
        [SerializeField] private GameObject spawnEffect;


        public async void StartSpawnEffect(Action start = null,Action finish = null)
        {
            start?.Invoke();
            spawnEffect.SetActive(true);
            await UniTask.WaitUntil(() => !spawnEffect.activeSelf);
            finish?.Invoke();
            
        }
    }
}