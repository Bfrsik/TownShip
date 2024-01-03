using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Mode currentMode;
    [SerializeField] private TileController Tiles;
    [SerializeField] private StructuresData data;
    [SerializeField] private float cameraSpeed = 10;
    [HideInInspector] public GameObject controllerCamera;
    [SerializeField] private Unit player;
    [SerializeField] private float cameraDirX = 20;
    [SerializeField] private float cameraDirY = 20;
    


    private void Start()
    {
        controllerCamera = new GameObject();
        controllerCamera.transform.parent = player.transform;
        controllerCamera.transform.localPosition = Vector3.zero;
        Camera.main.transform.parent = controllerCamera.transform;
        currentMode = new Playing(player,controllerCamera, Tiles, data, cameraSpeed, cameraDirX,cameraDirY);
    }

    private void Update()
    {
        currentMode = currentMode.Process();
    }
}
