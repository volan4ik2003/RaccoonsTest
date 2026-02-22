using _Game.Scripts.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class HUD : MonoBehaviour
{
    [SerializeField] private WinPanel _winPanel;

    public WinPanel WinPanel => _winPanel;

    [Inject]
    public void Init()
    {
        _winPanel.Close();
    }
}