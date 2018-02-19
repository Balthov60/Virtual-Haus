using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ThumbnailsHandler : MonoBehaviour
{

    private GameObject furnitures;

    void Start()
    {
        furnitures = GameObject.Find("FurnitureMenu").GetComponent<FurnitureUIHandler>().furnitures;
        CreateThumbnails(0);
    }


    private void CreateThumbnails(int roomIndex)
    {
        Transform room = furnitures.transform.GetChild(roomIndex);
        int furnituresQuantity = room.childCount;

        for (int i = 0; i < furnituresQuantity; i++)
        {
            GameObject furnitureToRender = Instantiate(room.GetChild(i).gameObject);
            GameObject cameraGameObject = new GameObject(room.GetChild(i).name + "Camera");
            Camera camera = cameraGameObject.AddComponent<Camera>();

            furnitureToRender.transform.position = new Vector3(i * 10, -50 * (roomIndex + 1), 0);
            cameraGameObject.transform.position = new Vector3(i * 10, -50 * (roomIndex + 1), -2);
            cameraGameObject.transform.LookAt(furnitureToRender.transform);

            RenderTexture renderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            renderTexture.Create();

            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.targetTexture = renderTexture;
            camera.backgroundColor = new Color(0, 0, 0, 0);

            AssetDatabase.CreateAsset(renderTexture, "Assets/UIComponents/Thumbnails/" + room.GetChild(i).name + ".renderTexture");
        }
    }
}