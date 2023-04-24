using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeGameDataManager : MonoBehaviour
{

    private static int _dataStamp = 0;
    public static int DataStamp
    {
        get { return _dataStamp; }
    }
    private static int _coinCounter = 0;

    private void OnDisable()
    {
        //CharacterController2D cc2d = _player.GetComponent<CharacterController2D>();
        //cc2d.OnCollision -= OnCollisionCallback;
    }

    private void OnEnable()
    {
        //_player = GameObject.FindGameObjectWithTag("Player");
        //CharacterController2D cc2d = _player.GetComponent<CharacterController2D>();
        //cc2d.OnCollision += OnCollisionCallback;
    }

    private void Awake()
    {

    }

    static void UpdateDataStamp()
    {
        ++_dataStamp;
    }

    public static int GetCoinCounter()
    {
        return _coinCounter;
    }

    public static void AddCoinCounter(int c)
    {
        _coinCounter += c;
        UpdateDataStamp();
    }

    void OnCollisionCallback(Collider2D coll2d, ColliderDistance2D collDist2d)
    {
        if (coll2d.gameObject.CompareTag("Coin"))
        {
            Destroy(coll2d.gameObject);
            RuntimeGameDataManager.AddCoinCounter(1);
        }
    }
}
