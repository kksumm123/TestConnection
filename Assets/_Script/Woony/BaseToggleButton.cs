using UnityEngine;
using UnityEngine.UI;

public abstract class BaseToggleButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] protected Sprite onSprite;
    [SerializeField] protected Sprite offSprite;

    protected bool ButtonToggleState
    {
        get => _buttonToggleState;
        set
        {
            _buttonToggleState = value;
            if (_buttonToggleState)
            {
                ToggleOn();
            }
            else
            {
                ToggleOff();
            }

        }
    }
    private bool _buttonToggleState;

    public void Initialize()
    {
        WoonyMethods.Asserts(this,
            (button, nameof(button)),
            (image, nameof(image)),
            (onSprite, nameof(onSprite)),
            (offSprite, nameof(offSprite)));

        button.onClick.AddListener(() => ButtonToggleState = !ButtonToggleState);
        OnInitialize();
    }

    protected virtual void OnInitialize() { }

    private void ToggleOn()
    {
        image.sprite = onSprite;
        OnToggleOn();
    }

    private void ToggleOff()
    {
        image.sprite = offSprite;
        OnToggleOff();
    }

    protected abstract void OnToggleOn();

    protected abstract void OnToggleOff();
}
