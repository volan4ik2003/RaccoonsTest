using _Game.Scripts.TileScripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.01f;
    [SerializeField] private float maxX = 3f;
    [SerializeField] private float shootForce = 15f;
    
    [SerializeField] private float spawnDelay = 0.5f; 

    private TileSpawner _spawner;
    private TileCube _currentTile;
    private Rigidbody _currentRb;

    private Vector3 _spawnPosition;
    private bool _isHolding;

    private void Start()
    {
        _spawner = GetComponent<TileSpawner>();
        SpawnNext();
    }

    private void Update()
    {
        HandleInput();
    }

    private void SpawnNext()
    {
        _currentTile = _spawner.SpawnTile();
        _currentRb = _currentTile.GetComponent<Rigidbody>();
        _spawnPosition = _currentTile.transform.position;
    }

    private void HandleInput()
    {
        var mouse = Mouse.current;
        
        if (mouse == null || _currentTile == null) return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            _isHolding = true;
            _currentRb.isKinematic = true;
        }

        if (mouse.leftButton.isPressed && _isHolding)
        {
            MoveTile(mouse);
        }

        if (mouse.leftButton.wasReleasedThisFrame && _isHolding)
        {
            _isHolding = false;
            ShootTile();
        }
    }

    private void MoveTile(Mouse mouse)
    {
        float deltaX = mouse.delta.ReadValue().x * moveSpeed;

        Vector3 pos = _currentTile.transform.position;
        pos.x += deltaX;

        pos.x = Mathf.Clamp(pos.x, _spawnPosition.x - maxX, _spawnPosition.x + maxX);

        _currentTile.transform.position = pos;
    }

    private void ShootTile()
    {
        _currentRb.isKinematic = false;
        _currentRb.AddForce(Vector3.forward * shootForce, ForceMode.Impulse);

        _currentTile = null;
        _currentRb = null;

        Invoke(nameof(SpawnNext), spawnDelay); 
    }
}