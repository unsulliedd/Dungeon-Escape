using UnityEngine;

public class Diamond : MonoBehaviour
{
    public int diamondValue = 1;
    private Player _player;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out _player);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_player != null && other.CompareTag("Player"))
        {
            _player.AddDiamonds(diamondValue);
            Destroy(gameObject);
        }
    }
}
