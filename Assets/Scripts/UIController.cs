using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    int _gameDataStamp = 0;
    private Text _coinText;

    private void Start()
    {
        Transform coinText = transform.Find("CoinText");
        if (coinText != null)
            _coinText = coinText.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int stamp = RuntimeGameDataManager.DataStamp;
        if (_gameDataStamp != stamp)
        {
            _gameDataStamp = stamp;
            // UI updating sample, _20191210_jintaeks
            if (_coinText != null)
            {
                _coinText.text = RuntimeGameDataManager.GetCoinCounter().ToString();
            }
        }
    }
}
