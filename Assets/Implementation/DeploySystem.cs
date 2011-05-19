using UnityEngine;
using System.Collections;
using DeskMetrics;

sealed public class DeploySystem : MonoBehaviour
{   
    
    public DeskMetrics.Watcher deskMetrics = null;

	private string m_sApplicationID = "4d47c012d9340b116a000000";
    private string m_sApplicationVersion = "1.0";
    private string m_sApplicationName = "Unity3D";

    #region APPLICATION PROPERTY

    public string applicationID
    {
        get
        {
            return m_sApplicationID;
        }
        set
        {
            m_sApplicationID = value;
        }
    }

    public string applicationVersion
    {
        get
        {
            return m_sApplicationVersion;
        }
        set
        {
            m_sApplicationVersion = value;
        }
    }

    public string applicationName
    {
        get
        {
            return m_sApplicationName;
        }
        set
        {
            m_sApplicationName = value;
        }
    }

    #endregion

    void Awake()
    {
        deskMetrics = new DeskMetrics.Watcher();
    }
	
    void Start() 
    {
		deskMetrics.Services.PostPort = 80;
        deskMetrics.Start( applicationID, applicationVersion );
        deskMetrics.Enabled = true;
	}
	
	// Update is called once per frame
	void Update() 
    {
	
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(300, 80, 100, 20), "DeskMetrics"))
        {
            deskMetrics.TrackEventPeriod("Version", "1.0", 30, true);
            deskMetrics.SendDataAsync();
        }
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    void OnDestroy()
    {
        Debug.Log("Destroy");
        deskMetrics.Stop();
    }

    void Initialize()
    {
        if (deskMetrics == null)
            Debug.Log("DeploySystem: deskMetrics is null please Initialize");
    }
}
