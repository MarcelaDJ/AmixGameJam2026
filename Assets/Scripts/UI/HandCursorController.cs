using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HandCursorController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Animator handAnimator;
    [SerializeField] private Canvas parentCanvas;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (handAnimator == null) 
            handAnimator = GetComponent<Animator>();

        if (parentCanvas == null) 
            parentCanvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        // Oculta el puntero por defecto de Windows al iniciar
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Mouse.current == null || parentCanvas == null) return;

        // 1. Obtener posición del ratón
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        // 2. Convertir coordenadas a la UI
        Camera cam = (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : parentCanvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                mousePosition,
                cam,
                out Vector2 localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }

        // 3. Raycast UI para detectar botones o stickers
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        if (EventSystem.current != null)
        {
            EventSystem.current.RaycastAll(eventData, results);
        }

        bool sobreBoton = false;
        bool sobreSticker = false;

        foreach (RaycastResult result in results)
        {
            GameObject go = result.gameObject;

            if (go == gameObject || go.transform.IsChildOf(transform)) 
                continue;

            if (go.GetComponent<UnityEngine.UI.Button>() != null || go.CompareTag("Button"))
            {
                sobreBoton = true;
                break;
            }

            if (!go.CompareTag("Untagged") && go.CompareTag("Sticker"))
            {
                sobreSticker = true;
                break;
            }
        }

        // 4. Mandar parámetros al Animator
        if (handAnimator != null)
        {
            handAnimator.SetBool("isPuntero", sobreBoton);
            handAnimator.SetBool("isSticker", sobreSticker);
        }
    }

    private void OnDisable()
    {
        // Muestra de nuevo el cursor si se desactiva el objeto
        Cursor.visible = true;
    }
}