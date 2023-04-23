using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeGameDataManager : MonoBehaviour
{
    static private RuntimeGameDataManager instance;
    static public RuntimeGameDataManager Instance
    {
        get { return instance; }
    }
    private int _dataStamp = 0;
    public int DataStamp
    {
        get { return _dataStamp; }
    }
    private int _coinCounter = 0;
    GameObject _player;

    private void OnDisable()
    {
        CharacterController2D cc2d = _player.GetComponent<CharacterController2D>();
        cc2d.OnCollision -= OnCollisionCallback;
    }

    private void OnEnable()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        CharacterController2D cc2d = _player.GetComponent<CharacterController2D>();
        cc2d.OnCollision += OnCollisionCallback;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void UpdateDataStamp()
    {
        ++_dataStamp;
    }

    public int GetCoinCounter()
    {
        return _coinCounter;
    }

    void AddCoinCounter(int c)
    {
        _coinCounter += c;
        UpdateDataStamp();
    }

    void OnCollisionCallback(Collider2D coll2d, ColliderDistance2D collDist2d)
    {
        if (coll2d.gameObject.CompareTag("Coin"))
        {
            Destroy(coll2d.gameObject);
            AddCoinCounter(1);
        }
    }
}
