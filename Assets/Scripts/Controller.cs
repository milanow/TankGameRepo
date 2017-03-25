using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller{
    KeyCode _moveUp;
    KeyCode _moveDown;
    KeyCode _moveLeft;
    KeyCode _moveRight;
    KeyCode _fire;
    Stack<KeyCode> inputStk;
    Dictionary<KeyCode, Vector2> directions;
    private Controller()
    {
    }

    public Controller(KeyCode moveUp, KeyCode moveDown, KeyCode moveLeft, KeyCode moveRight, KeyCode fire)
    {
        _moveUp = moveUp;
        _moveDown = moveDown;
        _moveLeft = moveLeft;
        _moveRight = moveRight;
        _fire = fire;
        inputStk = new Stack<KeyCode>();
        directions = new Dictionary<KeyCode, Vector2>()
        {
            {moveUp, Vector2.up },
            {moveDown, Vector2.down },
            {moveLeft, Vector2.left },
            {moveRight, Vector2.right },
        };
    }

    public Vector2 Move()
    {
        if (Input.GetKeyDown(_moveUp))
        {
            inputStk.Push(_moveUp);
        }
        else if (Input.GetKeyDown(_moveDown))
        {
            inputStk.Push(_moveDown);
        }
        else if (Input.GetKeyDown(_moveLeft))
        {
            inputStk.Push(_moveLeft);
        }
        else if (Input.GetKeyDown(_moveRight))
        {
            inputStk.Push(_moveRight);
        }

        // Using stack to ensure only one direction can be moved to
        while (inputStk.Count != 0)
        {
            KeyCode k = inputStk.Peek();
            if (Input.GetKey(k))
            {
                return directions[k];
            }else
            {
                inputStk.Pop();
            }
        }
        return Vector2.zero;
    }

    public bool Fire()
    {
        return Input.GetKey(_fire);
    }
}
