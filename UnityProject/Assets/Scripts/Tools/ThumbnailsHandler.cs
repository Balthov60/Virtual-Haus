using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public static class ThumbnailsHandler
{
    public static string thumbnailsPath = Application.dataPath + "/UIComponents/Thumbnails/";
    private static readonly int SIZE = 512;

    private static RenderTexture renderTexture;
    private static GameObject cameraGameObject;

    public static void CreateThumbnailsIfNotExist(GameObject furnitures)
    {
        renderTexture = new RenderTexture(SIZE, SIZE, 24);
        cameraGameObject = CreateCamera(renderTexture);

        for (int i = 0; i < furnitures.transform.childCount; i++)
        {
            CreateThumbnailsIfNotExistForRoom(furnitures, i);
        }

        UnityEngine.Object.DestroyImmediate(cameraGameObject);
        UnityEngine.Object.DestroyImmediate(renderTexture);
    }

    private static GameObject CreateCamera(RenderTexture renderTexture)
    {
        GameObject cameraGameObject = new GameObject();

        Camera camera = cameraGameObject.AddComponent<Camera>();
        camera.aspect = 1.0f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0, 0, 0, 0);

        camera.targetTexture = renderTexture;

        return cameraGameObject;
    }
    private static void CreateThumbnailsIfNotExistForRoom(GameObject furnitures, int roomIndex)
    {
        Transform room = furnitures.transform.GetChild(roomIndex);
        int furnituresQuantity = room.childCount;

        for (int i = 0; i < furnituresQuantity; i++)
        {
            if (FurnitureThumbnailsExist(room.GetChild(i).name)) continue;
            
            GameObject furnitureToRender = UnityEngine.Object.Instantiate(room.GetChild(i).gameObject);
            PlaceGameObjectForScreenShot(furnitureToRender, cameraGameObject);

            File.WriteAllBytes(thumbnailsPath + room.GetChild(i).name + ".png", TakePicture().EncodeToPNG());

            UnityEngine.Object.DestroyImmediate(furnitureToRender);
        }
    }
    private static Texture2D TakePicture()
    {
        cameraGameObject.GetComponent<Camera>().Render();
        RenderTexture.active = renderTexture;
        Texture2D virtualPhoto = new Texture2D(SIZE, SIZE, TextureFormat.ARGB32, false);
        virtualPhoto.ReadPixels(new Rect(0, 0, SIZE, SIZE), 0, 0);
        RenderTexture.active = cameraGameObject.GetComponent<Camera>().targetTexture = null;

        return virtualPhoto;
    }

    private static bool FurnitureThumbnailsExist(string name)
    {
        return File.Exists(thumbnailsPath + name + ".png");
    }
    private static void PlaceGameObjectForScreenShot(GameObject furniture, GameObject camera)
    {
        furniture.transform.position = new Vector3(-100, 0);
        Vector3 furnitureSize = furniture.GetComponent<Renderer>().bounds.size;

        float y = furnitureSize.y / 2;
        float z = (furnitureSize.y > furnitureSize.x) ? furnitureSize.y : furnitureSize.x;

        camera.transform.position = new Vector3(-100, y, -z);

        Vector3 lookAt = furniture.transform.position;
        lookAt.y += y;

        camera.transform.LookAt(lookAt);
    }
}