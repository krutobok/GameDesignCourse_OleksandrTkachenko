using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    public static CursorController Instance;

    public Image cursorImage;
    public Sprite defaultCursor;
    public Sprite handCursor;

    private bool cursorActive = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Початковий стан курсора
        SetCursorActive(false);
    }

    private void Update()
    {
        if (cursorActive)
        {
            // Курсор рухається разом з мишкою
            cursorImage.rectTransform.position = Input.mousePosition;
        }
        else
        {
            // Курсор заблокований у центрі
            cursorImage.rectTransform.position = new Vector2(Screen.width / 2f, Screen.height / 2f);
        }
    }

    public void SetCursorToDefault()
    {
        cursorImage.sprite = defaultCursor;
    }

    public void SetCursorToHand()
    {
        cursorImage.sprite = handCursor;
    }

    /// <summary>
    /// Активує або блокує курсор
    /// </summary>
    /// <param name="active">true – курсор вільний, false – заблокований</param>
    public void SetCursorActive(bool active)
    {      
        cursorActive = active;

        // Візуальна частина
        cursorImage.enabled = true; // курсор завжди видно, просто фіксується

        // Системна поведінка курсора
        Cursor.visible = false; // системний курсор не показується
        Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
    }
}


