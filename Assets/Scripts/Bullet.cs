using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private float _speed;
    private Vector3 _moveDir;
    private float _collisionDetectRange;
    bool _move;

    public void Init(Vector3 position, Vector3 direction)
    {
        this.transform.position = position;
        _moveDir = direction;
        _speed = WorldManager.Instance.BulletSpeed;
        _collisionDetectRange = this.transform.localScale.x;
    }

    // Update is called once per frame
    void Update() { 
        this.transform.position = new Vector3(this.transform.position.x + _moveDir.x * _speed * Time.deltaTime,
                this.transform.position.y + _moveDir.y * _speed * Time.deltaTime,
                this.transform.position.z + _moveDir.z * _speed * Time.deltaTime);
        if (!WorldManager.Instance.InGameScene(this.transform.position))
        {
            Destroy(this.gameObject);
        }
	}

    // Collision needs to be detect more frequently than others
    private void FixedUpdate()
    {
        DrawRayHit();
    }

    void DrawRayHit()
    {
        RaycastHit hitInfo;
        Vector3 endPoint = new Vector3(this.transform.position.x + _moveDir.x * _collisionDetectRange,
            this.transform.position.y, this.transform.position.z + _moveDir.z * _collisionDetectRange);
        Debug.DrawLine(this.transform.position, endPoint, Color.red);
        if (Physics.Raycast(this.transform.position, _moveDir, out hitInfo, _collisionDetectRange))
        {
            if (hitInfo.collider.tag == "Wall")
            {
                Destroy(this.gameObject);
            }
            else if(hitInfo.collider.tag == "Player")
            {
                WorldManager.Instance.OnHitPlayer(hitInfo.collider.gameObject);
                Destroy(this.gameObject);
            }
        }
    }


}
