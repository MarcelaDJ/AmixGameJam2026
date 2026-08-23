using UnityEngine;
using UnityEngine.InputSystem;

public class HandCursorController : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Animator handAnimator;
    [SerializeField] private Canvas parentCanvas;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (handAnimator == null) handAnimator = GetComponent<Animator>();

        if (parentCanvas == null)
        {
            parentCanvas = GetComponentInParent<Canvas>();
        }

        Cursor.visible = false;
    }

    private void Update()
    {
        if (Mouse.current != null && parentCanvas != null)
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                mouseScreenPos,
                parentCanvas.worldCamera,
                out Vector2 localPoint
            );

            rectTransform.anchoredPosition = localPoint;

            
            bool isPressing = Mouse.current.leftButton.isPressed;
            if (handAnimator != null)
            {
                handAnimator.SetBool("isGrabbing", isPressing);
            }
        }
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }
}