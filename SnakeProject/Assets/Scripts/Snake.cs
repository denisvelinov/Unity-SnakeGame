using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int direction;
    private Vector2Int position;
    private Queue<Vector2Int> newDirection;
    private float moveTimerMax;
    private float moveTimer;
    public Transform bodyPrefab;
    private List<Transform> body = new List<Transform>();
    private int runScore;
    private int highscore;
    private bool canMove = true;
    public GameOver gameOver;
    public InGameUI ingameUI;

    private void Start()
    {
        ResetState();
        highscore = PlayerPrefs.GetInt("Highscore", 0);
        ingameUI.ScoreSetup(runScore, highscore);
    }

    void Update()
    {
        if (newDirection.Count == 0)
        {
            HandleInput(); 
        }
        if (canMove)
        {
            HandleMovement(); 
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (direction.y != -1)
            {
                direction.x = 0;
                direction.y = 1;
                newDirection.Enqueue(direction);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (direction.y != 1)
            {
                direction.x = 0;
                direction.y = -1;
                newDirection.Enqueue(direction);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (direction.x != -1)
            {
                direction.x = 1;
                direction.y = 0;
                newDirection.Enqueue(direction);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (direction.x != 1)
            {
                direction.x = -1;
                direction.y = 0;
                newDirection.Enqueue(direction);
            }
        }
    }

    private void HandleMovement()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer >= moveTimerMax)
        {
            position += direction;
            moveTimer -= moveTimerMax;

            for (int i = body.Count - 1; i > 0; i--)
            {
                body[i].position = body[i - 1].position;
            }

            transform.position = new Vector3(position.x, position.y);
            transform.eulerAngles = new Vector3(0, 0, HandleRotation(direction));
            newDirection.Clear();
        }

    }

    private float HandleRotation(Vector2Int direction)
    {
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }

    private void Grow()
    {
        Transform bodySegment = Instantiate(this.bodyPrefab);
        body.Add(bodySegment);
        bodySegment.position = body[body.Count - 1].position;
    }
    private void ResetState()
    {
        for (int i = 1; i < body.Count; i++)
        {
            Destroy(body[i].gameObject);
        }

        body.Clear();

        position = new Vector2Int(0, 0);
        direction = new Vector2Int(1, 0);
        moveTimerMax = 0.3f;
        moveTimer = moveTimerMax;
        newDirection = new Queue<Vector2Int>();

        body.Add(this.transform);
        canMove = true;
    }

    private void ChangeMoveSpeed()
    {
        if (moveTimerMax > 0.19)
        {
            moveTimerMax -= 0.01f;
        }
        else if (moveTimerMax > 0.019)
        {
            moveTimerMax -= 0.005f;
        }
        
        HandleScore();
    }

    private void HandleScore()
    {
        runScore++;

        if (runScore > highscore)
        {
            PlayerPrefs.SetInt("Highscore", runScore);
            highscore = runScore;
        }

        ingameUI.ScoreSetup(runScore, highscore);
    }

    private void GameOver()
    {
        ingameUI.HideUI();
        gameOver.Setup(runScore, highscore);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            ChangeMoveSpeed();
            Grow();
        }
        else if (collision.tag == "Obstacle")
        {
            canMove = false;
            GameOver();
        }
    }

}
