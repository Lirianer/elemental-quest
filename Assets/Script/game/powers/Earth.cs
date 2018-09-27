using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Earth : Power
{
    private int counter = 0;

    public Earth()
    {
        this.name = "Tierra";
    }

    override public void update()
    {
        base.update();

        int col = (int)(CMouse.getX() / CTileMap.Instance.getTileWidth());
        int row = (int)(CMouse.getY() / CTileMap.Instance.getTileHeight());
        CTile tile = CTileMap.Instance.getTile(col, row);

        CTile downTile = CTileMap.Instance.getTile((int)tile.getX() / CTileMap.Instance.getTileWidth(), (int)(tile.getY() + CTileMap.Instance.getTileHeight()) / CTileMap.Instance.getTileHeight());
        CTile upTile = CTileMap.Instance.getTile((int)tile.getX() / CTileMap.Instance.getTileWidth(), (int)(tile.getY() - CTileMap.Instance.getTileHeight()) / CTileMap.Instance.getTileHeight());

        if (tile != null)
        {
            if (CMouse.firstPress(CMouse.BUTTONS.LEFT) && tile.getTileType() == CTile.Type.AIR && (downTile.getTileType() == CTile.Type.MUD || downTile.getTileType() == CTile.Type.ARTIFICIAL_EARTH))
            {
				if (counter < 3)
				{
                    CTileMap.Instance.changeTile(tile, CTile.Type.ARTIFICIAL_EARTH, 7);
					counter++;
				}
				else
				{
					Debug.Log("YA HAY " + (counter + 1)  + " TIERRA");
				}
            }
			else if (CMouse.firstPress(CMouse.BUTTONS.RIGHT) && tile.getTileType() == CTile.Type.ARTIFICIAL_EARTH && upTile.getTileType() != CTile.Type.ARTIFICIAL_EARTH)
			{
				CTileMap.Instance.changeTile(tile, CTile.Type.AIR);
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
