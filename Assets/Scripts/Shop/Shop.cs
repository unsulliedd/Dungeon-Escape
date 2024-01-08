using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject _shopPanel;
    private int _currentSelectedItem;
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
                AudioManager.Instance.PlayClickSound();
                _currentSelectedItem = 0;
                _currentItemelectedCost = 10;
                break;
            case 1:
                UIManager.Instance.UpdateShopSelection(-40);
                AudioManager.Instance.PlayClickSound();
                _currentSelectedItem = 1;
                _currentItemelectedCost = 10;
                break;
            case 2:
                UIManager.Instance.UpdateShopSelection(-144);
                AudioManager.Instance.PlayClickSound();
                _currentSelectedItem = 2;
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
            switch(_currentSelectedItem)
            {
                case 0:
                    Debug.Log("Purchased Item_1");
                    UIManager.Instance.UpdateOwnedItem(0);
                    break;
                case 1:
                    Debug.Log("Purchased Item_1");
                    UIManager.Instance.UpdateOwnedItem(1);
                    break;
                case 2:
                    GameManager.Instance.HasKeyToCastle = true;
                    UIManager.Instance.UpdateOwnedItem(2);
                    break;
                default:
                    Debug.Log("Invalid Item");
                    break;
            }
            AudioManager.Instance.PlayClickSound();
            _player.Diamonds -= _currentItemelectedCost;
            UIManager.Instance.UpdatePlayerDiamondCount(_player.Diamonds);
        }
    }

    public void ClosePanel()
    {
        AudioManager.Instance.PlayClickSound();
        _shopPanel.SetActive(false);
    }
}
