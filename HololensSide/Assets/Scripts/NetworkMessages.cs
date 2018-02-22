﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class VirtualHausMessageType : MsgType
{
    public static short CONNECTED = 1010;

    public static short APPARTMENT_LOADING = 1020;
    public static short APPARTMENT_LOADED = 1021;

    public static short FURNITURE_LOADING = 1030;
    public static short USER_READY = 1040;

    public static short USER_POSITION = 1050;
    public static short NEW_FURNITURE_POSITION = 1070;
}

public class EmptyMessage : MessageBase { }

public class AppartmentLoadingMessage : MessageBase
{
    public string modelName;
    public int roomQuantity;

    public Vector3 appartmentScale;
    public Vector3 appartmentPosition;
}
public class FurnitureLoadingMessage : MessageBase
{
    public string jsonFurnitures;
}

public class UserPositionMessage : MessageBase
{
    public Vector3 userPosition;
    public Quaternion userRotation;
}
public class NewFurniturePositionMessage : MessageBase
{
    public string furnitureName;
    public Vector3 furniturePosition;
    public Quaternion furnitureRotation;
}

/************************************************************************/
/* Serializable class for Furniture Loading Message (converted in JSON) */
/************************************************************************/

[Serializable]
public class NewFurnituresInformations
{
    public List<NewFurnitureInformations> list;

    public NewFurnituresInformations(int roomIndex)
    {
        list = new List<NewFurnitureInformations>();
        Transform room = GameObject.Find("Furnitures").transform.GetChild(roomIndex);

        for (int i = 0; i < room.childCount; i++)
        {
            GameObject furniture = room.GetChild(i).gameObject;

            list.Add(new NewFurnitureInformations()
            {
                furnitureName = furniture.name,
                prefabName = PrefabUtility.GetPrefabParent(furniture).name,

                furniturePosition = furniture.transform.position,
                furnitureRotation = furniture.transform.rotation
            });
        }
    }
}
[Serializable]
public class NewFurnitureInformations
{
    public string furnitureName;
    public string prefabName;

    public Vector3 furniturePosition;
    public Quaternion furnitureRotation;
}