using UnityEngine;
using System.Collections;

public class Power
{
    protected string name;

    protected float leftPowerBaseCooldown = 0f;
    protected float rightPowerBaseCooldown = 0f;
    
    protected float leftPowerElapsedCooldown = 1f;
    protected float rightPowerElapsedCooldown = 1f;

    protected bool isActivePower = false;

    virtual public void update() 
    { 
        //Chequear por click y activar poder dependiendo del click.
        //Si se activo uno de los poderes entonces hay que empezar a correr el cooldown

        if(CMouse.firstPress(CMouse.BUTTONS.LEFT) && isActivePower && leftPowerElapsedCooldown >= leftPowerBaseCooldown)
        {
            this.leftClickPower();

            leftPowerElapsedCooldown = 0f;
        }
        else if (CMouse.firstPress(CMouse.BUTTONS.RIGHT) && isActivePower && rightPowerElapsedCooldown >= rightPowerBaseCooldown)
        {
            this.rightClickPower();

            rightPowerElapsedCooldown = 0f;
        }

        if(leftPowerElapsedCooldown < leftPowerBaseCooldown)
        {
            leftPowerElapsedCooldown += Time.deltaTime;
        }

        if(rightPowerElapsedCooldown < rightPowerBaseCooldown)
        {
            rightPowerElapsedCooldown += Time.deltaTime;
        }
    }

    virtual public void render() { }

    public string getName()
    {
        return name;
    }

    virtual protected void leftClickPower() 
    {
         Debug.Log(this.name + ": Unimplemented LEFT click");
    }

    virtual protected void rightClickPower() 
    {
         Debug.Log(this.name + ": Unimplemented RIGHT click");
    }

    virtual public void destroy()
    {
        this.name = null;
    }

    public void setActive()
    {
        this.isActivePower = true;
    }

    public void setInactive()
    {
        this.isActivePower = false;
    }
}
