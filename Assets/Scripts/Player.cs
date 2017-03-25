using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    float _moveSpeed;
    float _fireInterval;
    Controller _controller;
    Vector2 _move;
    Vector3 _spawnPos;
    Vector3 _spawnDir;
    bool _canMove;
    float _collisionDetectRange;
    GameObject bullet;

	// Use this for initialization
	public void Init (Vector3 spawnPos, Vector3 towardsDirection, float moveSpeed, Controller controller) {
        _spawnPos = spawnPos;
        _spawnDir = towardsDirection;
        _moveSpeed = moveSpeed;
        _fireInterval = WorldManager.Instance.FireInterval;
        _controller = controller;
        _canMove = true;
        _collisionDetectRange = this.transform.localScale.x / 2 + 0.1f;
        bullet = Resources.Load("Prefab_Bullet") as GameObject;
        ResetPlayer();
        RunFireUpdate();
        
    }
	
	// Update is called once per frame
	void Update () {
        _move = _controller.Move();
        // Update rotation
        if (_move == Vector2.up)
        {
            this.transform.eulerAngles = new Vector3(0, -90f, 0);
        }
        else if (_move == Vector2.down)
        {
            this.transform.eulerAngles = new Vector3(0, 90f, 0);
        }
        else if (_move == Vector2.left)
        {
            this.transform.eulerAngles = new Vector3(0, -180f, 0);
        }
        else if (_move == Vector2.right)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        DrawRayHit();

        // Update position
        if (_canMove)
        {
            this.transform.position = new Vector3(this.transform.position.x + _move.x * _moveSpeed * Time.deltaTime, this.transform.position.y, this.transform.position.z + _move.y * _moveSpeed * Time.deltaTime);
        }
        
    }
    
    private void DrawRayHit()
    {
        RaycastHit hitInfo;
        Vector3 endPoint = new Vector3(this.transform.position.x + _move.x*_collisionDetectRange,
            this.transform.position.y, this.transform.position.z + _move.y*_collisionDetectRange);
        Debug.DrawLine(this.transform.position, endPoint, Color.green);
        if (Physics.Raycast(this.transform.position, this.transform.right, out hitInfo, _collisionDetectRange)) {
            //Debug.Log("Detect: " + hitInfo.collider.name);
            if (hitInfo.collider.tag == "Wall" || hitInfo.collider.tag == "Player")
            {
                _canMove = false;
            }
        }
        else
        {
            _canMove = true;
        }
    }

    private void RunFireUpdate()
    {
        StartCoroutine("fireCoroutine");
    }

    IEnumerator fireCoroutine()
    {
        while (true)
        {
            if (_controller.Fire())
            {
                GameObject go = Instantiate(bullet);
                go.GetComponent<Bullet>().Init(new Vector3(this.transform.position.x + _move.x*_collisionDetectRange,
                    this.transform.position.y, this.transform.position.z + _move.y*_collisionDetectRange), this.transform.right);
            }
            yield return new WaitForSeconds(_fireInterval);

        }
    }

    public void ResetPlayer()
    {
        this.transform.position = _spawnPos;
        this.transform.eulerAngles = _spawnDir;
    }
}
