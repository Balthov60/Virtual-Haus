using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureUIHandler : MonoBehaviour {

    public GameObject leftSide;
    public GameObject rightSide;
    public GameObject scrollView;
    private GameObject furnitures;

    public GameObject leftPartUIItem;
    public GameObject rightPartUIItem;

    private InputManager inputManager;
    private ModHandler modHandler;
    private RayCast rayCast;
    private DragFurniture dragFurniture;

    private float scrollViewHeight;
    private float rightSideHeight;
    private float leftPartUIItemHeight;
    private float rightPartUIItemHeight;

    private float realHeight = 0;

    private double scrollStack;
    private bool canClick;


    private void Start()
    {
        scrollViewHeight = scrollView.GetComponent<RectTransform>().rect.height;
        rightSideHeight = rightSide.GetComponent<RectTransform>().rect.height;

        leftPartUIItemHeight = leftPartUIItem.GetComponent<RectTransform>().rect.height;
        rightPartUIItemHeight = rightPartUIItem.GetComponent<RectTransform>().rect.height;

        furnitures = GameObject.Find("Furnitures");
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        modHandler = GameObject.Find("ModHandler").GetComponent<ModHandler>();
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        dragFurniture = GameObject.Find("EditionHandler").GetComponent<DragFurniture>();

        scrollStack = 0;

        CreateUI();
    }

    /**************************/
    /* UI Interaction Handler */
    /**************************/

    private void Update()
    {
        if (scrollViewHeight < realHeight)
            Scroll();

        Select();
    }

    /// <summary>
    /// Scroll if user scroll on the trackpad. Use a scroll stack to prevent unwanted click.
    /// </summary>
    private void Scroll()
    {
        scrollStack += inputManager.GetTrackpadHandler().GetMenuTrackpadRotationOffset();

        if (!(Mathf.Abs((float)scrollStack) >= 3))
            return;

        if (scrollStack < -200 || scrollStack > 0) // test if scrollStack < -200 for gap issues when angle go from 360 to 0
        {
            Vector2 pos = rightSide.GetComponent<RectTransform>().anchoredPosition;
            pos.y -= 0.1f;

            if (pos.y < 0)
                pos.y = 0;

            rightSide.GetComponent<RectTransform>().anchoredPosition = pos;
        }
        else
        {
            Vector2 pos = rightSide.GetComponent<RectTransform>().anchoredPosition;
            pos.y += 0.1f;

            if (scrollViewHeight + pos.y > rightSideHeight)
                pos.y = rightSideHeight - scrollViewHeight;

            rightSide.GetComponent<RectTransform>().anchoredPosition = pos;
        }

        scrollStack = 0;
    }
    /// <summary>
    /// Test if user want to select an item and handle is request if so.
    /// </summary>
    private void Select()
    {
        if (!modHandler.IsInEditionMod() || !inputManager.UserClick())
            return;

        Transform hitObject = rayCast.GetHit().transform;

        if (hitObject.parent == leftSide.transform)
        {
            inputManager.CanClick = false;
            UpdateRightUIPart(hitObject.GetSiblingIndex());
        }
        else if (hitObject.parent == rightSide.transform)
        {
            inputManager.CanClick = false;
            Transform ui = rayCast.GetHit().transform;
            dragFurniture.SelectObject(GameObject.Find(ui.GetChild(0).GetComponent<Text>().text));
            SetFurnitureSelected(ui);
        }
    }

    /*******************/
    /* UI Manipulation */
    /*******************/

    private void CreateUI()
    {
        ThumbnailsHandler.CreateThumbnailsIfNotExist(furnitures);

        UpdateLeftUIPart();
        UpdateRightUIPart(0);
    }

    private void UpdateLeftUIPart()
    {
        for (int i = 0; i < furnitures.transform.childCount; i++)
        {
            GameObject temp = Instantiate(leftPartUIItem, leftSide.transform);
            Vector2 position = temp.GetComponent<RectTransform>().anchoredPosition;
            position.y -= leftPartUIItemHeight * i;

            temp.GetComponent<RectTransform>().anchoredPosition = position;
            temp.GetComponentInChildren<Text>().text = furnitures.transform.GetChild(i).name;
        }
    }
    public void UpdateRightUIPart(int index)
    {
        foreach (Transform child in rightSide.transform)
            Destroy(child.gameObject);

        Transform room = furnitures.transform.GetChild(index);
        int furnitureQuantity = room.childCount;
        UpdateUISize(furnitureQuantity);

        for (int i = 0; i < furnitureQuantity; i++)
        {
            GameObject item = AddNewUIFor(room, i);
            if (room.GetChild(i).position != DragFurniture.DEFAULT_FURNITURE_POSITION)
                SetFurnitureSelected(item.transform);
        }
    }

    private GameObject AddNewUIFor(Transform room, int index)
    {
        GameObject temp = Instantiate(rightPartUIItem, rightSide.transform);
        temp.name = room.GetChild(index).name + "_ui";

        Vector2 position = temp.GetComponent<RectTransform>().anchoredPosition;
        position.y -= rightPartUIItemHeight * (index / 2) + 0.5f;
        temp.GetComponent<RectTransform>().anchoredPosition = position;

        if (index % 2 == 1)
        {
            Vector2 pivot = temp.GetComponent<RectTransform>().pivot;
            pivot.x = -1;
            temp.GetComponent<RectTransform>().pivot = pivot;

            Vector3 center = temp.GetComponent<BoxCollider>().center;
            center.x += 1;
            temp.GetComponent<BoxCollider>().center = center;
        }

        temp.GetComponentInChildren<Text>().text = room.GetChild(index).name;
        Texture2D texture = new Texture2D(512, 512);
        texture.LoadImage(File.ReadAllBytes(ThumbnailsHandler.thumbnailsPath + room.GetChild(index).name + ".png"));

        temp.GetComponentInChildren<RawImage>().texture = texture;

        return temp;
    }

    private void UpdateUISize(int furnitureQuantity)
    {
        Vector2 size = rightSide.GetComponent<RectTransform>().sizeDelta;
        rightSideHeight = size.y = furnitureQuantity / 2 + furnitureQuantity % 2;
        rightSide.GetComponent<RectTransform>().sizeDelta = size;

        realHeight = size.y;
    }
    private void SetFurnitureSelected(Transform ui)
    {
        Color color = ui.GetChild(2).GetComponent<Image>().color;
        color.a = 0.5f;
        ui.GetChild(2).GetComponent<Image>().color = color;
    }
}
