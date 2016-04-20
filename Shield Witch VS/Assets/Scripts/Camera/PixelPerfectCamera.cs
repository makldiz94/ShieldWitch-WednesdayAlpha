﻿using UnityEngine;
using System.Collections;

public class PixelPerfectCamera : MonoBehaviour {

    public static float pixelsToUnits = 1f;
    public static float scale = 1f;

    public static float targetAspect = 16f / 15f;
    public static float windowaspect = (float)Screen.width / (float)Screen.height;
    public float scaleheight = windowaspect / targetAspect;

    public Vector2 nativeResolution = new Vector2(1920, 1080);

    void Awake() {
        //var camera = GetComponent<Camera>();

        /*if (camera.orthographic) {
			scale = Screen.height/nativeResolution.y;
			pixelsToUnits *= scale;
			camera.orthographicSize = (Screen.height / 2.0f) / pixelsToUnits;
		} */
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
            float scalewidth = 1.10f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
        
    }
}

