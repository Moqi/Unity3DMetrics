﻿// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - Services.cs                                     //
//     Copyright (c) 2010-2011 DeskMetrics Limited                       //
//                                                                       //
//     http://deskmetrics.com                                            //
//     http://support.deskmetrics.com                                    //
//                                                                       //
//     support@deskmetrics.com                                           //
//                                                                       //
//     This code is provided under the DeskMetrics Modified BSD License  //
//     A copy of this license has been distributed in a file called      //
//     LICENSE with this source code.                                    //
//                                                                       //
// **********************************************************************//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Collections;
using System.Net.Security;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

using UnityEngine;

namespace DeskMetrics
{
    public class Services
    {
        private string _proxyusername;

        private string _proxypassword;

        private string _proxyhost;

        private Int32 _proxyport;

        public string ProxyHost
        {
            get
            {
                return _proxyhost;
            }
            set
            {
                _proxyhost = value;
            }
        }

        public string ProxyUserName
        {
            get
            {
                return _proxyusername;
            }
            set
            {
                _proxyusername = value;
            }
        }

        public string ProxyPassword
        {
            get
            {
                return _proxypassword;
            }
            set
            {
                _proxypassword = value;
            }
        }
 
        public Int32 ProxyPort
        {
            get
            {
                return _proxyport;
            }
            set
            {
                _proxyport = value;
            }
        }
		
		string _postserver = Settings.DefaultServer;
		internal string PostServer
        {
            get
            {
                return _postserver;
            }
            set
            {
                _postserver = value;
            }
        }
		
		int _postport = Settings.DefaultPort;
        internal int PostPort
        {
            get
            {
                return _postport;
            }
            set
            {
                _postport = value;
            }
        }

		int _posttimeout = Settings.Timeout;
        public int PostTimeOut{
            get
            {
                return _posttimeout;
            }
            set 
            {
                _posttimeout = value;
            }
        }
		
        private Watcher watcher;
        public Services(Watcher watcher)
        {
            this.watcher = watcher;
        }

        protected object ObjectLock = new System.Object();

        Thread SendDataThread;

        internal string PostData(string PostMode,string json)
        {
            lock (ObjectLock)
            {
				watcher.CheckApplicationCorrectness();
                string url;
				
				PostPort = 80;
				
                if (PostPort == 443)
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                        delegate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslError)
                        {
                            bool validationResult = true;
                            return validationResult;
                        };

                    url = "https://" + watcher.ApplicationId + "." + Settings.DefaultServer + PostMode;
                }
                else
                {
                    url = "http://" + watcher.ApplicationId + "." + Settings.DefaultServer + PostMode;
                }
				/*
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				
                //request.Timeout = Settings.Timeout;
				request.Timeout = 10000;

                if (!string.IsNullOrEmpty(ProxyHost))
                {
                    string uri;

                    WebProxy myProxy = new WebProxy();

                    if (ProxyPort != 0)
                    {
                        uri = ProxyHost + ":" + ProxyPort ;
                    }
                    else
                    {
                        uri = ProxyHost;
                    }

                    Uri newUri = new Uri(uri);
                    myProxy.Address = newUri;
                    myProxy.Credentials = new NetworkCredential(ProxyUserName, ProxyPassword);
                    request.Proxy = myProxy;
                }
                else
                {
                    request.Proxy = WebRequest.DefaultWebProxy;
                }

                request.UserAgent = Settings.UserAgent;
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Method = "POST";

                byte[] postBytes = null;

                postBytes = Encoding.UTF8.GetBytes("data=[" + json + "]");

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postBytes.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(postBytes, 0, postBytes.Length);
                requestStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader streamreader = new StreamReader(response.GetResponseStream());
                Console.WriteLine(streamreader.ReadToEnd());
                streamreader.Close();
                */
				
				byte[] postBytes = null;				

                postBytes = Encoding.UTF8.GetBytes("data=[" + json + "]");
				
				Hashtable headers = new Hashtable();
				headers.Add("Content-type", "application/x-www-form-urlencoded");
				headers.Add("Content-Length", postBytes.Length);
								
				WWW www = new WWW(url, postBytes ,headers);    				
				
				
				return "";

            }
        }

        /// <summary>
        /// </summary>
        /// <param name="Log">json message</param>
        internal void SendData(string json)
        {
            lock (ObjectLock)
            {
                if (watcher.Started)
                    if (!string.IsNullOrEmpty(watcher.ApplicationId) && (watcher.Enabled == true))
                        PostData(Settings.ApiEndpoint,json);
            }
        }
    }
}
