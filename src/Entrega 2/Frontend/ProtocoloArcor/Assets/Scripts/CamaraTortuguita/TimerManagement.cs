using UnityEngine;
using TMPro;
using System;

public class TimerManagement : MonoBehaviour
{
    private bool _timerActive;
    private float _currentTime;
    private float _initialTime;

    [SerializeField] private float _startMinutes;
    [SerializeField] private TMP_Text _timerText;

    void Start()
    {
        _timerActive = true;

        _currentTime = _startMinutes * 60f;

        // Guarda o tempo inicial da fase
        _initialTime = _currentTime;
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

    // Retorna o tempo que o jogador levou
    public float GetElapsedTime()
    {
        return _initialTime - _currentTime;
    }

    // Para o cronômetro
    public void StopTimer()
    {
        _timerActive = false;
    }
}
