using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField]private TileController tiles;
    [SerializeField] private Item item;

    private Vector3Int GetMouseOnGridPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mouseCellPos = tiles.buildings.WorldToCell(mousePos);
        mouseCellPos.z = 0;

        return mouseCellPos;
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
            tiles.buildings.SetTile(position,null);
        }
    }

    private void Update()
    {
        InputComand();
    }

    private void InputComand()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            
            Build(GetMouseOnGridPosition(),item);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            
           Destroy(GetMouseOnGridPosition());
        }

       
    }


}
