using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[System.Serializable]
public class CursorAnimationData
{
    public string name;                  
    public Sprite[] cursorFrames;        
    public float frameRate = 0.1f;       
    public float cursorScale = 1.5f;     // Escala recomendada: entre 1.2 y 2.5
    public Vector2 hotSpot = Vector2.zero; 
    public string[] objectTags;          

    [HideInInspector] public Texture2D[] cachedTextures; // Almacena las texturas procesadas
}

public class MultiCursorUIHandler : MonoBehaviour
{
    [Header("Configuración de Animaciones")]
    [SerializeField] private CursorAnimationData defaultAnimation;
    [SerializeField] private List<CursorAnimationData> customAnimations;

    private CursorAnimationData currentAnimation;
    private int currentFrame;
    private float timer;

    private void Awake()
    {
        // Pre-procesar y escalar todas las texturas una sola vez al cargar la escena
        PreprocessAnimation(defaultAnimation);
        foreach (var anim in customAnimations)
        {
            PreprocessAnimation(anim);
        }
    }

    private void Start()
    {
        SetCurrentAnimation(defaultAnimation);
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        if (EventSystem.current != null)
        {
            EventSystem.current.RaycastAll(eventData, results);
        }

        CursorAnimationData matchedAnim = null;

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == null) continue;
            GameObject go = result.gameObject;

            if (go.GetComponent<UnityEngine.UI.Button>() != null || go.CompareTag("Button"))
            {
                matchedAnim = GetAnimationForTag("Button");
                if (matchedAnim != null) break;
            }

            if (!go.CompareTag("Untagged") && go.CompareTag("Sticker"))
            {
                matchedAnim = GetAnimationForTag("Sticker");
                if (matchedAnim != null) break;
            }
        }

        if (matchedAnim == null)
        {
            matchedAnim = defaultAnimation;
        }

        if (currentAnimation != matchedAnim)
        {
            SetCurrentAnimation(matchedAnim);
        }

        AnimateCursor();
    }

    private void PreprocessAnimation(CursorAnimationData anim)
    {
        if (anim == null || anim.cursorFrames == null || anim.cursorFrames.Length == 0) return;

        anim.cachedTextures = new Texture2D[anim.cursorFrames.Length];

        for (int i = 0; i < anim.cursorFrames.Length; i++)
        {
            if (anim.cursorFrames[i] != null)
            {
                anim.cachedTextures[i] = ScaleSpriteTexture(anim.cursorFrames[i], anim.cursorScale);
            }
        }
    }

    private Texture2D ScaleSpriteTexture(Sprite sprite, float scale)
    {
        int width = Mathf.RoundToInt(sprite.rect.width * scale);
        int height = Mathf.RoundToInt(sprite.rect.height * scale);

        Texture2D croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);
        Color[] pixels = sprite.texture.GetPixels(
            (int)sprite.rect.x,
            (int)sprite.rect.y,
            (int)sprite.rect.width,
            (int)sprite.rect.height
        );
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        if (Mathf.Approximately(scale, 1.0f) || scale <= 0)
        {
            return croppedTexture;
        }

        Texture2D scaledTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color color = croppedTexture.GetPixelBilinear((float)x / width, (float)y / height);
                scaledTexture.SetPixel(x, y, color);
            }
        }
        scaledTexture.Apply();
        return scaledTexture;
    }

    private void SetCurrentAnimation(CursorAnimationData anim)
    {
        if (anim == null || anim.cachedTextures == null || anim.cachedTextures.Length == 0) return;

        currentAnimation = anim;
        currentFrame = 0;
        timer = 0f;

        ApplyCursorFrame(0);
    }

    private void AnimateCursor()
    {
        if (currentAnimation == null || currentAnimation.cachedTextures == null || currentAnimation.cachedTextures.Length <= 1) return;

        timer += Time.deltaTime;
        if (timer >= currentAnimation.frameRate)
        {
            timer -= currentAnimation.frameRate;
            currentFrame = (currentFrame + 1) % currentAnimation.cachedTextures.Length;

            ApplyCursorFrame(currentFrame);
        }
    }

    private void ApplyCursorFrame(int frameIndex)
    {
        if (currentAnimation.cachedTextures == null || frameIndex >= currentAnimation.cachedTextures.Length) return;

        Texture2D texture = currentAnimation.cachedTextures[frameIndex];
        if (texture != null)
        {
            Cursor.SetCursor(texture, currentAnimation.hotSpot * currentAnimation.cursorScale, CursorMode.Auto);
        }
    }

    private CursorAnimationData GetAnimationForTag(string tag)
    {
        for (int i = 0; i < customAnimations.Count; i++)
        {
            if (customAnimations[i].objectTags == null) continue;
            for (int j = 0; j < customAnimations[i].objectTags.Length; j++)
            {
                if (customAnimations[i].objectTags[j] == tag)
                    return customAnimations[i];
            }
        }
        return null;
    }

    private void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}