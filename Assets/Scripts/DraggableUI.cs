using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableUI : MonoBehaviour, IDragHandler
{
    // Positions
    private Vector2 defaultPosition;
    private Vector2 lastValidPosition;
    // Buttons
    private Button defaultLocationButton;
    private Button saveControlsLocation;

    // Player Prefs Keys
    private string playerPrefsKeyX;
    private string playerPrefsKeyY;

    // References
    private RectTransform rectTransform;

    void Awake()
    {
        // Get the RectTransform component of the UI element
        rectTransform = GetComponent<RectTransform>();

        // Set the default position to the initial position
        defaultPosition = rectTransform.position;

        // Try to find and assign the DefaultLocationButton and SaveControlsLocation buttons
        GameObject.FindGameObjectWithTag("DefaultLocationButton").TryGetComponent(out defaultLocationButton);
        GameObject.FindGameObjectWithTag("SaveControlsLocation").TryGetComponent(out saveControlsLocation);
    }

    void Start()
    {
        // Set the default position to the initial position
        defaultPosition = rectTransform.position;

        // Generate unique PlayerPrefs keys based on the UI element's name for X and Y positions
        playerPrefsKeyX = $"DraggableUI_{gameObject.name}_PosX";
        playerPrefsKeyY = $"DraggableUI_{gameObject.name}_PosY";

        // Check if PlayerPrefs has saved positions for the UI element
        if (PlayerPrefs.HasKey(playerPrefsKeyX) && PlayerPrefs.HasKey(playerPrefsKeyY))
        {
            // Retrieve the saved X and Y positions from PlayerPrefs
            float x = PlayerPrefs.GetFloat(playerPrefsKeyX);
            float y = PlayerPrefs.GetFloat(playerPrefsKeyY);

            // Set the last valid position to the loaded position
            lastValidPosition = new Vector2(x, y);

            // Set the initial position to the loaded position
            rectTransform.position = lastValidPosition;
        }
        else
            // If no saved positions, set the last valid position to the default position
            lastValidPosition = defaultPosition;

        // Attach the ResetToDefaultPosition method to the DefaultLocationButton's click event
        if (defaultLocationButton)
            defaultLocationButton.onClick.AddListener(ResetToDefaultPosition);

        // Attach the SaveControlLocation method to the SaveControlsLocation button's click event
        if (saveControlsLocation)
            saveControlsLocation.onClick.AddListener(SaveControlLocation);
    }

    // Called when the UI element is dragged
    public void OnDrag(PointerEventData eventData)
    {
        // Update the UI element's position based on the drag event
        rectTransform.position = eventData.position;

        // Check if the new position is valid, revert to the last valid position if not
        if (!IsPositionValid())
            rectTransform.position = lastValidPosition;
        else
        {
            // Update the last valid position and save the new position
            lastValidPosition = rectTransform.position;
            SaveControlLocation();
        }
    }

    // Save the current UI element's position to PlayerPrefs
    private void SaveControlLocation()
    {
        PlayerPrefs.SetFloat(playerPrefsKeyX, lastValidPosition.x);
        PlayerPrefs.SetFloat(playerPrefsKeyY, lastValidPosition.y);
        PlayerPrefs.Save();
    }

    // Check if the current UI element's position is valid (does not overlap with other draggable UI elements)
    private bool IsPositionValid()
    {
        foreach (DraggableUI otherDraggable in FindObjectsOfType<DraggableUI>())
        {
            if (otherDraggable != this)
            {
                RectTransform otherRectTransform = otherDraggable.GetComponent<RectTransform>();

                // Check if the current UI element's position is within the bounds of other draggable UI elements
                if (RectTransformUtility.RectangleContainsScreenPoint(otherRectTransform, rectTransform.position))
                    return false;
            }
        }
        return true;
    }

    // Reset the UI element's position to the default position and save the changes
    public void ResetToDefaultPosition()
    {
        rectTransform.position = defaultPosition;
        lastValidPosition = defaultPosition;

        // Reset the player prefs to the default position
        PlayerPrefs.SetFloat(playerPrefsKeyX, lastValidPosition.x);
        PlayerPrefs.SetFloat(playerPrefsKeyY, lastValidPosition.y);
        PlayerPrefs.Save();
    }
}