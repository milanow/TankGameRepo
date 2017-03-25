using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    [SerializeField]
    private Text score;

    private static Score _instance;
    public static Score Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void UpdateScore(int player1score, int player2score)
    {
        score.text = player1score + "  :  " + player2score;
    }
}
