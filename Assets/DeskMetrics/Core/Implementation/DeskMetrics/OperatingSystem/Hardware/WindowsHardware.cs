// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - OperatingSystem/Hardware/WindowsHardware.cs     //
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
using System.Text.RegularExpressions;
using System.IO;

using UnityEngine;

namespace DeskMetrics.OperatingSystem.Hardware
{
	public class WindowsHardware:IHardware
	{

		        /// <summary>
        /// Field Processor Name
        /// </summary>
        private string _processorName;
        /// <summary>
        /// Field Processor Archicteture
        /// </summary>
        private int _processorArchicteture;
        /// <summary>
        /// Field Processor Cores
        /// </summary>
        private int _processorCore;
        /// <summary>
        /// Field Memory total
        /// </summary>
        private double _memoryTotal;
        /// <summary>
        /// Field Memory free
        /// </summary>
        private double _memoryFree;
        /// <summary>
        /// Field Disk space total
        /// </summary>
        private long _diskTotal;
        /// <summary>
        /// Field Disk free
        /// </summary>
        private long _diskFree;
        /// <summary>
        /// Field Screen Resolution
        /// </summary>
        private string _screenResolution;
        /// <summary>
        /// Field Processor Brand
        /// </summary>
        private string _processorBrand;
        /// <summary>
        /// Field Processor Frequency
        /// </summary>
        private double _processorFrequency;
		
		#region IHardware implementation
		public override  string ProcessorName {
			get {
                return _processorName;
			}
			set {
                _processorName = value;
            }
		}

		public override  int ProcessorArchicteture {
			get {
                return _processorArchicteture;
			}
			set {
                _processorArchicteture = value;
			}
		}

		public override  int ProcessorCores {
			get {
                return _processorCore;
			}
			set {
                _processorCore = value;
			}
		}

		public override double MemoryTotal {
			get {
                return _memoryTotal;
			}
			set {
                _memoryTotal = value;
			}
		}

		public override double MemoryFree {
			get {
                return _memoryFree;
			}
			set {
                _memoryFree = value;
			}
		}

		public override long DiskTotal {
			get {
                return _diskTotal;
			}
			set {
                _diskTotal = value;
			}
		}

		public override long DiskFree {
			get {
                return _diskFree;
			}
			set {
                _diskFree = value;
			}
		}

		public override string ScreenResolution {
			get {
                return _screenResolution;
			}
			set {
                _screenResolution = value;
			}
		}

		public override string ProcessorBrand {
			get {
                return _processorBrand;
			}
			set {
                _processorBrand = value;
			}
		}

		public override double ProcessorFrequency {
			get {
                return _processorFrequency;
			}
			set {
                _processorFrequency = value;
			}
		}
		#endregion
		
		Dictionary<int, int> arch = new Dictionary<int, int>
        {
            {0,32}, //x86
            {1,32}, //MIPS
            {2,32}, //Alpha
            {3,32}, //PowerPC
            {6,32}, //Itanium
            {9,64} //x64
        };

        private Dictionary<int, int> Arch
        {
            get { return arch; }
        }

        public WindowsHardware()
        {
            GetProcessorData();
            GetMemoryData();
            GetScreenResolution();
            GetDiskData();
        }
		
		void GetProcessorData()
        {
            try
            {
				GetProcessorDataFromManagementObject();
            }
            catch 
            {
                //Probably Unix
            }
        }

        void GetProcessorDataFromManagementObject()
        {
            GetProcessorName();
            GetProcessorBrand();
            GetProcessorArchitecture();
            GetNumberOfProcessorCores();
            GetProcessorFrequency();
        }

        void GetProcessorFrequency()
        {
            try
            {
				// Don't work yet!!!
				//FIX
				ProcessorFrequency = 0;
            }
            catch
            {
                ProcessorFrequency = -1;
            }
        }

        void GetNumberOfProcessorCores()
        {
            try
            {                
                ProcessorCores = SystemInfo.processorCount;
            }
            catch
            {
                ProcessorCores = -1;
            }
        }

        void GetProcessorArchitecture()
        {
            try
            {
				// FIX
				ProcessorArchicteture = -1;
            }
            catch
            {
                ProcessorArchicteture = -1;
            }
        }

        void GetProcessorBrand()
        {
            try
            {
                ProcessorBrand = SystemInfo.processorType;
            }
            catch
            {
                ProcessorBrand = "null";
            }
        }

        void GetProcessorName()
        {
            try
            {
                string valuename = SystemInfo.processorType;

                if (valuename != "")
                {
                    valuename = valuename.Replace("(TM)", "");
                    valuename = valuename.Replace("(R)", "");
                    valuename = valuename.Replace("  ", "");

					ProcessorName = valuename;
                }
            }
            catch (Exception)
            {
                ProcessorName = "null";
            }
        }

        /// <summary>
        /// GetProcessorFrequency  Physical Memory  GetComponentName
        /// </summary>
        void GetMemoryData()
        {

            try
            {
				// FIX
				MemoryFree  = SystemInfo.systemMemorySize;
				MemoryTotal = SystemInfo.systemMemorySize;                
            }
            catch
            {
				try
				{
					string[] free = IOperatingSystem.GetCommandExecutionOutput("free","-m").Split('\n');
					string memoryinfo = free[1];
					Regex regex = new Regex(@"\d+");
					MatchCollection matches = regex.Matches(memoryinfo);
					double mega = 1024*1024;
	                MemoryFree = Int32.Parse(matches[2].ToString())*mega;
	                MemoryTotal = Int32.Parse(matches[0].ToString())*mega;
				}
				catch
				{
					MemoryFree = 0;
					MemoryTotal = 0;
				}
            }

        }

        /// <summary>
        /// GetProcessorFrequency Disk Size  GetComponentName
        /// </summary>

        void GetDiskData()
        {
            try
            {
                string[] diretorios = Directory.GetLogicalDrives();
                foreach (string item in diretorios)
                {
                    if (Directory.Exists(item + "Windows"))
                    {
                        DriveInfo _drive = new DriveInfo(item);
                        DiskTotal = _drive.TotalSize;
                        DiskFree  = _drive.TotalFreeSpace;
                    }
                }
            }
            catch
            {
                DiskTotal = -1;
                DiskFree = -1;
            }
        }
        /// <summary>
        /// GetProcessorFrequency Screen resolution GetComponentName
        /// </summary>
        void GetScreenResolution()
        {
            try
            {			
				ScreenResolution = Screen.width + "x" + Screen.width;
            }
            catch
            {
                ScreenResolution = "null";
            }
        }    

	}
}

