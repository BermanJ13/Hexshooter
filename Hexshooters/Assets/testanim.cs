using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animation))]
[ExecuteInEditMode]
public class LegacyAnimateSpriteRenderer : MonoBehaviour
{
    public float Frame;      // Animate this value with the legacy Animator using the timeline, you can preview changes in edit mode.
    public Sprite[] Frames;     // Here yoy add all the sprites that will be animated
    private SpriteRenderer Renderer;

    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        int frame = Mathf.FloorToInt(Frame);

        if (frame < 0 || Frames == null)
        {
            Frame = 0f;
            return;
        }

        if (frame >= Frames.Length)
        {
            Frame = Frames.Length;
            return;
        }

        if (Frames[frame] != Renderer.sprite)
            Renderer.sprite = Frames[frame];
    }

    //This executes in Editor mode, we call Update so you can preview your animation in the editor.
    void OnRenderObject()
    {
        if (!Application.isPlaying)
            Update();
    }
}
