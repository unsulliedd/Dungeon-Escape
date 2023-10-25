using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _touchControls;
    [SerializeField] private TextMeshProUGUI _playerDiamondCount;
    [SerializeField] private Image _selectionIMG;
    [SerializeField] private Button item0btn;
    [SerializeField] private Button item1btn;
    [SerializeField] private Button item2btn;

    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("UIManager is NULL");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        if (IsTouchDevice())
            _touchControls.SetActive(true);
    }

    public void UpdatePlayerDiamondCount(int diamonds)
    {
        _playerDiamondCount.text = "" + diamonds + " Diamond";
    }

    public void UpdateShopSelection(int yPos)
    {
        _selectionIMG.rectTransform.anchoredPosition = new Vector2(_selectionIMG.rectTransform.anchoredPosition.x, yPos);
    }

    public void UpdateOwnedItem(int item)
    {
        switch (item)
        {
            case 0:
                item0btn.interactable = false;
                break;
            case 1:
                item1btn.interactable = false;
                break;
            case 2:
                item2btn.interactable = false;
                break;
            default:
            Debug.Log("Invalid Item");
                break;
        }
    }

    bool IsTouchDevice()
    {
        return Input.touchSupported;
    }
}
