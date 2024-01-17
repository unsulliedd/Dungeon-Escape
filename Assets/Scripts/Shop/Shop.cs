using UnityEngine;

public class Shop : MonoBehaviour
{
    // Reference to the shop panel GameObject in the Unity Editor
    [SerializeField] private GameObject _shopPanel;

    // Variables to track the currently selected item in the shop and its cost
    private int _currentSelectedItem;
    private int _currentItemelectedCost;

    // Reference to the Player script attached to the player GameObject
    private Player _player;

    void Awake()
    {
        // Attempt to find the player GameObject and get the Player component
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out _player);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;    // Stop the game when entering the shop area
            AudioManager.Instance.TogglePause();    // Pause the game audio when entering the shop

            // Update UI with player's diamond count when entering the shop area
            UIManager.Instance.UpdatePlayerDiamondCount(_player.Diamonds);

            // Determine which items are owned and update the UI accordingly
            UpdateShopUIOwnedItem();

            // If no item is selected initially, choose a default item.
            if (_currentSelectedItem == 0)
                // First item selected by default
                SelectItem(0);

            // Activate the shop panel
            _shopPanel.SetActive(true);
        }
    }

    // Method to handle item selection in the shop
    public void SelectItem(int item)
    {
        AudioManager.Instance.PlayClickSound();

        // Update UI to highlight the selected item
        switch (item)
        {
            case 0:
                if (_player.Health < 4)  // Check if player's health is less than the maximum
                {
                    UIManager.Instance.UpdateShopSelection(60); // Update the selection indicator position
                    _currentSelectedItem = 0;       // Update the currently selected item
                    _currentItemelectedCost = 10;   // Update the currently selected item's cost
                }
                break;
            case 1:
                if (_player.Health < 4)
                {
                    UIManager.Instance.UpdateShopSelection(-40);
                    _currentSelectedItem = 1;
                    _currentItemelectedCost = 30;
                }
                break;
            case 2:
                UIManager.Instance.UpdateShopSelection(-144);
                _currentSelectedItem = 2;
                _currentItemelectedCost = 30;
                break;
            default:
                Debug.Log("Invalid Item");
                break;
        }
    }


    // Method to handle item purchase
    public void BuyItem()
    {
        if (_player.Diamonds >= _currentItemelectedCost)
        {
            // Process the purchase based on the selected item
            switch (_currentSelectedItem)
            {
                case 0:
                    // Increase player's health if they have not reached the maximum
                    if (_player.Health < 4)
                    {
                        _player.Health++;
                        UIManager.Instance.UpdateHealth(_player.Health);
                    }
                    else
                        _currentItemelectedCost = 0;    // Prevent the player from purchasing the item again
                    break;
                case 1:
                    
                    if (_player.Health < 4)
                    {
                        _player.Health = 4; // Set player's health to the maximum
                        UIManager.Instance.UpdateHealth(_player.Health);
                    }
                    else
                        _currentItemelectedCost = 0;
                    break;
                case 2:
                    // Enable the key to the castle in the game manager
                    if (!GameManager.Instance.HasKeyToCastle)
                    {
                        GameManager.Instance.HasKeyToCastle = true;
                        UIManager.Instance.UpdateOwnedItem(2);
                    }
                    else
                        _currentItemelectedCost = 0;
                    break;
                default:
                    Debug.Log("Invalid Item");
                    break;
            }

            _player.Diamonds -= _currentItemelectedCost;  // Deduct the cost from player's diamonds
            AudioManager.Instance.PlayClickSound();  // Deduct the cost from player's diamonds and update UI
            UIManager.Instance.UpdatePlayerDiamondCount(_player.Diamonds);  // Update UI with player's diamond count

            UpdateShopUIOwnedItem();
        }
    }

    // Update the UI interactability based on the player's health
    private void UpdateShopUIOwnedItem()
    {
        if (_player.Health == 4)
        {
            UIManager.Instance.UpdateOwnedItem(0);  // Interactable = false
            UIManager.Instance.UpdateOwnedItem(1);  // Interactable = false
        }
        else
        {
            UIManager.Instance.UpdateOwnedItem(3);  // Interactable = true
            UIManager.Instance.UpdateOwnedItem(4);  // Interactable = true
        }
    }

    // Method to close the shop panel
    public void ClosePanel()
    {
        Time.timeScale = 1f;    // Resume the game when exiting the shop area
        AudioManager.Instance.TogglePause();   // Resume the game audio when exiting the shop
        AudioManager.Instance.PlayClickSound();
        _shopPanel.SetActive(false);
    }
}