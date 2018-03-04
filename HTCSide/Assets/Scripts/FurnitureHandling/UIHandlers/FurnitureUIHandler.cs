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
    public GameObject furnitures;

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

    private double scrollStack;
    private bool canClick;


    void Start() {
		scrollViewHeight = scrollView.GetComponent<RectTransform>().rect.height;
        rightSideHeight = rightSide.GetComponent<RectTransform>().rect.height;

        leftPartUIItemHeight = leftPartUIItem.GetComponent<RectTransform>().rect.height;
        rightPartUIItemHeight = rightPartUIItem.GetComponent<RectTransform>().rect.height;

        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        modHandler = GameObject.Find("ModHandler").GetComponent<ModHandler>();
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        dragFurniture = GameObject.Find("EditionHandler").GetComponent<DragFurniture>();

        scrollStack = 0;

        CreateUI();
    }


    void Update()
    {
        Scroll();

        if (canClick)
        {
            Select();
        }
        else
        {
            canClick = !inputManager.IsTriggerClicked();
        }
    }

    private void Scroll()
    {
        scrollStack += inputManager.GetTrackpadHandler().GetMenuTrackpadRotationOffset();

        if (!(Mathf.Abs((float) scrollStack) >= 3))
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

    private void Select()
    {
        if (!modHandler.IsInEditionMod() || !inputManager.IsTriggerClicked())
            return;

        if (!rayCast.Hit())
            return;

        canClick = false;
        Transform hitObject = rayCast.GetHit().transform;

        if (hitObject.parent == leftSide.transform)
        {
            UpdateRightUIPart(hitObject.GetSiblingIndex());
        }
        else if (hitObject.parent == rightSide.transform)
        {
            Transform ui = rayCast.GetHit().transform;
            dragFurniture.SelectObject(GameObject.Find(ui.GetChild(0).GetComponent<Text>().text));

            Color color = ui.GetChild(2).GetComponent<Image>().color;
            color.a = 0.5f;
            ui.GetChild(2).GetComponent<Image>().color = color;
        }
    }

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
    private void UpdateRightUIPart(int index)
    {
        foreach (Transform child in rightSide.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        Transform room = furnitures.transform.GetChild(index);
        int furnitureQuantity = room.childCount;

        Vector2 size = rightSide.GetComponent<RectTransform>().sizeDelta;
        rightSideHeight = size.y = furnitureQuantity / 2 + furnitureQuantity % 2;
        rightSide.GetComponent<RectTransform>().sizeDelta = size;

        for (int i = 0; i < furnitureQuantity; i++)
        {
            GameObject temp = Instantiate(rightPartUIItem, rightSide.transform);
            temp.name = room.GetChild(i).name + "_ui";

            Vector2 position = temp.GetComponent<RectTransform>().anchoredPosition;
            position.y -= rightPartUIItemHeight * ((int) i / 2) + 0.5f;
            temp.GetComponent<RectTransform>().anchoredPosition = position;

            if (i % 2 == 1)
            {
                Vector2 pivot = temp.GetComponent<RectTransform>().pivot;
                pivot.x = -1;
                temp.GetComponent<RectTransform>().pivot = pivot;

                Vector3 center = temp.GetComponent<BoxCollider>().center;
                center.x += 1;
                temp.GetComponent<BoxCollider>().center = center;
            }

            temp.GetComponentInChildren<Text>().text = room.GetChild(i).name;
            Texture2D texture = new Texture2D(512, 512);
            texture.LoadImage(File.ReadAllBytes(ThumbnailsHandler.thumbnailsPath + room.GetChild(i).name + ".png"));

            temp.GetComponentInChildren<RawImage>().texture = texture;
        }
    }
}
