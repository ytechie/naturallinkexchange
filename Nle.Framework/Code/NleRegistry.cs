using System;
using Microsoft.Win32;
using YTech.General;

namespace Nle
{
	/// <summary>
	/// Manages the registry key(s) that Natural Link Exchange applications use.
	/// </summary>
	public class NleRegistry
	{
		/// <summary>
		/// The root key that contains the registry keys and values used by Natural Link Exchange applications.
		/// </summary>
		public const string REGKEY_ROOT = "Software\\YTech\\NLE";
		/// <summary>
		/// The value name for the database connection string.
		/// </summary>
		public const string REGKEY_DB_CONNECTION_STRING = "DbConnectionString";

		/// <summary>
		/// Gets and sets the database connection string in the registry.
		/// </summary>
		public static string DbConnection
		{
			get { return RegistryUtilities.ReadRegistryKey(Registry.LocalMachine, REGKEY_ROOT, REGKEY_DB_CONNECTION_STRING); }
			set { RegistryUtilities.WriteRegistryKey(Registry.LocalMachine, REGKEY_ROOT, REGKEY_DB_CONNECTION_STRING, value); }
		}
	}
}
