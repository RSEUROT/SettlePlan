using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomicBrainGoap : MonoBehaviour
{
    private static EconomicBrainGoap instance;
    public static EconomicBrainGoap Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject("Economic_BrainGoap").AddComponent<EconomicBrainGoap>();
            }
            return instance;
        }
    }

    int numPeasant;
    private List<AgentGoap> allPeasants;

    private int numSupplyPile;
    private int numField;
    private int numWill;
    private int numHouse;
    private int numForge;

    // resources
    [SerializeField]
    private int numLogs;
    [SerializeField]
    private int numOres;
    [SerializeField]
    private int numBlee;
    [SerializeField]
    private int numBreads;

    public int NumLogs
    {
        get
        {
            return numLogs;
        }

        set
        {
            numLogs = value;
        }
    }
    public int NumOres
    {
        get
        {
            return numOres;
        }

        set
        {
            numOres = value;
        }
    }
    public int NumBlee
    {
        get
        {
            return numBlee;
        }

        set
        {
            numBlee = value;
        }
    }
    public int NumBreads
    {
        get
        {
            return numBreads;
        }

        set
        {
            numBreads = value;
        }
    }


    // Use this for initialization
    void Awake ()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        UpdatePaesantsList();

    }
	private void UpdatePaesantsList()
    {
        allPeasants = new List<AgentGoap>();

        AgentGoap[] tempAgent = FindObjectsOfType<AgentGoap>();

        for (int i = 0; i < tempAgent.Length; i++)
        {
            numPeasant++;
            allPeasants.Add(tempAgent[i]);
        }
    }
    private void AddPaesant()
    {
    }

    private void MakeChoice(AgentGoap _ag)
    {
        _ag.RemovePeopleDatas();

        if(_ag.ratio < 0.2f && numBreads > 0)
        {
            _ag.gameObject.AddComponent<Eater>();
            _ag.gameObject.GetComponent<Eater>().SetDefaultActions();
        }
        else if ((float)numBreads < ((float)numPeasant * 0.5f))
        {
            if ((float)numBlee < ((float)numPeasant * 0.3f))
            {               
                _ag.gameObject.AddComponent<Farmer>();
                _ag.gameObject.GetComponent<Farmer>().SetDefaultActions();
            }
            else
            {
                _ag.gameObject.AddComponent<Cooker>();
                _ag.gameObject.GetComponent<Cooker>().SetDefaultActions();
            }
        }
        else if ((float)numLogs < (float)numOres)
        {
            _ag.gameObject.AddComponent<Logger>();
            _ag.gameObject.GetComponent<Logger>().SetDefaultActions();
        }
        else
        {
            _ag.gameObject.AddComponent<Miner>();
            _ag.gameObject.GetComponent<Miner>().SetDefaultActions();
        }
    }

    public void AgentInactive(AgentGoap ag)
    {
        MakeChoice(ag);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
