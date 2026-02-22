using _Game.Scripts.Infrastructure.Factories;
using _Game.Scripts.Infrastructure.States;
using UnityEngine;

public class WinGameState : IState
{
    private readonly UIFactory _uiFactory;

    public WinGameState(UIFactory uiFactory)
    {
        _uiFactory = uiFactory;
    }

    public void Enter()
    {
        _uiFactory.HUD.WinPanel.Open();

        Time.timeScale = 0f;
    }

    public void Exit()
    {
        _uiFactory.HUD.WinPanel.Close();
        Time.timeScale = 1f;
    }
}