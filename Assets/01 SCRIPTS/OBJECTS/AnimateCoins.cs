using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class AnimateCoins : Singleton<AnimateCoins>
{
    [Space]
    [Header("Available coins : (coins to pool)")]
    Queue<GameObject> coinsQueue = new Queue<GameObject>();


    [Space]
    [Header("Animation settings")]
    [SerializeField] [Range(0.5f, 0.9f)] float minAnimDuration;
    [SerializeField] [Range(0.9f, 2f)] float maxAnimDuration;

    [SerializeField] Ease easeType;
    [SerializeField] float spread;


    private int _c = 0;

    public int Coins
    {
        get { return _c; }
        set
        {
            _c = value;
        }
    }
    public void PrepareCoins(GameObject animatedCoinPrefab, int maxCoins)
    {
        GameObject coin;
        for (int i = 0; i < maxCoins; i++)
        {
            coin = Instantiate(animatedCoinPrefab);
            coin.transform.parent = transform;
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
    }

    void Animate(Text CoinPositionUI, Vector3 targetCoinPosition, Vector3 collectedCoinPosition, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (coinsQueue.Count > 0)
            {
                GameObject coin = coinsQueue.Dequeue();
                coin.SetActive(true);
                coin.transform.position = collectedCoinPosition + new Vector3(Random.Range(-spread, spread), 0f, 0f);

                float duration = Random.Range(minAnimDuration, maxAnimDuration);
                coin.transform.DOMove(targetCoinPosition, duration)
                .SetEase(easeType)
                .OnComplete(() =>
                {
                    coin.SetActive(false);
                    coinsQueue.Enqueue(coin);
                    Coins++;
                    CoinPositionUI.text = Coins.ToString();
                });
            }
        }
    }

    public void AddCoins(Text CoinPositionUI, Vector3 targetCoinPosition, Vector3 collectedCoinPosition, int amount)
    {
        Animate(CoinPositionUI, targetCoinPosition, collectedCoinPosition, amount);
    }
}
