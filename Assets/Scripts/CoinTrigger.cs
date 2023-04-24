using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        CharacterController2D cc2d = _player.GetComponent<CharacterController2D>();
        cc2d.OnCollision += OnCollisionCallback;
    }

    private void OnCollisionCallback(Collider2D coll2d, ColliderDistance2D collDist2d)
    {
        if (coll2d.gameObject.CompareTag("Coin"))
        {
            Destroy(coll2d.gameObject);
            RuntimeGameDataManager.AddCoinCounter(1);
        }
    }
}
