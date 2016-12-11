using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardScript : MonoBehaviour {

    Dictionary<string, List<string>> boatsPos = new Dictionary<string, List<string>>();
    string player;

    //public string url = "http://130.229.163.161:8000/game";
    public string url = "http://localhost:8000/game";
    private IEnumerator printMessage;
    int t = -1;

    void Start()
    {
        StartCoroutine(SendPosBoat(30));
        //InvokeRepeating("callHelper",0.5f,0.5f); //many calls
        InvokeRepeating("callHelper", 0.5f, 5f);
    }

    void callHelper()
    {
        printMessage = getMessage();
        StartCoroutine(printMessage);
    }

    IEnumerator getMessage()
    {
        WWW www = new WWW(url + "?t=" + t);
        Debug.Log("Getting message from server");
        yield return www;

        if (www.text.Length > 0)
        {
            t++;
            Debug.Log(www.text);
            string recivedCommands = www.text;
            string[] commands = recivedCommands.Split('x');
            if(commands.Length == 1)
            {
                Debug.Log("You are now " + commands[0]);
                player = commands[0];
            }
            else if(commands.Length > 1)
            {
                makeMove(commands);
            }
        }
    }

    void makeMove(string[] commands)
    {
        if(commands[1] == player && commands[3] == "x")
        {
            bombOpponentHit(commands[2]);
        }
        else if (commands[1] == player && commands[3] == "o")
        {
            bombOpponentMiss(commands[2]);
        }
        else if (commands[1] != player && commands[3] == "x")
        {
            bombMeHit(commands[2]);
        }
        else if (commands[1] == player && commands[3] == "o")
        {
            bombMeMiss(commands[2]);
        }
    }

    void bombOpponentHit(string bombPos)
    {
        Debug.Log("Opponent hit!");
    }

    void bombOpponentMiss(string bombPos)
    {
        Debug.Log("Opponent miss!");

    }

    void bombMeHit(string bombPos)
    {
        Debug.Log("Me hit!");
    }

    void bombMeMiss(string bombPos)
    {
        Debug.Log("Me miss!");
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

        WWWForm form = new WWWForm();
        form.AddField("Positions", listToSend);

        string urlBoats = "?init=" + listToSend;
        Debug.Log("Sending" + listToSend);
        //WWW www = new WWW("http://localhost:8000/game?init=A1B1C1xD4D5"); 
        WWW www = new WWW(urlBoats);
        StartCoroutine(WaitForRequest(www));
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

