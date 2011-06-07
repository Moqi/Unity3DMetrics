using UnityEngine;
using System.Collections;
using DeskMetrics;

sealed public class DeploySystem : MonoBehaviour
{       
    public DeskMetrics.Watcher deskMetrics = null;

	public string m_ApplicationID = "";
    public string m_ApplicationVersion = "1.0";
    public string m_ApplicationName = "Unity3D";

    #region APPLICATION PROPERTY

    public string applicationID
    {
        get
        {
            return m_ApplicationID;
        }
        set
        {
            m_ApplicationID = value;
        }
    }

    public string applicationVersion
    {
        get
        {
            return m_ApplicationVersion;
        }
        set
        {
            m_ApplicationVersion = value;
        }
    }

    public string applicationName
    {
        get
        {
            return m_ApplicationName;
        }
        set
        {
            m_ApplicationName = value;
        }
    }

    #endregion

    void Awake()
    {        
        deskMetrics = new DeskMetrics.Watcher();
    }
	
    void Start() 
    {		
        deskMetrics.Start( applicationID, applicationVersion );		
		
        deskMetrics.TrackCustomData("Video Controller", SystemInfo.graphicsDeviceName);
        deskMetrics.TrackCustomData("Operating system", SystemInfo.operatingSystem);
        deskMetrics.TrackCustomData("Processor name", SystemInfo.processorType);
        deskMetrics.TrackCustomData("Processors count", SystemInfo.processorCount.ToString());
				
		deskMetrics.TrackCustomData("System Memory", SystemInfo.systemMemorySize.ToString());
		deskMetrics.TrackCustomData("Graphics Memory", SystemInfo.graphicsMemorySize.ToString());
		deskMetrics.TrackCustomData("Display Resolution", Screen.currentResolution.ToString() );
		deskMetrics.TrackCustomData("Language system", Application.systemLanguage.ToString() );                
	}

	private int _buttonCountDown = 0;
	void OnGUI()
	{
		if (GUI.Button(new Rect(100, 50, 120, 30), "Track Event Value"))
		{
			_buttonCountDown++;			
		}

		if (GUI.Button(new Rect(280, 50, 120, 30), "Track Event"))
		{
			deskMetrics.TrackEvent("Track Event", " Click On MAGIC BUTTON");
		}
	}
	
    void OnDestroy()
    {
		deskMetrics.TrackEventValue("Button 1 Down", "Button Down", _buttonCountDown.ToString() );                
        deskMetrics.Stop();               
    }

    void Initialize()
    {
        if (deskMetrics == null)
            Debug.Log("DeploySystem: deskMetrics is null please Initialize");
    }
}
