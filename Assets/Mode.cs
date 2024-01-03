using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Mode
{
    public Name name;
    protected Unit player;
    protected GameObject controllerCamera;
    protected EVENT stage;
    protected Mode nextMode;
    protected TileController tiles;
    protected float cameraSpeed;
    protected float cameraDirX;
    protected float cameraDirY;
    protected StructuresData structures;

    protected Vector3Int GetMouseOnGridPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mouseCellPos = tiles.buildings.WorldToCell(mousePos);
        mouseCellPos.z = 0;

        return mouseCellPos;
    }

    public enum Name
    {
        PLAYING,
        BUILDING
    }

    public enum EVENT
    {
        ENTER,
        UPDATE,
        EXIT
    }

    protected virtual void Enter()
    {
        stage = EVENT.UPDATE;
    }

    protected virtual void Update()
    {
        stage = EVENT.UPDATE;
    }

    protected virtual void Exit()
    {
        stage = EVENT.EXIT;
    }

    public Mode Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage != EVENT.EXIT) return this;
        Exit();
        return nextMode;
    }

    public virtual void InputController()
    {
    }

    protected Mode(Unit player, GameObject controllerCamera, TileController tiles, StructuresData structures, float cameraSpeed, float cameraDirX,
        float cameraDirY)
    {
        this.controllerCamera = controllerCamera;
        this.player = player;
        this.structures = structures;
    }
}

public class Playing : Mode
{
    public Playing(Unit player, GameObject controllerCamera, TileController tiles, StructuresData structures, float cameraSpeed, float cameraDirX,
        float cameraDirY) : base(player, controllerCamera, tiles, structures, cameraSpeed, cameraDirX, cameraDirY)
    {
        this.structures = structures;
        this.tiles = tiles;
        this.cameraSpeed = cameraSpeed;
        this.cameraDirX = cameraDirX;
        this.cameraDirY = cameraDirY;
        this.controllerCamera = controllerCamera;
        name = Name.PLAYING;
        this.player = player;
    }

    protected override void Enter()
    {
        Debug.Log("Playing");
        base.Enter();
    }

    protected override void Update()
    {
        InputController();
    }

    protected override void Exit()
    {
        base.Exit();
    }

    public override void InputController()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            nextMode = new Building(player, controllerCamera, tiles, structures, cameraSpeed, cameraDirX, cameraDirY);
            stage = EVENT.EXIT;
        }

        player.inputVector.x = Input.GetAxisRaw("Horizontal");
        player.inputVector.y = Input.GetAxisRaw("Vertical");
    }
}

public class Building : Mode
{
    private bool isConstructionSelected = true;

    private Item item;
    /// <summary>
    /// /////////////////////////////// item
    /// </summary>
    /// <param name="player"></param>
    /// <param name="controllerCamera"></param>
    /// <param name="tiles"></param>
    /// <param name="cameraSpeed"></param>
    /// <param name="cameraDirX"></param>
    /// <param name="cameraDirY"></param>


    public Building(Unit player, GameObject controllerCamera, TileController tiles, StructuresData structures, float cameraSpeed, float cameraDirX,
        float cameraDirY) : base(player, controllerCamera, tiles, structures, cameraSpeed, cameraDirX, cameraDirY)
    {
        this.structures = structures;
        this.tiles = tiles;
        this.cameraSpeed = cameraSpeed;
        this.cameraDirX = cameraDirX;
        this.cameraDirY = cameraDirY;
        this.controllerCamera = controllerCamera;
        this.name = Name.BUILDING;
        this.player = player;
    }

    protected override void Enter()
    {
        Debug.Log("Building");

        base.Enter();
    }

    protected override void Update()
    {
        InputController();
    }

    protected override void Exit()
    {
        controllerCamera.transform.parent = player.transform;
        controllerCamera.transform.localPosition = Vector3.zero;
        base.Exit();
    }

    public override void InputController()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            nextMode = new Playing(player, controllerCamera, tiles, structures, cameraSpeed, cameraDirX, cameraDirY);
            stage = EVENT.EXIT;
        }

        Vector2 input;
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        MoveCamera(input);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            item = structures.Items[0];
            Build(GetMouseOnGridPosition(),item);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            Destroy(GetMouseOnGridPosition());
        }
    }
    
    private void Build(Vector3Int position, Item item2build)
    {
        if (!tiles.buildings.HasTile(position)&& !tiles.environment.HasTile(position))
        {
            tiles.buildings.SetTile(position,item2build.tile);
        }
        
    }

    private void Destroy(Vector3Int position)
    {
        if (tiles.buildings.HasTile(position) )
        {
            TileRuleCustom tile = tiles.buildings.GetTile<TileRuleCustom>(position);
            tiles.buildings.SetTile(position,null);
            
            Debug.Log($"{tile.item.name}");
            //
        }
    }

    private void MoveCamera(Vector2 dir)
    {
        Vector2 directionToPlayer = player.transform.position - controllerCamera.transform.position;
        Vector2 prepare = dir * cameraSpeed * Time.fixedDeltaTime;
        float posX = controllerCamera.transform.localPosition.x + prepare.x;
        float posY = controllerCamera.transform.localPosition.y + prepare.y;
        if (Mathf.Abs(posX) > cameraDirX)
        {
            posX = cameraDirX * Mathf.Sign(posX);
        }

        if (Mathf.Abs(posY) > cameraDirY)
        {
            posY = cameraDirY * Mathf.Sign(posY);
        }

        controllerCamera.transform.localPosition = new Vector3(posX, posY);
    }
}