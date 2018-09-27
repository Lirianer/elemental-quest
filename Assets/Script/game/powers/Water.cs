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
        int col = (int)(CMouse.getX() / CTileMap.Instance.getTileWidth());
        int row = (int)(CMouse.getY() / CTileMap.Instance.getTileHeight());
        CTile tile = CTileMap.Instance.getTile(col, row);
        //Verifica los tiles de arrriba y abajo 

        CTile downTile = CTileMap.Instance.getTile((int)tile.getX() / CTileMap.Instance.getTileWidth(), (int)(tile.getY() + CTileMap.Instance.getTileHeight()) / CTileMap.Instance.getTileHeight());
        CTile upTile = CTileMap.Instance.getTile((int)tile.getX() / CTileMap.Instance.getTileWidth(), (int)(tile.getY() - CTileMap.Instance.getTileHeight()) / CTileMap.Instance.getTileHeight());

        if (tile != null)
        {
            if (CMouse.firstPress(CMouse.BUTTONS.LEFT) && tile.getTileType() == CTile.Type.WATER)
            {
                if (counter < 1)
                {
                    int iceImage = 16;

                    if(upTile.getTileType() != CTile.Type.WATER)
                        iceImage = 15;

                    CTileMap.Instance.changeTile(tile, CTile.Type.ICE, iceImage);
                    counter++;
                }
                else
                {
                    Debug.Log("YA HAY " + (counter + 1) + " Agua");
                }
            }
            else if (CMouse.firstPress(CMouse.BUTTONS.RIGHT) && tile.getTileType() == CTile.Type.ICE)
            {
                int waterImage = 9;

                if(upTile.getTileType() != CTile.Type.WATER)
                    waterImage = 17;

                CTileMap.Instance.changeTile(tile, CTile.Type.WATER, waterImage);
                counter--;
            }
            else if (CMouse.firstPress(CMouse.BUTTONS.RIGHT))
            {
                Debug.Log(tile.getTileType());
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
