using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColliderSizer : MonoBehaviour
{
    Camera mainCamera;
    private BoxCollider2D boxcoll;
    private float targetAspect = 16.0f / 9.0f;
    private int ScreenSizeX = 0;
    private int ScreenSizeY = 0;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        boxcoll = GetComponent<BoxCollider2D>();

        RescaleCamera();
    }


    private void RescaleCamera()
    {

        if (Screen.width == ScreenSizeX && Screen.height == ScreenSizeY) return;

        float targetaspect = 16.0f / 9.0f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;
        Camera camera = GetComponent<Camera>();

        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }

        ScreenSizeX = Screen.width;
        ScreenSizeY = Screen.height;
    }

    void OnPreCull()
    {
        if (Application.isEditor) return;
        Rect wp = Camera.main.rect;
        Rect nr = new Rect(0, 0, 1, 1);

        Camera.main.rect = nr;
        GL.Clear(true, true, Color.black);

        Camera.main.rect = wp;

    }

    void UpdateSize(float width,float height)
    {
        boxcoll.size = new Vector2(width, height);
    }

    // Update is called once per frame
    void Update()
    {
        float targetAspect = 16.0f / 9.0f; // Dla przyk³adu proporcji 16:9
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera.main.rect = new Rect(0.0f, (1.0f - scaleHeight) / 2.0f, 1.0f, scaleHeight);


        float height = mainCamera.orthographicSize*2;
        float width = height * mainCamera.aspect;
        //Debug.Log("Width: " + width + " Height: " + height);
        UpdateSize(width, height);
    }
}
