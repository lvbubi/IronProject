using UnityEngine;
using System.Collections;
using System.Collections.Generic;
class BodyPart
{
    string id;
    int energyCost;
    bool durability;
    public
    BodyPart(string id,int energyCost)
    {
        durability = true;
        this.id = id;
        this.energyCost = energyCost;
    }
    public int Start()
    {
        return energyCost;
    }
    public bool CheckPart()
    {
        if (!durability)
            throw new System.ArgumentException(id+" is broken", "original");
        int number = Random.Range(0, 1000);
        if (number == 1)
            durability = false;
        return true;
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
        CheckPart();
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
        CheckPart();
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
        CheckPart();
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
        CheckPart();
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
        BodyParts[0].CheckPart();
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

public class Controller : MonoBehaviour
{
    IronMan ironman = new IronMan();
    private float nextActionTime = 0f;
    public float period = 5;
    bool fly = false;
    bool leftArmShoot = false;
    bool rightArmShoot = false;
    bool chestShoot = false;
    // Use this for initialization
    void Start()
    {
        try
        {
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
        catch (System.ArgumentException ex)
        {
            Debug.Log(ex.Message);
        }
    }
    void UseFunctions()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            if (fly)
            {
                try
                {
                    ironman.Fly();
                    GetComponent<Animator>().SetBool("Fly",true);
                }
                catch (System.ArgumentException ex)
                {
                    Debug.Log(ex.Message+" Kényszerleszállás");
                }
                Debug.Log(ironman.getEneryLevel());

            }
        }
        if (chestShoot)
            {
                try
                {
                    chestShoot = !chestShoot;
                    ironman.chestShoot();
                    GetComponent<Animator>().SetTrigger("ChestShoot");
                }
                catch (System.ArgumentException ex)
                {
                    Debug.Log(ex.Message + " ChestShoot is not working");
                }
            }
        if (leftArmShoot)
        {
            try
            {
                leftArmShoot = !leftArmShoot;
                ironman.leftArmShoot();
                GetComponent<Animator>().SetTrigger("LeftArmShoot");
            }
            catch (System.ArgumentException ex)
            {
                Debug.Log(ex.Message + " LeftArmShoot is not working");
            }

        }
        if (rightArmShoot)
        {
            try
            {
                rightArmShoot = !rightArmShoot;
                ironman.rightArmShoot();
                GetComponent<Animator>().SetTrigger("RightArmShoot");
            }
            catch (System.ArgumentException ex)
            {
                Debug.Log(ex.Message + " RightArmShoot is not working");
            }

        }
        
    }
    // Update is called once per frame
    void Update()
    {
        UseFunctions();

        if (ironman.getEneryLevel() > 10)
        {
            if (Input.GetKeyDown(KeyCode.F))//On keydown
            {
                fly = !fly;
                if (GetComponent<Animator>().GetBool("Fly") == true)
                    GetComponent<Animator>().SetBool("Fly", false);
            }
            if (Input.GetKeyDown(KeyCode.C))//On keydown
            {
                chestShoot = true;
            }
        }
        else if (ironman.getEneryLevel()<=0)
        {
            leftArmShoot = false;
            rightArmShoot = false;
            chestShoot = false;
            fly = false;
            Debug.Log("Ironman ShutDown");
        }

        else if (fly || ironman.getEneryLevel()<=10)
        {
            Debug.Log("Alacson energiszaint -> Energiatakarékos");
            period = 20;
            leftArmShoot = false;
            rightArmShoot = false;
            chestShoot = false;
            if (Input.GetKeyDown(KeyCode.F))
            {
                fly = !fly;
                GetComponent<Animator>().SetBool("Fly", (!GetComponent<Animator>().GetBool("Fly")));

            }
        }

        GetComponent<Animator>().SetInteger("Energy", ironman.getEneryLevel());
    }
}
