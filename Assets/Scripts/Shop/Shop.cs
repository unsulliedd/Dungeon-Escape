using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject _shopPanel;
    private int _currentItemelectedCost;
    private Player _player;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out _player);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.UpdatePlayerDiamondCount(_player.Diamonds);
            _shopPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _shopPanel.SetActive(false);
        }
    }

    public void SelectItem(int item)
    {
        switch (item)
        {
            case 0:
                UIManager.Instance.UpdateShopSelection(60);
                _currentItemelectedCost = 10;
                break;
            case 1:
                UIManager.Instance.UpdateShopSelection(-40);
                _currentItemelectedCost = 10;
                break;
            case 2:
                UIManager.Instance.UpdateShopSelection(-144);
                _currentItemelectedCost = 20;
                break;
            default:
                Debug.Log("Invalid Item");
                break;
        }
    }

    public void BuyItem()
    {
        if (_player.Diamonds >= _currentItemelectedCost)
        {
            _player.Diamonds -= _currentItemelectedCost;
            UIManager.Instance.UpdatePlayerDiamondCount(_player.Diamonds);
        }
    }

    public void ClosePanel()
    {
        _shopPanel.SetActive(false);
    }
}
