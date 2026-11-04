using UnityEngine;
using TMPro;
using System;

public class TimerManagement : MonoBehaviour
{
    
    private bool _timerActive;
    private float _currentTime;
    [SerializeField] private float _startMinutes;
    [SerializeField] private TMP_Text _timerText;

    void Start()
    {
        _timerActive = true;
        _currentTime = _startMinutes * 60f;
    }

    void Update()
    {
        if (_timerActive)
        {
            _currentTime -= Time.deltaTime;

            if (_currentTime <= 0)
            {
                _currentTime = 0;
                _timerActive = false;
            }
        }

        TimeSpan time = TimeSpan.FromSeconds(_currentTime);

        _timerText.text = time.ToString(@"mm\:ss\:fff");
    }
}