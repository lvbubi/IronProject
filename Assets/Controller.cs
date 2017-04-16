﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


class BodyPart
{
    string id;
    int energyCost;

    public
    BodyPart(string id,int energyCost)
    {
        this.id = id;
        this.energyCost = energyCost;
    }
    public int Start()
    {
        return energyCost;
    }
    public virtual int useFunction()
    {
        return 0;
    }
    public virtual int Flying()
    {
        return 0;
    }
    public virtual int Shoot()
    {
        return 0;
    }


    public string getName()
    {
        return id;
    }
}

class Leg :  BodyPart
{
    public Leg(string id, int energyCost):base(id,energyCost)
    {
       
    }
    public override int useFunction()
    {
        return 2;

    }
}
class Head : BodyPart
{
    public Head(string id, int energyCost) : base(id, energyCost)
    {

    }
    public override int useFunction()
    {
        return 1;
    }
}

class Chest : BodyPart
{
    public Chest(string id, int energyCost) : base(id, energyCost)
    {

    }
    public override int useFunction()
    {
        return 1;
    }
}

class Arm : BodyPart
{
    public Arm(string id, int energyCost) : base(id, energyCost)
    {

    }
    public override int useFunction()
    {
        return 2;
    }
}

class IronMan
{
    List<BodyPart> BodyParts = new List<BodyPart>();
    int energylevel;

    public IronMan(){
        BodyParts.Add(new Leg("LeftLeg", 1));
        BodyParts.Add(new Leg("RightLeg", 1));
        BodyParts.Add(new Chest("Chest", 2));
        BodyParts.Add(new Head("Head", 1));
        BodyParts.Add(new Arm("LeftArm", 1));
        BodyParts.Add(new Arm("RightArm", 1));
        energylevel = 100;
    }
    public void Fly()
    {
        Moove();
        int energyCost = 0;
        foreach (BodyPart bodypart in BodyParts)
            if (bodypart.getName().Equals("LeftLeg") || bodypart.getName().Equals("RightLeg")
                || bodypart.getName().Equals("LeftArm") || bodypart.getName().Equals("RightArm"))
                energyCost += bodypart.useFunction();

        energylevel -= energyCost;
    }

    public void Moove()
    {
        int energyCost = 0;
        foreach (BodyPart bodypart in BodyParts)
            energyCost += bodypart.Start();
        energylevel -= energyCost;
    }

    public void leftArmShoot()
    {
        int energyCost = 0;
        foreach (BodyPart bodypart in BodyParts)
            if ( bodypart.getName().Equals("LeftArm"))
                energyCost += bodypart.useFunction();
        energylevel -= energyCost;
    }

    public void rightArmShoot()
    {
        int energyCost = 0;
        foreach (BodyPart bodypart in BodyParts)
            if (bodypart.getName().Equals("Chest"))
                energyCost += bodypart.useFunction();
        energylevel -= energyCost;
    }
    public void chestShoot()
    {
        int energyCost = 0;
        foreach (BodyPart bodypart in BodyParts)
            if (bodypart.getName().Equals("RightArm"))
                energyCost += bodypart.useFunction();
        energylevel -= energyCost;
    }
    public int getEneryLevel()
    {
        return energylevel;
    }
}

public class Controller : MonoBehaviour {
    IronMan ironman=new IronMan();
	// Use this for initialization
	void Start () {
        Debug.Log(ironman.getEneryLevel());
        ironman.Moove();
        Debug.Log(ironman.getEneryLevel());
        ironman.Fly();
        Debug.Log(ironman.getEneryLevel());
        ironman.leftArmShoot();
        Debug.Log(ironman.getEneryLevel());
        ironman.chestShoot();
        Debug.Log(ironman.getEneryLevel());
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
