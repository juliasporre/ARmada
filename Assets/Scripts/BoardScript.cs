using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


/*TODO
 *
 * 
 * 
 */

public class BoardScript : MonoBehaviour {

    Dictionary<string, List<string>> boatsPos = new Dictionary<string, List<string>>();
    Dictionary<string, List<string>> OpponentsBoatsPos = new Dictionary<string, List<string>>();
    string player; //This player

    public GameObject userDisplay;
    public GameObject fireworks;
    public Rigidbody rocket;

	public GameObject kryss;
	public GameObject ring;

    public GameObject birdaboat;
    public GameObject blackperl;
    public GameObject speedyboat;
    public GameObject supersail;
    public GameObject tinyboat;
    public GameObject whitefang;




    //public string url = "http://130.229.175.61:8000/game"; //use this if other computer is server 
    public string url = "http://localhost:8000/game";
    private IEnumerator printMessage;
    int t = 0; //Begin the game, waiting for command 0

    void Start()
    {
        StartCoroutine(SendPosBoat(20f)); //when starting the game, wait 20 sec to position the boats on the board
        //InvokeRepeating("callHelper",0.5f,0.5f); //many calls
        InvokeRepeating("callHelper", 0.5f, 5f); //does not need to handle player, client does


    }

    void callHelper()
    { //only surves to starts getMessage
		if (player != null) //Dont ask for moves until player has been assigned in SendPosBoat
		{
       	 	printMessage = getMessage();
        	StartCoroutine(printMessage);
		}
    }

    IEnumerator getMessage()
    {
		WWW www = new WWW(url + "?t=" + t);
        yield return www;

        if (www.text.Length > 0)
        {
            t++;

            string recivedCommands = www.text;
            Debug.Log(recivedCommands);
            string[] commands = recivedCommands.Split(' '); //recieved commands are seperated by " "
           
            if(commands.Length > 1)
            {
                makeMove(commands);
            }
        }
    }

    void makeMove(string[] commands)
    {
        if (commands[1] == player && commands[2] == "WIN")
        {
            win();
        }
        else if (commands[1] != player && commands[2] == "WIN")
        {
            loss();
        }
        //If the game has not yet reached end state: commands[4] should be name of opponents boat
        else if (commands.Length > 3 && commands[1] == player && commands[3] == "x") //x = hit
        {
            bombOpponentHit("o" + commands[2]);
            if (OpponentsBoatsPos.ContainsKey(commands[4]))
            {
                List<string> list = OpponentsBoatsPos[commands[4]];
                list.Add("o" + commands[2]);
            }
            else if (OpponentsBoatsPos.ContainsKey(commands[4]) == false)
            {
                List<string> list = new List<string>();
                list.Add("o" + commands[2]);
                OpponentsBoatsPos.Add(commands[4], list);
            }
        }
        else if (commands.Length > 3 && commands[1] == player && commands[3] == "o") //o = miss
        {
            bombOpponentMiss("o" + commands[2]);
        }
        else if (commands.Length > 3 && commands[1] == player && commands[3] == "X") //X = sunk
        {
            if (OpponentsBoatsPos.ContainsKey(commands[4]))
            {
                List<string> list = OpponentsBoatsPos[commands[4]];
                list.Add("o" + commands[2]);
            }
            else if (OpponentsBoatsPos.ContainsKey(commands[4]) == false)
            {
                List<string> list = new List<string>();
                list.Add("o" + commands[2]);
                OpponentsBoatsPos.Add(commands[4], list);
            }
            bombOpponentSunk("o" + commands[2], commands[4]);
        }

        //if turn is opponent
        else if (commands.Length > 3 && commands[1] != player && commands[3] == "x") //x = hit
        {
            bombMeHit(commands[2]);
        }
        else if (commands.Length > 3 && commands[1] != player && commands[3] == "o") //o = miss
        {
            bombMeMiss(commands[2]);
        }
        else if (commands.Length > 3 && commands[1] != player && commands[3] == "X") //X = sunk
        {
            bombMeSunk(commands[2]);
        }
    }

    void bombOpponentHit(string bombPos)
    {
        Debug.Log("Your attack hit!");
		fireMissle(bombPos);
		markHit (bombPos);
    }

    void bombOpponentMiss(string bombPos)
    {
        Debug.Log("Your attack missed.");
		fireMissle(bombPos);
		markMiss(bombPos);
	}


    void bombMeHit(string bombPos)
    {
        Debug.Log("Your boat was hit!");
		fireMissle(bombPos);
    }

    void bombMeMiss(string bombPos)
    {
        Debug.Log("Your boats was not hit.");
		fireMissle(bombPos);
    }

    void bombOpponentSunk(string bombPos, string boat)
    {
        Debug.Log("You sank the opponents boat!");
        fireMissle(bombPos);
        markHit(bombPos);

        //Calculating placement of the ship
        List<string> posList = OpponentsBoatsPos[boat];
        Vector3 middlepoint = new Vector3(0, 0, 0);
        foreach (string position in posList)
        {
            middlepoint += GameObject.Find(position).transform.position;
        }
        middlepoint = middlepoint / posList.Count;
        Debug.Log(boat);
        //Möjligt att detta inte fungerar, ändra så att det blir som explosion, missile osv.
        GameObject opponentsSinkingBoat = Instantiate(GameObject.Find(boat), middlepoint, transform.rotation);
        opponentsSinkingBoat.transform.SetParent(GameObject.Find(bombPos).transform, false);

        sinkingBoat(bombPos);

        //Removes boxcolliders, O and X from the game board
        foreach(string position in posList)
        {
            Destroy(GameObject.Find(position));
        }
    }

    void bombMeSunk(string bombPos)
	{
		Debug.Log("Your boat was sunk.");
		fireMissle(bombPos);
        sinkingBoat(bombPos);
	}

    void sinkingBoat(string boatPos)
    {
        foreach (string boat in boatsPos.Keys)
        {
            Debug.Log(boat);
            List<string> posList = boatsPos[boat];
            foreach (string position in posList)
            {
                if(position == boatPos)
                {
                    var sunkBoat = GameObject.Find(boat);
                    Debug.Log(sunkBoat);
                    for(int i = 0; i < 50; i++)
                    {
                        sunkBoat.transform.Rotate(Vector3.forward, 2);
                        sunkBoat.transform.position += Vector3.down;
                    }
					/*Rotation med tid
					 * Transform from = sunkBoat.transform;
					Transform to = -from;

					sunkBoat.transform.rotation = Quaternion.Slerp (from.rotation, to.rotation, Time.time * 5f);
					*/
                }
            }
        }
    }

	void markHit (string bombPos)
	{ //Places an O where boat was hit on opponents side
      //Vector3 place = GameObject.Find(bombPos).transform.position;
        Vector3 place = new Vector3(0, 0, 0);

        GameObject success = Instantiate(ring, place,transform.rotation);
        success.transform.SetParent(GameObject.Find(bombPos).transform, false);
    }

	void markMiss(string bombPos)
	{//Places an X where boat was hit on opponents side
		//Vector3 place = GameObject.Find(bombPos).transform.position;
        Vector3 place = new Vector3(0, 0, 0);
        GameObject fail = Instantiate(kryss, place,transform.rotation);
        fail.transform.SetParent(GameObject.Find(bombPos).transform, false);
    }
		
	void win()
	{
        userDisplay.GetComponent<Text>().text = "WINNER :D";
        fireworks.GetComponent<Renderer>().enabled = true;
        Debug.Log("YOU WON");
	}

	void loss()
	{
		Debug.Log("You lose");
        userDisplay.GetComponent<Text>().text = "YOU LOST";
    }

	void fireMissle(string boxName)
	//creates a missle above the box "A1,A2..." that was sent by command
	{
        Vector3 place = GameObject.Find(boxName).transform.position;
        Rigidbody rocketClone = (Rigidbody)Instantiate(rocket, place, transform.rotation * Quaternion.Euler(90, 0, 0));
        rocketClone.transform.parent = transform; //fäster raketen på board

        rocketClone.transform.position += transform.up * 40f; //vrider raketen med huvet ner

        rocketClone.velocity = -transform.up * 5f; //hastighet nedåt
    }
    void Update()
    {

        if (Input.GetKeyDown("space"))
       {
            //Test code, works with space
            Debug.Log("TEST BOMBING");
            Vector3 place = GameObject.Find("A1").transform.position;
			Rigidbody rocketClone = (Rigidbody)Instantiate(rocket, place, transform.rotation*Quaternion.Euler(90,0,0));
			rocketClone.transform.parent = transform; //fäster raketen på board

			rocketClone.transform.position += transform.up * 40f; //vrider raketen med huvet ner

			rocketClone.velocity = -transform.up * 5f; //hastighet nedåt

            sinkingBoat("A1");

			markHit ("A1");

            }
    }

    public void OnChildsTriggerEnter(string name, Collider other) // Other = boat
    {
        Debug.Log("Placed a boat named: " + other.name);
        if (boatsPos.ContainsKey(other.name))
        {
            List<string> list = boatsPos[other.name];
            if (list.Contains(name) == false)
            {
                list.Add(name);
            }
        }
        else
        {
            List<string> list = new List<string>();
            list.Add(name);
            boatsPos.Add(other.name, list);
        }
    }


    public void OnChildsTriggerExit(string name, Collider other)
    {
        Debug.Log("Removed " + other.name + " from " + name);
        if (boatsPos.ContainsKey(other.name))
        {
            List<string> list = boatsPos[other.name];
            if (list.Contains(name) && list.Count > 1)
            {
                list.Remove(name);
            }
            else if (list.Contains(name) && list.Count == 1)
            {
                list.Remove(other.name);
            }
        }
    }


    IEnumerator SendPosBoat(float time)
    {
		//Gets the position of each boat
        yield return new WaitForSeconds(time);
        string listToSend = "";

        foreach (string boat in boatsPos.Keys)
        {
            listToSend += boat + "_";
            List<string> posList = boatsPos[boat];
            foreach (string position in posList)
            {
                listToSend +=  position;
            }

            listToSend += "_";
        }
        listToSend = listToSend.Remove(listToSend.Length - 1);
        //listToSend = "blackperl_A1B1xtitanic_D2D3D4"; //position of ship for testing

        string urlBoats = url + "?init=" + listToSend;
        Debug.Log("Sending " + urlBoats);
        WWW www = new WWW(urlBoats);
		yield return www;

        player = www.text;
        Debug.Log("You are now " + player);

        var allColliders = GetComponentsInChildren<Collider>();
        foreach (var childCollider in allColliders)
        {
            Destroy(childCollider);
        }
    }
}

