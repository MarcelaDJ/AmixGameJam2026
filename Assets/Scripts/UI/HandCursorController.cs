using UnityEngine;
using UnityEngine.InputSystem; 

public class HandCursorController : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Animator handAnimator;
    [SerializeField] private Vector2 offset = new Vector2(0, -20f);

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (handAnimator == null) handAnimator = GetComponent<Animator>();

        
        Cursor.visible = false;
    }

    private void Update()
    {
        
        if (Mouse.current != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            rectTransform.position = mousePos + offset;

            
            bool isPressing = Mouse.current.leftButton.isPressed;
            SetGrabbing(isPressing);
        }
    }

    public void SetGrabbing(bool isGrabbing)
    {
        if (handAnimator != null)
        {
            handAnimator.SetBool("isGrabbing", isGrabbing);
        }
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }
}