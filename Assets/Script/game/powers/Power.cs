using UnityEngine;
using System.Collections;

public class Power
{
    protected string name;

    virtual public void update() { }

    virtual public void render() { }

    public string getName()
    {
        return name;
    }

    public void destroy()
    {
        this.name = null;
    }
}
