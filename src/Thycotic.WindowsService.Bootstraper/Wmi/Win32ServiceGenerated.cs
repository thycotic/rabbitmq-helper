#region WMI Generated

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Management;

namespace Thycotic.WindowsService.Bootstraper.Wmi
{
	[System.CodeDom.Compiler.GeneratedCode("WMI CodeDom", "3.5.0.0")]
	internal partial class Win32Service : Component, IWin32Service
	{
		// Private property to hold the WMI namespace in which the class resides.
		private static string CreatedWmiNamespace = "root\\cimv2";

		// Private property to hold the name of WMI class which created this class.
		private static string CreatedClassName = "Win32_Service";

		// Private member variable to hold the ManagementScope which is used by the various methods.
		private static ManagementScope statMgmtScope;

		private ManagementSystemProperties PrivateSystemProperties;

		// Underlying lateBound WMI object.
		private ManagementObject PrivateLateBoundObject;

		// Member variable to store the 'automatic commit' behavior for the class.
		private bool AutoCommitProp;

		// Private variable to hold the embedded property representing the instance.
		private readonly ManagementBaseObject embeddedObj;

		// The current WMI object used
		private ManagementBaseObject curObj;

		// Flag to indicate if the instance is an embedded object.
		private bool isEmbedded;

		// Below are different overloads of constructors to initialize an instance of the class with a WMI object.
		public Win32Service()
		{
			InitializeObject(null, null, null);
		}

		public Win32Service(string keyName)
		{
			InitializeObject(null, new ManagementPath(ConstructPath(keyName)), null);
		}

		public Win32Service(ManagementScope mgmtScope, string keyName)
		{
			InitializeObject(((mgmtScope)), new ManagementPath(ConstructPath(keyName)), null);
		}

		public Win32Service(ManagementPath path, ObjectGetOptions getOptions)
		{
			InitializeObject(null, path, getOptions);
		}

		public Win32Service(ManagementScope mgmtScope, ManagementPath path)
		{
			InitializeObject(mgmtScope, path, null);
		}

		public Win32Service(ManagementPath path)
		{
			InitializeObject(null, path, null);
		}

		public Win32Service(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions getOptions)
		{
			InitializeObject(mgmtScope, path, getOptions);
		}

		public Win32Service(ManagementObject theObject)
		{
			Initialize();
			if (CheckIfProperClass(theObject))
			{
				PrivateLateBoundObject = theObject;
				PrivateSystemProperties = new ManagementSystemProperties(PrivateLateBoundObject);
				curObj = PrivateLateBoundObject;
			}
			else
			{
				throw new ArgumentException("Class name does not match.");
			}
		}

		[Obsolete("Do not use this constructor. Use one that does not require embedding.", true)]
		public Win32Service(ManagementBaseObject theObject)
		{
			Initialize();
			if (CheckIfProperClass(theObject))
			{
				embeddedObj = theObject;
				PrivateSystemProperties = new ManagementSystemProperties(theObject);
				curObj = embeddedObj;
				isEmbedded = true;
			}
			else
			{
				throw new ArgumentException("Class name does not match.");
			}
		}

		// Property returns the namespace of the WMI class.
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string OriginatingNamespace
		{
			get { return "root\\cimv2"; }
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ManagementClassName
		{
			get
			{
				string strRet = CreatedClassName;
				if ((curObj != null))
				{
					if ((curObj.ClassPath != null))
					{
						strRet = ((string) (curObj["__CLASS"]));
						if ((string.IsNullOrEmpty(strRet)))
						{
							strRet = CreatedClassName;
						}
					}
				}
				return strRet;
			}
		}

		// Property pointing to an embedded object to get System properties of the WMI object.
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ManagementSystemProperties SystemProperties
		{
			get { return PrivateSystemProperties; }
		}

		// Property returning the underlying lateBound object.
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ManagementBaseObject LateBoundObject
		{
			get { return curObj; }
		}

		// ManagementScope of the object.
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ManagementScope Scope
		{
			get
			{
				if ((isEmbedded == false))
				{
					return PrivateLateBoundObject.Scope;
				}
				else
				{
					return null;
				}
			}
			set
			{
				if ((isEmbedded == false))
				{
					PrivateLateBoundObject.Scope = value;
				}
			}
		}

		// Property to show the commit behavior for the WMI object. If true, WMI object will be automatically saved after each property modification.(ie. Put() is called after modification of a property).
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AutoCommit
		{
			get { return AutoCommitProp; }
			set { AutoCommitProp = value; }
		}

		// The ManagementPath of the underlying WMI object.
		[Browsable(true)]
		public ManagementPath Path
		{
			get
			{
				if ((isEmbedded == false))
				{
					return PrivateLateBoundObject.Path;
				}
				else
				{
					return null;
				}
			}
			set
			{
				if ((isEmbedded == false))
				{
					if ((CheckIfProperClass(null, value, null) != true))
					{
						throw new ArgumentException("Class name does not match.");
					}
					PrivateLateBoundObject.Path = value;
				}
			}
		}

		// Public static scope property which is used by the various methods.
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public static ManagementScope StaticScope
		{
			get { return statMgmtScope; }
			set { statMgmtScope = value; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsAcceptPauseNull
		{
			get
			{
				if ((curObj["AcceptPause"] == null))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof (WMIValueTypeConverter))]
		public bool AcceptPause
		{
			get
			{
				if ((curObj["AcceptPause"] == null))
				{
					return Convert.ToBoolean(0);
				}
				return ((bool) (curObj["AcceptPause"]));
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsAcceptStopNull
		{
			get
			{
				if ((curObj["AcceptStop"] == null))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof (WMIValueTypeConverter))]
		public bool AcceptStop
		{
			get
			{
				if ((curObj["AcceptStop"] == null))
				{
					return Convert.ToBoolean(0);
				}
				return ((bool) (curObj["AcceptStop"]));
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Caption
		{
			get { return ((string) (curObj["Caption"])); }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsCheckPointNull
		{
			get
			{
				if ((curObj["CheckPoint"] == null))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof (WMIValueTypeConverter))]
		public uint CheckPoint
		{
			get
			{
				if ((curObj["CheckPoint"] == null))
				{
					return Convert.ToUInt32(0);
				}
				return ((uint) (curObj["CheckPoint"]));
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string CreationClassName
		{
			get { return ((string) (curObj["CreationClassName"])); }
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Description
		{
			get { return ((string) (curObj["Description"])); }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsDesktopInteractNull
		{
			get
			{
				if ((curObj["DesktopInteract"] == null))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof (WMIValueTypeConverter))]
		public bool DesktopInteract
		{
			get
			{
				if ((curObj["DesktopInteract"] == null))
				{
					return Convert.ToBoolean(0);
				}
				return ((bool) (curObj["DesktopInteract"]));
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string DisplayName
		{
			get { return ((string) (curObj["DisplayName"])); }
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ErrorControl
		{
			get { return ((string) (curObj["ErrorControl"])); }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsExitCodeNull
		{
			get
			{
				if ((curObj["ExitCode"] == null))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof (WMIValueTypeConverter))]
		public uint ExitCode
		{
			get
			{
				if ((curObj["ExitCode"] == null))
				{
					return Convert.ToUInt32(0);
				}
				return ((uint) (curObj["ExitCode"]));
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsInstallDateNull
		{
			get
			{
				if ((curObj["InstallDate"] == null))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof (WMIValueTypeConverter))]
		public DateTime InstallDate
		{
			get
			{
				if ((curObj["InstallDate"] != null))
				{
					return ToDateTime(((string) (curObj["InstallDate"])));
				}
				else
				{
					return DateTime.MinValue;
				}
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string Name
		{
			get { return ((string) (curObj["Name"])); }
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string PathName
		{
			get { return ((string) (curObj["PathName"])); }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsProcessIdNull
		{
			get
			{
				if ((curObj["ProcessId"] == null))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof (WMIValueTypeConverter))]
		public uint ProcessId
		{
			get
			{
				if ((curObj["ProcessId"] == null))
				{
					return Convert.ToUInt32(0);
				}
				return ((uint) (curObj["ProcessId"]));
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsServiceSpecificExitCodeNull
		{
			get
			{
				if ((curObj["ServiceSpecificExitCode"] == null))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof (WMIValueTypeConverter))]
		public uint ServiceSpecificExitCode
		{
			get
			{
				if ((curObj["ServiceSpecificExitCode"] == null))
				{
					return Convert.ToUInt32(0);
				}
				return ((uint) (curObj["ServiceSpecificExitCode"]));
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ServiceType
		{
			get { return ((string) (curObj["ServiceType"])); }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsStartedNull
		{
			get
			{
				if ((curObj["Started"] == null))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof (WMIValueTypeConverter))]
		public bool Started
		{
			get
			{
				if ((curObj["Started"] == null))
				{
					return Convert.ToBoolean(0);
				}
				return ((bool) (curObj["Started"]));
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string StartMode
		{
			get { return ((string) (curObj["StartMode"])); }
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string StartName
		{
			get { return ((string) (curObj["StartName"])); }
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string State
		{
			get { return ((string) (curObj["State"])); }
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Status
		{
			get { return ((string) (curObj["Status"])); }
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SystemCreationClassName
		{
			get { return ((string) (curObj["SystemCreationClassName"])); }
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SystemName
		{
			get { return ((string) (curObj["SystemName"])); }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsTagIdNull
		{
			get
			{
				if ((curObj["TagId"] == null))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof (WMIValueTypeConverter))]
		public uint TagId
		{
			get
			{
				if ((curObj["TagId"] == null))
				{
					return Convert.ToUInt32(0);
				}
				return ((uint) (curObj["TagId"]));
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsWaitHintNull
		{
			get
			{
				if ((curObj["WaitHint"] == null))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof (WMIValueTypeConverter))]
		public uint WaitHint
		{
			get
			{
				if ((curObj["WaitHint"] == null))
				{
					return Convert.ToUInt32(0);
				}
				return ((uint) (curObj["WaitHint"]));
			}
		}

		private bool CheckIfProperClass(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions OptionsParam)
		{
			if (((path != null)
			     && (string.Compare(path.ClassName, ManagementClassName, true, CultureInfo.InvariantCulture) == 0)))
			{
				return true;
			}
			else
			{
				return CheckIfProperClass(new ManagementObject(mgmtScope, path, OptionsParam));
			}
		}

		private bool CheckIfProperClass(ManagementBaseObject theObj)
		{
			if (((theObj != null)
			     && (string.Compare(((string) (theObj["__CLASS"])), ManagementClassName, true, CultureInfo.InvariantCulture) == 0)))
			{
				return true;
			}
			else
			{
				Array parentClasses = ((Array) (theObj["__DERIVATION"]));
				if ((parentClasses != null))
				{
					int count = 0;
					for (count = 0; (count < parentClasses.Length); count = (count + 1))
					{
						if ((string.Compare(((string) (parentClasses.GetValue(count))), ManagementClassName, true, CultureInfo.InvariantCulture) == 0))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private bool ShouldSerializeAcceptPause()
		{
			if ((IsAcceptPauseNull == false))
			{
				return true;
			}
			return false;
		}

		private bool ShouldSerializeAcceptStop()
		{
			if ((IsAcceptStopNull == false))
			{
				return true;
			}
			return false;
		}

		private bool ShouldSerializeCheckPoint()
		{
			if ((IsCheckPointNull == false))
			{
				return true;
			}
			return false;
		}

		private bool ShouldSerializeDesktopInteract()
		{
			if ((IsDesktopInteractNull == false))
			{
				return true;
			}
			return false;
		}

		private bool ShouldSerializeExitCode()
		{
			if ((IsExitCodeNull == false))
			{
				return true;
			}
			return false;
		}

		// Converts a given datetime in DMTF format to System.DateTime object.
		private static DateTime ToDateTime(string dmtfDate)
		{
			DateTime initializer = DateTime.MinValue;
			int year = initializer.Year;
			int month = initializer.Month;
			int day = initializer.Day;
			int hour = initializer.Hour;
			int minute = initializer.Minute;
			int second = initializer.Second;
			long ticks = 0;
			string dmtf = dmtfDate;
			DateTime datetime = DateTime.MinValue;
			string tempString = string.Empty;
			if ((dmtf == null))
			{
				throw new ArgumentOutOfRangeException();
			}
			if ((dmtf.Length == 0))
			{
				throw new ArgumentOutOfRangeException();
			}
			if ((dmtf.Length != 25))
			{
				throw new ArgumentOutOfRangeException();
			}
			try
			{
				tempString = dmtf.Substring(0, 4);
				if (("****" != tempString))
				{
					year = int.Parse(tempString);
				}
				tempString = dmtf.Substring(4, 2);
				if (("**" != tempString))
				{
					month = int.Parse(tempString);
				}
				tempString = dmtf.Substring(6, 2);
				if (("**" != tempString))
				{
					day = int.Parse(tempString);
				}
				tempString = dmtf.Substring(8, 2);
				if (("**" != tempString))
				{
					hour = int.Parse(tempString);
				}
				tempString = dmtf.Substring(10, 2);
				if (("**" != tempString))
				{
					minute = int.Parse(tempString);
				}
				tempString = dmtf.Substring(12, 2);
				if (("**" != tempString))
				{
					second = int.Parse(tempString);
				}
				tempString = dmtf.Substring(15, 6);
				if (("******" != tempString))
				{
					ticks = (long.Parse(tempString)*(((TimeSpan.TicksPerMillisecond/1000))));
				}
				if (((((((((year < 0)
				           || (month < 0))
				          || (day < 0))
				         || (hour < 0))
				        || (minute < 0))
				       || (minute < 0))
				      || (second < 0))
				     || (ticks < 0)))
				{
					throw new ArgumentOutOfRangeException();
				}
			}
			catch (Exception e)
			{
				throw new ArgumentOutOfRangeException(null, e.Message);
			}
			datetime = new DateTime(year, month, day, hour, minute, second, 0);
			datetime = datetime.AddTicks(ticks);
			TimeSpan tickOffset = TimeZone.CurrentTimeZone.GetUtcOffset(datetime);
			int UTCOffset = 0;
			int OffsetToBeAdjusted = 0;
			long OffsetMins = (((tickOffset.Ticks/TimeSpan.TicksPerMinute)));
			tempString = dmtf.Substring(22, 3);
			if ((tempString != "******"))
			{
				tempString = dmtf.Substring(21, 4);
				try
				{
					UTCOffset = int.Parse(tempString);
				}
				catch (Exception e)
				{
					throw new ArgumentOutOfRangeException(null, e.Message);
				}
				OffsetToBeAdjusted = ((int) ((OffsetMins - UTCOffset)));
				datetime = datetime.AddMinutes(((OffsetToBeAdjusted)));
			}
			return datetime;
		}

		// Converts a given System.DateTime object to DMTF datetime format.
		private static string ToDmtfDateTime(DateTime date)
		{
			string utcString = string.Empty;
			TimeSpan tickOffset = TimeZone.CurrentTimeZone.GetUtcOffset(date);
			long OffsetMins = (((tickOffset.Ticks/TimeSpan.TicksPerMinute)));
			if ((Math.Abs(OffsetMins) > 999))
			{
				date = date.ToUniversalTime();
				utcString = "+000";
			}
			else
			{
				if ((tickOffset.Ticks >= 0))
				{
					utcString = string.Concat("+", (((tickOffset.Ticks/TimeSpan.TicksPerMinute))).ToString().PadLeft(3, '0'));
				}
				else
				{
					string strTemp = ((OffsetMins)).ToString();
					utcString = string.Concat("-", strTemp.Substring(1, (strTemp.Length - 1)).PadLeft(3, '0'));
				}
			}
			string dmtfDateTime = ((date.Year)).ToString().PadLeft(4, '0');
			dmtfDateTime = string.Concat(dmtfDateTime, ((date.Month)).ToString().PadLeft(2, '0'));
			dmtfDateTime = string.Concat(dmtfDateTime, ((date.Day)).ToString().PadLeft(2, '0'));
			dmtfDateTime = string.Concat(dmtfDateTime, ((date.Hour)).ToString().PadLeft(2, '0'));
			dmtfDateTime = string.Concat(dmtfDateTime, ((date.Minute)).ToString().PadLeft(2, '0'));
			dmtfDateTime = string.Concat(dmtfDateTime, ((date.Second)).ToString().PadLeft(2, '0'));
			dmtfDateTime = string.Concat(dmtfDateTime, ".");
			DateTime dtTemp = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, 0);
			long microsec = (((((date.Ticks - dtTemp.Ticks)
			                    *1000)
			                   /TimeSpan.TicksPerMillisecond)));
			string strMicrosec = ((microsec)).ToString();
			if ((strMicrosec.Length > 6))
			{
				strMicrosec = strMicrosec.Substring(0, 6);
			}
			dmtfDateTime = string.Concat(dmtfDateTime, strMicrosec.PadLeft(6, '0'));
			dmtfDateTime = string.Concat(dmtfDateTime, utcString);
			return dmtfDateTime;
		}

		private bool ShouldSerializeInstallDate()
		{
			if ((IsInstallDateNull == false))
			{
				return true;
			}
			return false;
		}

		private bool ShouldSerializeProcessId()
		{
			if ((IsProcessIdNull == false))
			{
				return true;
			}
			return false;
		}

		private bool ShouldSerializeServiceSpecificExitCode()
		{
			if ((IsServiceSpecificExitCodeNull == false))
			{
				return true;
			}
			return false;
		}

		private bool ShouldSerializeStarted()
		{
			if ((IsStartedNull == false))
			{
				return true;
			}
			return false;
		}

		private bool ShouldSerializeTagId()
		{
			if ((IsTagIdNull == false))
			{
				return true;
			}
			return false;
		}

		private bool ShouldSerializeWaitHint()
		{
			if ((IsWaitHintNull == false))
			{
				return true;
			}
			return false;
		}

		[Browsable(true)]
		public void CommitObject()
		{
			if ((isEmbedded == false))
			{
				PrivateLateBoundObject.Put();
			}
		}

		[Browsable(true)]
		public void CommitObject(PutOptions putOptions)
		{
			if ((isEmbedded == false))
			{
				PrivateLateBoundObject.Put(putOptions);
			}
		}

		private void Initialize()
		{
			PreInitialize();
			AutoCommitProp = true;
			isEmbedded = false;
		}

		partial void PreInitialize();

		private static string ConstructPath(string keyName)
		{
			string strPath = "root\\cimv2:Win32_Service";
			strPath = string.Concat(strPath, string.Concat(".Name=", string.Concat("\"", string.Concat(keyName, "\""))));
			return strPath;
		}

		private void InitializeObject(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions getOptions)
		{
			Initialize();
			if ((path != null))
			{
				if ((CheckIfProperClass(mgmtScope, path, getOptions) != true))
				{
					throw new ArgumentException("Class name does not match.");
				}
			}
			PrivateLateBoundObject = new ManagementObject(mgmtScope, path, getOptions);
			PrivateSystemProperties = new ManagementSystemProperties(PrivateLateBoundObject);
			curObj = PrivateLateBoundObject;
		}

		// Different overloads of GetInstances() help in enumerating instances of the WMI class.
		public static ServiceCollection GetInstances()
		{
			return GetInstances(null, null, null);
		}

		public static ServiceCollection GetInstances(string condition)
		{
			return GetInstances(null, condition, null);
		}

		public static ServiceCollection GetInstances(String[] selectedProperties)
		{
			return GetInstances(null, null, selectedProperties);
		}

		public static ServiceCollection GetInstances(string condition, String[] selectedProperties)
		{
			return GetInstances(null, condition, selectedProperties);
		}

		public static ServiceCollection GetInstances(ManagementScope mgmtScope, EnumerationOptions enumOptions)
		{
			if ((mgmtScope == null))
			{
				if ((statMgmtScope == null))
				{
					mgmtScope = new ManagementScope();
					mgmtScope.Path.NamespacePath = "root\\cimv2";
				}
				else
				{
					mgmtScope = statMgmtScope;
				}
			}
			ManagementPath pathObj = new ManagementPath();
			pathObj.ClassName = "Win32_Service";
			pathObj.NamespacePath = "root\\cimv2";
			ManagementClass clsObject = new ManagementClass(mgmtScope, pathObj, null);
			if ((enumOptions == null))
			{
				enumOptions = new EnumerationOptions();
				enumOptions.EnsureLocatable = true;
			}
			return new ServiceCollection(clsObject.GetInstances(enumOptions));
		}

		public static ServiceCollection GetInstances(ManagementScope mgmtScope, string condition)
		{
			return GetInstances(mgmtScope, condition, null);
		}

		public static ServiceCollection GetInstances(ManagementScope mgmtScope, String[] selectedProperties)
		{
			return GetInstances(mgmtScope, null, selectedProperties);
		}

		public static ServiceCollection GetInstances(ManagementScope mgmtScope, string condition, String[] selectedProperties)
		{
			if ((mgmtScope == null))
			{
				if ((statMgmtScope == null))
				{
					mgmtScope = new ManagementScope();
					mgmtScope.Path.NamespacePath = "root\\cimv2";
				}
				else
				{
					mgmtScope = statMgmtScope;
				}
			}
			ManagementObjectSearcher ObjectSearcher = new ManagementObjectSearcher(mgmtScope, new SelectQuery("Win32_Service", condition, selectedProperties));
			EnumerationOptions enumOptions = new EnumerationOptions();
			enumOptions.EnsureLocatable = true;
			ObjectSearcher.Options = enumOptions;
			return new ServiceCollection(ObjectSearcher.Get());
		}

		[Browsable(true)]
		public static Win32Service CreateInstance()
		{
			ManagementScope mgmtScope = null;
			if ((statMgmtScope == null))
			{
				mgmtScope = new ManagementScope();
				mgmtScope.Path.NamespacePath = CreatedWmiNamespace;
			}
			else
			{
				mgmtScope = statMgmtScope;
			}
			ManagementPath mgmtPath = new ManagementPath(CreatedClassName);
			ManagementClass tmpMgmtClass = new ManagementClass(mgmtScope, mgmtPath, null);
			return new Win32Service(tmpMgmtClass.CreateInstance());
		}

		[Browsable(true)]
		public void Delete()
		{
			PrivateLateBoundObject.Delete();
		}

		public uint Change(bool DesktopInteract, string DisplayName, byte ErrorControl, string LoadOrderGroup, string[] LoadOrderGroupDependencies, string PathName, string[] ServiceDependencies, byte ServiceType, string StartMode, string StartName, string StartPassword)
		{
			if ((isEmbedded == false))
			{
				ManagementBaseObject inParams = null;
				inParams = PrivateLateBoundObject.GetMethodParameters("Change");
				inParams["DesktopInteract"] = ((DesktopInteract));
				inParams["DisplayName"] = ((DisplayName));
				inParams["ErrorControl"] = ((ErrorControl));
				inParams["LoadOrderGroup"] = ((LoadOrderGroup));
				inParams["LoadOrderGroupDependencies"] = ((LoadOrderGroupDependencies));
				inParams["PathName"] = ((PathName));
				inParams["ServiceDependencies"] = ((ServiceDependencies));
				inParams["ServiceType"] = ((ServiceType));
				inParams["StartMode"] = ((StartMode));
				inParams["StartName"] = ((StartName));
				inParams["StartPassword"] = ((StartPassword));
				ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("Change", inParams, null);
				return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
			}
			else
			{
				return Convert.ToUInt32(0);
			}
		}

		public uint ChangeStartMode(string StartMode)
		{
			if ((isEmbedded == false))
			{
				ManagementBaseObject inParams = null;
				inParams = PrivateLateBoundObject.GetMethodParameters("ChangeStartMode");
				inParams["StartMode"] = ((StartMode));
				ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ChangeStartMode", inParams, null);
				return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
			}
			else
			{
				return Convert.ToUInt32(0);
			}
		}

		public uint Create(bool DesktopInteract, string DisplayName, byte ErrorControl, string LoadOrderGroup, string[] LoadOrderGroupDependencies, string Name, string PathName, string[] ServiceDependencies, byte ServiceType, string StartMode, string StartName, string StartPassword)
		{
			if ((isEmbedded == false))
			{
				ManagementBaseObject inParams = null;
				inParams = PrivateLateBoundObject.GetMethodParameters("Create");
				inParams["DesktopInteract"] = ((DesktopInteract));
				inParams["DisplayName"] = ((DisplayName));
				inParams["ErrorControl"] = ((ErrorControl));
				inParams["LoadOrderGroup"] = ((LoadOrderGroup));
				inParams["LoadOrderGroupDependencies"] = ((LoadOrderGroupDependencies));
				inParams["Name"] = ((Name));
				inParams["PathName"] = ((PathName));
				inParams["ServiceDependencies"] = ((ServiceDependencies));
				inParams["ServiceType"] = ((ServiceType));
				inParams["StartMode"] = ((StartMode));
				inParams["StartName"] = ((StartName));
				inParams["StartPassword"] = ((StartPassword));
				ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("Create", inParams, null);
				return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
			}
			else
			{
				return Convert.ToUInt32(0);
			}
		}

		public uint Delete0()
		{
			if ((isEmbedded == false))
			{
				ManagementBaseObject inParams = null;
				ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("Delete", inParams, null);
				return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
			}
			else
			{
				return Convert.ToUInt32(0);
			}
		}

		public ServiceWin32ReturnCode InterrogateService()
		{
			if ((isEmbedded == false))
			{
				ManagementBaseObject inParams = null;
				ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("InterrogateService", inParams, null);
				return (ServiceWin32ReturnCode)Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
			}
			else
			{
				return (ServiceWin32ReturnCode)Convert.ToUInt32(0);
			}
		}

		public uint PauseService()
		{
			if ((isEmbedded == false))
			{
				ManagementBaseObject inParams = null;
				ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("PauseService", inParams, null);
				return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
			}
			else
			{
				return Convert.ToUInt32(0);
			}
		}

		public uint ResumeService()
		{
			if ((isEmbedded == false))
			{
				ManagementBaseObject inParams = null;
				ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ResumeService", inParams, null);
				return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
			}
			else
			{
				return Convert.ToUInt32(0);
			}
		}

		public virtual ServiceWin32ReturnCode StartService()
		{
			if ((isEmbedded == false))
			{
				ManagementBaseObject inParams = null;
				ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("StartService", inParams, null);
				return (ServiceWin32ReturnCode) Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
			}
			else
			{
				return (ServiceWin32ReturnCode) Convert.ToUInt32(0);
			}
		}

		public virtual ServiceWin32ReturnCode StopService()
		{
			if ((isEmbedded == false))
			{
				ManagementBaseObject inParams = null;
				ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("StopService", inParams, null);
				return (ServiceWin32ReturnCode) Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
			}
			else
			{
				return (ServiceWin32ReturnCode) Convert.ToUInt32(0);
			}
		}

		public uint UserControlService(byte ControlCode)
		{
			if ((isEmbedded == false))
			{
				ManagementBaseObject inParams = null;
				inParams = PrivateLateBoundObject.GetMethodParameters("UserControlService");
				inParams["ControlCode"] = ((ControlCode));
				ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("UserControlService", inParams, null);
				return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
			}
			else
			{
				return Convert.ToUInt32(0);
			}
		}

		// Enumerator implementation for enumerating instances of the class.
		public class ServiceCollection : object, ICollection
		{
			private readonly ManagementObjectCollection privColObj;

			public ServiceCollection(ManagementObjectCollection objCollection)
			{
				privColObj = objCollection;
			}

			public virtual int Count
			{
				get { return privColObj.Count; }
			}

			public virtual bool IsSynchronized
			{
				get { return privColObj.IsSynchronized; }
			}

			public virtual object SyncRoot
			{
				get { return this; }
			}

			public virtual void CopyTo(Array array, int index)
			{
				privColObj.CopyTo(array, index);
				int nCtr;
				for (nCtr = 0; (nCtr < array.Length); nCtr = (nCtr + 1))
				{
					array.SetValue(new Win32Service(((ManagementObject)(array.GetValue(nCtr)))), nCtr);
				}
			}

			public virtual IEnumerator GetEnumerator()
			{
				return new ServiceEnumerator(privColObj.GetEnumerator());
			}

			public class ServiceEnumerator : object, IEnumerator
			{
				private readonly ManagementObjectCollection.ManagementObjectEnumerator privObjEnum;

				public ServiceEnumerator(ManagementObjectCollection.ManagementObjectEnumerator objEnum)
				{
					privObjEnum = objEnum;
				}

				public virtual object Current
				{
					get { return new Win32Service(((ManagementObject)(privObjEnum.Current))); }
				}

				public virtual bool MoveNext()
				{
					return privObjEnum.MoveNext();
				}

				public virtual void Reset()
				{
					privObjEnum.Reset();
				}
			}
		}

		// TypeConverter to handle null values for ValueType properties
		public class WMIValueTypeConverter : TypeConverter
		{
			private readonly TypeConverter baseConverter;

			private readonly Type baseType;

			public WMIValueTypeConverter(Type inBaseType)
			{
				baseConverter = TypeDescriptor.GetConverter(inBaseType);
				baseType = inBaseType;
			}

			public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
			{
				return baseConverter.CanConvertFrom(context, srcType);
			}

			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return baseConverter.CanConvertTo(context, destinationType);
			}

			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				return baseConverter.ConvertFrom(context, culture, value);
			}

			public override object CreateInstance(ITypeDescriptorContext context, IDictionary dictionary)
			{
				return baseConverter.CreateInstance(context, dictionary);
			}

			public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
			{
				return baseConverter.GetCreateInstanceSupported(context);
			}

			public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributeVar)
			{
				return baseConverter.GetProperties(context, value, attributeVar);
			}

			public override bool GetPropertiesSupported(ITypeDescriptorContext context)
			{
				return baseConverter.GetPropertiesSupported(context);
			}

			public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				return baseConverter.GetStandardValues(context);
			}

			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return baseConverter.GetStandardValuesExclusive(context);
			}

			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return baseConverter.GetStandardValuesSupported(context);
			}

			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if ((baseType.BaseType == typeof (Enum)))
				{
					if ((value.GetType() == destinationType))
					{
						return value;
					}
					if ((((value == null)
					      && (context != null))
					     && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false)))
					{
						return "NULL_ENUM_VALUE";
					}
					return baseConverter.ConvertTo(context, culture, value, destinationType);
				}
				if (((baseType == typeof (bool))
				     && (baseType.BaseType == typeof (ValueType))))
				{
					if ((((value == null)
					      && (context != null))
					     && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false)))
					{
						return "";
					}
					return baseConverter.ConvertTo(context, culture, value, destinationType);
				}
				if (((context != null)
				     && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false)))
				{
					return "";
				}
				return baseConverter.ConvertTo(context, culture, value, destinationType);
			}
		}

		// Embedded class to represent WMI system Properties.
		[TypeConverter(typeof (ExpandableObjectConverter))]
		public class ManagementSystemProperties
		{
			private readonly ManagementBaseObject PrivateLateBoundObject;

			public ManagementSystemProperties(ManagementBaseObject ManagedObject)
			{
				PrivateLateBoundObject = ManagedObject;
			}

			[Browsable(true)]
			public int GENUS
			{
				get { return ((int) (PrivateLateBoundObject["__GENUS"])); }
			}

			[Browsable(true)]
			public string CLASS
			{
				get { return ((string) (PrivateLateBoundObject["__CLASS"])); }
			}

			[Browsable(true)]
			public string SUPERCLASS
			{
				get { return ((string) (PrivateLateBoundObject["__SUPERCLASS"])); }
			}

			[Browsable(true)]
			public string DYNASTY
			{
				get { return ((string) (PrivateLateBoundObject["__DYNASTY"])); }
			}

			[Browsable(true)]
			public string RELPATH
			{
				get { return ((string) (PrivateLateBoundObject["__RELPATH"])); }
			}

			[Browsable(true)]
			public int PROPERTY_COUNT
			{
				get { return ((int) (PrivateLateBoundObject["__PROPERTY_COUNT"])); }
			}

			[Browsable(true)]
			public string[] DERIVATION
			{
				get { return ((string[]) (PrivateLateBoundObject["__DERIVATION"])); }
			}

			[Browsable(true)]
			public string SERVER
			{
				get { return ((string) (PrivateLateBoundObject["__SERVER"])); }
			}

			[Browsable(true)]
			public string NAMESPACE
			{
				get { return ((string) (PrivateLateBoundObject["__NAMESPACE"])); }
			}

			[Browsable(true)]
			public string PATH
			{
				get { return ((string) (PrivateLateBoundObject["__PATH"])); }
			}
		}
	}
}
#endregion