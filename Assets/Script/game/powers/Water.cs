using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Water : Power
{
    private int counter = 0;

    public Water()
    {
        this.name = "Water";
    }

    override public void update()
    {
        base.update();

        //Pone las columnas para poneer los tiles
        int col = (int)(CMouse.getX() / CTileMap.TILE_WIDTH);
        int row = (int)(CMouse.getY() / CTileMap.TILE_HEIGHT);
        CTile tile = CTileMap.Instance.getTile(col, row);
        //Verifica los tiles de arrriba y abajo 

        CTile downTile = CTileMap.Instance.getTile((int)tile.getX() / CTileMap.TILE_WIDTH, (int)(tile.getY() + CTileMap.TILE_HEIGHT) / CTileMap.TILE_HEIGHT);
        CTile upTile = CTileMap.Instance.getTile((int)tile.getX() / CTileMap.TILE_WIDTH, (int)(tile.getY() - CTileMap.TILE_HEIGHT) / CTileMap.TILE_HEIGHT);

        if (tile != null)
        {
            if (CMouse.firstPress(CMouse.BUTTONS.LEFT) && tile.getTileType() == CTile.Type.WATER && downTile.getTileType() != CTile.Type.WATER)
            {
                if (counter < 1)
                {
                    CTileMap.Instance.changeTile(tile, CTile.Type.ICE);
                    counter++;
                }
                else
                {
                    Debug.Log("YA HAY " + (counter + 1) + " Agua");
                }
            }
            else if (CMouse.firstPress(CMouse.BUTTONS.RIGHT) && tile.getTileType() == CTile.Type.ICE && upTile.getTileType() == CTile.Type.AIR)
            {
                CTileMap.Instance.changeTile(tile, CTile.Type.WATER);
                counter--;
            }
        }





    }

    override public void render()
    {
        base.render();
    }

    public void destroy()
    {
        base.destroy();
    }



   
}
