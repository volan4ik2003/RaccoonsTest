using _Game.Scripts.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    [SerializeField] private WinPanel _winPanel;

    public WinPanel WinPanel => _winPanel;

    public void Init()
    {
        _winPanel.Close();
    }
}