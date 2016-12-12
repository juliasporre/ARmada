using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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
    public Rigidbody rocket;
    public float speed = 10f;

    public string url = "http://130.229.174.226:8000/game"; //use this if other computer is server 
    //public string url = "http://localhost:8000/game";
	// hårdkodat atm, får kolla på detta senare
    private IEnumerator printMessage;
    int t = 0; //Begin the game, waiting for command 0

    void Start()
    {
        StartCoroutine(SendPosBoat(1)); //when starting the game, wait 20 sec to position the boats on the board
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
		}
		else if (commands[1] != player && commands[2] == "WIN") //X=sunk
		{
			loss();
		}
        else if(commands[1] == player && commands[3] == "x") //x= hit
        {
            bombOpponentHit(commands[2]);
        }
        else if (commands[1] == player && commands[3] == "o") //o = miss
        {
            bombOpponentMiss(commands[2]);
        }
		else if (commands[1] == player && commands[3] == "X") //X=sunk
		{
			bombOpponentSunk(commands[2]);
		}
		//if turn is opponent
        else if (commands[1] != player && commands[3] == "x") //x= hit
        {
            bombMeHit(commands[2]);
        }
        else if (commands[1] != player && commands[3] == "o") //o=miss
        {
            bombMeMiss(commands[2]);
        }
		else if (commands[1] != player && commands[3] == "X") //X=sunk
		{
			bombMeSunk(commands[2]);
		}
	
    }

    void bombOpponentHit(string bombPos)
    {
        Debug.Log("Your attack hit!");
        Vector3 place = GameObject.Find("o" + bombPos).transform.position;
        place.y = 10;
        Object bomb = Instantiate(originalbomb, place, originalbomb.transform.rotation);
    }

    void bombOpponentMiss(string bombPos)
    {
        Debug.Log("Your attack missed.");
        Vector3 place = GameObject.Find("o" + bombPos).transform.position;
        place.y = 10;
        Object bomb = Instantiate(originalbomb, place, originalbomb.transform.rotation);

}
	void bombOpponentSunk(string bombPos)    
	{
		Debug.Log("You sank the opponents boat!");
		Vector3 place = GameObject.Find("o" + bombPos).transform.position;
		place.y = 10;
		Object bomb = Instantiate(originalbomb, place, originalbomb.transform.rotation);

	}

    void bombMeHit(string bombPos)
    {
        Debug.Log("Your boat was hit!");
        Vector3 place = GameObject.Find(bombPos).transform.position;
        place.y = 10;
        Object bomb = Instantiate(originalbomb, place, originalbomb.transform.rotation);
    }

    void bombMeMiss(string bombPos)
    {
        Debug.Log("Your boats was not hit.");
        Vector3 place = GameObject.Find(bombPos).transform.position;
        place.y = 10;
        Object bomb = Instantiate(originalbomb, place, originalbomb.transform.rotation);
    }

	void bombMeSunk(string bombPos)
	{
		Debug.Log("Your boat was sunk.");
		Vector3 place = GameObject.Find(bombPos).transform.position;
		place.y = 10;
		Object bomb = Instantiate(originalbomb, place, originalbomb.transform.rotation);
	}

	void win()
	{
		//Julia, lägg in några coola animeringseffekter. 
		Debug.Log("YOU WIIIN YEEEH.");

	}

	void loss()
	{
		//Julia, lägg in några coola animeringseffekter. :) 
		Debug.Log("You loss, goddamit. Loser. :/");
	}


    void Update()
    {

        if (Input.GetKeyDown("space"))
       {
            Debug.Log("TEST BOMBING");
            Vector3 place = GameObject.Find("E5").transform.position;
            Debug.Log(place);
            place.x = 0;
            place.y = 0;
            place.z = 0;

            Rigidbody rocketClone = (Rigidbody)Instantiate(rocket, place, transform.rotation);
            //rocketClone.velocity = transform.forward * speed;

            // You can also acccess other components / scripts of the clone
            //rocketClone.GetComponent<MyRocketScript>().DoSomething();
            }

            // Calls the fire method when holding down ctrl or mouse



            //Object bomb = Instantiate(originalbomb, place, originalbomb.transform.rotation);
            //bomb.addComponent
            //transform.Translate(Vector3.up * 260 * Time.deltaTime, Space.World);
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


    void OnTriggerExit(Collider other)
    {
        Debug.Log("Boat gone");

        if (boatsPos.ContainsKey(other.name))
        {
            boatsPos.Remove(other.name);
        }
    }


    IEnumerator SendPosBoat(float time)
    {
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
		listToSend = "A1B1xD2D3D4";

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

