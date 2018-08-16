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

        int col = (int)(CMouse.getX() / CTileMap.TILE_WIDTH);
        int row = (int)(CMouse.getY() / CTileMap.TILE_HEIGHT);
        CTile tile = CTileMap.Instance.getTile(col, row);

        CTile downTile = CTileMap.Instance.getTile((int)tile.getX() / CTileMap.TILE_WIDTH, (int)(tile.getY() + CTileMap.TILE_HEIGHT) / CTileMap.TILE_HEIGHT);
        CTile upTile = CTileMap.Instance.getTile((int)tile.getX() / CTileMap.TILE_WIDTH, (int)(tile.getY() - CTileMap.TILE_HEIGHT) / CTileMap.TILE_HEIGHT);

        if (tile != null)
        {
            if (CMouse.firstPress(CMouse.BUTTONS.LEFT) && tile.getTileType() == CTile.Type.AIR && downTile.getTileType() != CTile.Type.AIR)
            {
				if (counter < 3)
				{
                    CTileMap.Instance.changeTile(tile, CTile.Type.EARTH);
					counter++;
				}
				else
				{
					Debug.Log("YA HAY " + (counter + 1)  + " TIERRA");
				}
            }
			else if (CMouse.firstPress(CMouse.BUTTONS.RIGHT) && tile.getTileType() == CTile.Type.EARTH && upTile.getTileType() == CTile.Type.AIR)
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
