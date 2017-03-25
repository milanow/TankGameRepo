using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {
    public float PlayerMoveSpeed = 2f;
    public float BulletSpeed = 4f;
    public float FireInterval = 0.3f;
    public Vector3 Player1SpawnPos = new Vector3(-3.0f, 0, 0);
    public Vector3 Player2SpawnPos = new Vector3(3f, 0, 0);
    public string Player1Name = "Player1";
    public string Player2Name = "Player2";

    [HideInInspector] public int Player1Score = 0;
    [HideInInspector] public int Player2Score = 0;
    private Rect gameSceneBox = new Rect(-5.28f, -2.98f, 10.56f, 5.96f);
    private GameObject player;
    private List<GameObject> players;
    //private BulletPool bulletPool;

    private static WorldManager _instance = null;
    public static WorldManager Instance {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        player = Resources.Load("Prefab_Player") as GameObject;
    }

    private void Start()
    {
        SpawnPlayers();
        ResetGame();
    }

    private void ResetRound()
    {
        for(int i = 0; i < players.Count; ++i)
        {
            players[i].GetComponent<Player>().ResetPlayer();
        }
        foreach(GameObject b in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Destroy(b);
        }
    }

    public void ResetGame()
    {
        Score.Instance.UpdateScore(Player1Score = 0, Player2Score = 0);
        ResetRound();
    }

    private void SpawnPlayers()
    {
        players = new List<GameObject>();
        GameObject player1 = Instantiate(player) as GameObject;
        Controller controller1 = new Controller(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.Q);
        player1.GetComponent<Player>().Init(Player1SpawnPos, Vector3.zero, PlayerMoveSpeed, controller1);
        player1.name = Player1Name;
        players.Add(player1);

        GameObject player2 = Instantiate(player) as GameObject;
        Controller controller2 = new Controller(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.RightShift);
        player2.GetComponent<Player>().Init(Player2SpawnPos, Vector3.up*180, PlayerMoveSpeed, controller2);
        player2.name = Player2Name;
        players.Add(player2);
    }

    public void OnHitPlayer(GameObject hittedPlayer)
    {
        for(int i = 0; i < players.Count; ++i)
        {
            if (players[i].name == hittedPlayer.name)
            {
                if (i == 0) Player2Score++;
                else Player1Score++;
                Score.Instance.UpdateScore(Player1Score, Player2Score);
                ResetRound();
            }
        }

        
    }

    public bool InGameScene(Vector3 position)
    {
        return gameSceneBox.Contains(new Vector2(position.x, position.y));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
