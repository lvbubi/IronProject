using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
            throw new System.ArgumentException(id+" is broken");
        int number = Random.Range(0, 10);
        if( id== "LeftLeg" || id == "RightLeg")
            number = Random.Range(0, 20);
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
    public GameObject Cam;
    IronMan ironman = new IronMan();
    private float nextActionTime = 0f;
    public float period = 5;
    public Text EnergyText;
    public Text ErrorText;
    private int energy;
    bool fly = false;
    bool leftArmShoot = false;
    bool rightArmShoot = false;
    bool chestShoot = false;
    bool flyBroken = false;
	//for monving
	public float speed;
    public float MovementSpeed;
	private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        energy = ironman.getEneryLevel();
		rb = GetComponent<Rigidbody>();

    }

	void Moving (){
        if (Input.GetKey(KeyCode.W)) {
            transform.position += transform.forward * Time.deltaTime * MovementSpeed;
            if (!fly)
                GetComponent<Animator>().SetBool("Walk", true);

            }
        if (Input.GetKeyUp(KeyCode.W))
            GetComponent<Animator>().SetBool("Walk", false);

        if (Input.GetKey(KeyCode.Space) && fly && !flyBroken)
        {
            Vector3 movement = new Vector3(0, 5.0f, 0);

            rb.AddForce(movement * MovementSpeed);
        }
        if (Input.GetKey(KeyCode.S) && fly)
        {
            Vector3 movement = new Vector3(0, -1f, 0);

            rb.AddForce(movement * MovementSpeed);
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
                    fly = false;
                    flyBroken = true;
                    ErrorText.text = ex.Message + " Kényszerleszállás";
                    Debug.Log(ex.Message+" Kényszerleszállás");
                }

            }
            if (chestShoot && !fly)
            {
                try
                {
                    chestShoot = !chestShoot;
                    ironman.chestShoot();
                    GetComponent<Animator>().SetTrigger("ChestShoot");
                }
                catch (System.ArgumentException ex)
                {
                    ErrorText.text = ex.Message + " ChestShoot is not working";
                    Debug.Log(ex.Message + " ChestShoot is not working");
                }
            }
            if (leftArmShoot && !fly)
            {
                try
                {
                    leftArmShoot = !leftArmShoot;
                    ironman.leftArmShoot();
                    GetComponent<Animator>().SetTrigger("LeftArmShoot");
                }
                catch (System.ArgumentException ex)
                {
                    ErrorText.text = ex.Message + " LeftArmShoot is not working";
                    Debug.Log(ex.Message + " LeftArmShoot is not working");
                }

            }
            if (rightArmShoot && !fly)
            {
                try
                {
                    rightArmShoot = !rightArmShoot;
                    ironman.rightArmShoot();
                    GetComponent<Animator>().SetTrigger("RightArmShoot");
                }
                catch (System.ArgumentException ex)
                {
                    ErrorText.text = ex.Message + " RightArmShoot is not working";
                    Debug.Log(ex.Message + " RightArmShoot is not working");
                }

            }
        }

        
    }


    // Update is called once per frame
    void Update()
    {

        if (energy > 0)
        {
            if (Input.GetMouseButton(1))
            {
                var CharacterRotation = Cam.transform.rotation;
                CharacterRotation.x = 0;
                CharacterRotation.z = 0;

                transform.rotation = CharacterRotation;

            }
            UseFunctions();
            Moving();
        }




        if (energy > 10)
        {
            if (Input.GetKeyDown(KeyCode.F))//On keydown
            {
                fly = !fly;
                if (GetComponent<Animator>().GetBool("Fly") == true)
                    GetComponent<Animator>().SetBool("Fly", false);
                else
                    GetComponent<Animator>().SetBool("Fly", true);
            }
            if (Input.GetMouseButtonDown(2))//On keydown
                chestShoot = true;
            
            if (Input.GetKeyDown(KeyCode.Q))
                leftArmShoot = true;

            if (Input.GetKeyDown(KeyCode.E))
                rightArmShoot = true;
        }
        else if (energy <= 0)
        {
            leftArmShoot = false;
            rightArmShoot = false;
            chestShoot = false;
            fly = false;
            GetComponent<Animator>().SetBool("Fly", false);
            ErrorText.text = "Ironman ShutDown";
            Debug.Log("Ironman ShutDown");
        }

        else if (fly || energy <= 10)
        {
            ErrorText.text = "Alacson energiszaint -> Energiatakarékos";
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
        energy = ironman.getEneryLevel();
        if (energy < 0)
            energy = 0;
        GetComponent<Animator>().SetInteger("Energy", energy);
        EnergyText.text = "Energy: " + energy;

        if (flyBroken && transform.position.y < 10)
        {
            GetComponent<Animator>().SetBool("Fly", false);
        }
    }



}
