using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


/*TODO
 * Ta bort boxcolliders efter det att positionerna är skickade
 * Fylla i vad som händer när man får tillbaka attack
 * 
 */

public class BoardScript : MonoBehaviour {

    Dictionary<string, List<string>> boatsPos = new Dictionary<string, List<string>>();
    Dictionary<string, Vector3> BoxesPos = new Dictionary<string, Vector3>();
    string player; //This player

    public GameObject originalbomb;
    public GameObject userDisplay;
    public GameObject fireworks;
    public Rigidbody rocket;
    public float speed = 10f;

    public string url = "http://130.229.174.226:8000/game"; //use this if other computer is server 
    //public string url = "http://localhost:8000/game";
	// hårdkodat atm, får kolla på detta senare
    private IEnumerator printMessage;
    int t = 0; //Begin the game, waiting for command 0

    void Start()
    {
        StartCoroutine(SendPosBoat(30)); //when starting the game, wait 20 sec to position the boats on the board
        //InvokeRepeating("callHelper",0.5f,0.5f); //many calls
        InvokeRepeating("callHelper", 0.5f, 5f); //does not need to handle player, client does
    }

    void callHelper()
    { //only surves to starts getMessage
		if (player != null) //Dont ask for moves util player has been assigned in SendPosBoat
		{
       	 	printMessage = getMessage();
        	StartCoroutine(printMessage);
		}
    }

    IEnumerator getMessage()
    {
		//Debug.Log("http://130.229.174.226:8000/game" + "?t=" + t);
		WWW www = new WWW(url + "?t=" + t);
        //Debug.Log("Getting message from server: ");
        yield return www;
		//Debug.Log(www.text);
	
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
		if (commands[1] == player && commands[2] == "WIN") //X=sunk
		{
			win();
			//Display a text that says "You are the champion"
		}
		else if (commands[1] != player && commands[2] == "WIN") //X=sunk
		{
			loss();
			//Display a text that says "You have lost"
		}

		//If the game has not yet reached end state:
        else if(commands[1] == player && commands[3] == "x") //x= hit
        {
            bombOpponentHit(commands[2]);
			//display an mark of hit at position commands[3] side of opponent
			//Vector3 place = GameObject.Find("o" + commands[3]).transform.position;
			//Object success = Instantiate(SuccessMark, place, transform.rotation);

        }
        else if (commands[1] == player && commands[3] == "o") //o = miss
        {
            bombOpponentMiss(commands[2]);
			//display an mark of miss at position commands[3]
			//Vector3 place = GameObject.Find("o" + commands[3]).transform.position;
			//Object fail = Instantiate(MissMark, place, transform.rotation);
        }
		else if (commands[1] == player && commands[3] == "X") //X=sunk
		{
			bombOpponentSunk(commands[2]);
			//display the boat that was sunk
			//Vector3 place = GameObject.Find("o" + commands[3]).transform.position;
			//instantiate a sunk boat at its original position. Or instanstiate at the beginning and make it visible here?
			//Object bomb = Instantiate(TheBoatThatWasSunk(boatsPos[commands[3]], place, originalbomb.transform.rotation);
		}

		//if turn is opponent
        else if (commands[1] != player && commands[3] == "x") //x= hit
        {
            bombMeHit(commands[2]);

			//Vector3 place = GameObject.Find(commands[3]).transform.position;
        }
        else if (commands[1] != player && commands[3] == "o") //o=miss
        {
            bombMeMiss(commands[2]);
			//Vector3 place = GameObject.Find(commands[3]).transform.position;


        }
		else if (commands[1] != player && commands[3] == "X") //X=sunk
		{
			bombMeSunk(commands[2]);
		}
	
    }

    void bombOpponentHit(string bombPos)
    {
        Debug.Log("Your attack hit!");
		fireMissle("o" + bombPos);

    }

    void bombOpponentMiss(string bombPos)
    {
        Debug.Log("Your attack missed.");
		fireMissle("o"+bombPos);

	}
	void bombOpponentSunk(string bombPos)
	{
		Debug.Log ("You sank the opponents boat!");
		fireMissle("o"+bombPos);

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

	void bombMeSunk(string bombPos)
	{
		Debug.Log("Your boat was sunk.");
		fireMissle(bombPos);
        sinkingBoat(bombPos);
	}


    void sinkingBoat(string boatPos)
    {
        Debug.Log("NU SJUNKER DEN");
        foreach (string boat in boatsPos.Keys)
        {
            Debug.Log(boat);
            List<string> posList = boatsPos[boat];
            foreach (string position in posList)
            {
                if(position == boatPos)
                {
                    Debug.Log("FOUND THE BOAT");
                    var sunkBoat = GameObject.Find(boat);
                    Debug.Log(sunkBoat);
                    for(int i = 0; i < 50; i++)
                    {
                        sunkBoat.transform.Rotate(Vector3.forward, 2);
                        sunkBoat.transform.position += Vector3.down;
                    }

                }
            }
        }
    }


	void win()
	{
        //Julia, lägg in några coola animeringseffekter. Fyrverkerier eller nått xD
        userDisplay.GetComponent<Text>().text = "WINNER :D";
        fireworks.GetComponent<Renderer>().enabled = true;
        Debug.Log("YOU WIIIN YEEEH.");

	}

	void loss()
	{
		//Julia, lägg in några coola animeringseffekter. :)  typ regn, #sadface
		Debug.Log("You lose, goddamit. Loser. :/");
        userDisplay.GetComponent<Text>().text = "YOU ARE THE LOOOOOOZER";
    }

	void fireMissle(string boxName)
	//creates a missle above the box "A1,A2..." that was sent by command
	{
        /*Vector3 place = GameObject.Find(boxName).transform.position;
        Rigidbody rocketClone = (Rigidbody)Instantiate(rocket, place, transform.rotation * Quaternion.Euler(90, 0, 0));
        rocketClone.transform.parent = transform; //fäster raketen på board

        rocketClone.transform.position += transform.up * 40f; //vrider raketen med huvet ner

        rocketClone.velocity = -transform.up * 5f * speed; //hastighet nedåt*/
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

			rocketClone.velocity = -transform.up * 5f * speed ; //hastighet nedåt

            sinkingBoat("A1");

            }
    }

    public void OnChildsTriggerEnter(string name, Collider other)
    {
        Debug.Log("Placed a boat named: " + other.name);
        if (boatsPos.ContainsKey(other.name))
        {
            List<string> list = boatsPos[other.name];
            if (list.Contains(name) == false)
            {
                list.Add(name);
                Debug.Log("box name" + name);
            }
        }
        else
        {
            List<string> list = new List<string>();
            list.Add(name);
            boatsPos.Add(other.name, list);
            Debug.Log("box name when boat existed in list" + name);
            Debug.Log(GetComponent<Collider>());
        }
    }


    public void OnChildsTriggerExit(string name, Collider other)
    {
        Debug.Log("Boat gone");

        if (boatsPos.ContainsKey(other.name))
        {
            boatsPos.Remove(other.name);
        }
    }


    IEnumerator SendPosBoat(float time)
    {
		//Bygger på tokens för båtar. 
        yield return new WaitForSeconds(time);
        string listToSend = "";

        foreach (string boat in boatsPos.Keys)
        {
            List<string> posList = boatsPos[boat];
            foreach (string position in posList)
            {
                listToSend = listToSend + position;
            }
            listToSend = listToSend + "x";
        }
        //WWWForm form = new WWWForm();
        //form.AddField("Positions", listToSend);
		listToSend = "A1B1xD2D3D4"; //position of ship

		string urlBoats = url + "?init=" + listToSend;
        Debug.Log("Sending " + urlBoats);
        WWW www = new WWW(urlBoats);
        //StartCoroutine(WaitForRequest(www));
		yield return www;

		Debug.Log("You are now " + www.text);
		player = www.text;
        var allColliders = GetComponentsInChildren<Collider>();
        foreach (var childCollider in allColliders)
        {
            Destroy(childCollider);
        }
    }

        IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
   
        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!");
        }
        else
        {
            Debug.Log("WWW Error");
        }
    }

}

