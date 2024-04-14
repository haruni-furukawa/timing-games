using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameManager : MonoBehaviour
{
    private enum TimingState
    {
        Perfect,
        Great,
        Good,
        Miss
    }

    private const double SCREEN_WIDTH = 1920;
    private const double SCREEN_HEIGHT = 1080;
    private const double NOTE_MOVE_SPACE_RATE = 0.6;
    private double NOTE_MOVE_SPACE => SCREEN_WIDTH * NOTE_MOVE_SPACE_RATE;
    [SerializeField] private RectTransform note = default;
    [SerializeField] private RectTransform target = default;
    [SerializeField] private TextMeshProUGUI scoreValueText = default;
    [SerializeField] private float noteSpeed = 5.0f;

    private double _elapsedTime = 0;

    private bool _isRight = true;

    private int _currentScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60; // 60 FPS
        Debug.Log("NOTE_MOVE_SPACE = " + NOTE_MOVE_SPACE);

        UpdateScoreValueText();
    }

    // Update is called once per frame
    void Update()
    {
        MoveNoteLogic();

        int addScore = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float centerOffset = Vector2.Distance(target.anchoredPosition, note.anchoredPosition);

            switch (GetTimingState(centerOffset))
            {
                case TimingState.Perfect:
                    addScore = 100;
                    break;
                case TimingState.Great:
                    addScore = 50;
                    break;
                case TimingState.Good:
                    addScore = 10;
                    break;
                case TimingState.Miss:
                default:
                    break;
            }
        }

        if (addScore > 0)
        {
            _currentScore += addScore;
            UpdateScoreValueText();
        }
    }

    private void MoveNoteLogic()
    {
        Vector2 nextAnchoredPosition = note.anchoredPosition;
        float speed = noteSpeed * Time.deltaTime;

        if (_isRight)
        {
            nextAnchoredPosition.x += speed;
        }
        else
        {
            nextAnchoredPosition.x -= speed;
        }

        note.anchoredPosition = nextAnchoredPosition;

        float nowXPos = note.anchoredPosition.x;
        if (nowXPos >= NOTE_MOVE_SPACE / 2)
        {
            _isRight = false;
        }
        else if (nowXPos <= -NOTE_MOVE_SPACE / 2)
        {
            _isRight = true;
        }
    }

    private TimingState GetTimingState(float centerOffset)
    {
        if (centerOffset >= 200)
        {
            return TimingState.Miss;
        }

        if (centerOffset >= 100)
        {
            return TimingState.Good;
        }

        if (centerOffset >= 50)
        {
            return TimingState.Great;
        }

        return TimingState.Perfect;
    }

    private void UpdateScoreValueText()
    {
        if (scoreValueText == null)
        {
            return;
        }

        scoreValueText.text = $"{_currentScore:D6}";
    }
}
