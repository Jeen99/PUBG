using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalClient
{
    /// <summary>
	/// Сохраняет и загружает авторизационне данные
	/// </summary>
	static class AuthorizationData
    {
		private static readonly string pathData = @"AuthorizationData.txt";

		public static bool LoadAuthorizationData(out string login, out string pass)
		{
			login = string.Empty;
			pass = string.Empty;

			try
			{
				using (StreamReader sr = new StreamReader(pathData, System.Text.Encoding.Default))
				{
					login = sr.ReadLine();
					pass = sr.ReadLine();
				}
				return true;
			}
			catch (FileNotFoundException ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				return false;
			}
		}

		public static void SaveAuthorizationData(string login, string pass)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter(pathData, false, System.Text.Encoding.Default))
				{
					sw.WriteLine(login);
					sw.WriteLine(pass);
				}
			}
			catch (FileNotFoundException ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		public static void DeleteAuthorizationData()
		{
			try
			{
				File.Delete(pathData);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}
	}
}
