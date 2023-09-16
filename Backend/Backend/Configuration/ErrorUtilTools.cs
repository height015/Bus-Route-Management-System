using System;
namespace Configuration;

public static class ErrorUtilTools
{
	public static void LogErr(string trace, string errSource, string msg)
	{
		try
		{
			string localDate = GetLocalDate();
			string localTime = GetLocaTime();
string text = GetBasePath() + "AppErrorLogging\\ErrorLog\\Logs.txt";
			if (File.Exists(text))
			{
				try
				{
					FileInfo fileInfo = new FileInfo(text);
					if (fileInfo.Length >> 10 >= 10)
					{
						string text2 = text.Substring(0, text.Length - 4);
						text2 = text2 + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + fileInfo.Extension;
						fileInfo.CopyTo(text2);
						File.WriteAllText(text, string.Empty);
					}
				}
				catch (Exception)
				{

				}

                    StreamWriter streamWriter = new StreamWriter(text, append: true);
				string text3 = "\r\n";
				text3 += "<New Error>\r\n";
                    text3 += text3 + "[Time] :" + localDate + " -:- " + localTime + "\r\n";
                    text3 += text3 +"[Error Message] :" + msg + "\r\n";
                    text3 += text3 + "[Error Location] :" + trace + "\r\n";
                    text3 += text3 + "[Instance] :" + errSource + "\r\n";
                    text3 += "<New Error>\r\n";
				streamWriter.WriteLine(text3);
				streamWriter.Close();
                }
                else
                {
                    new FileStream(text, FileMode.CreateNew, FileAccess.ReadWrite).Close();
                    StreamWriter streamWriter2 = new StreamWriter(text, append: true);
                    string text4 = "\r\n";
                    text4 += "<New Error>\r\n";
                    text4 += text4 + "[Time] :" + localDate + " -:- " + localTime + "\r\n";
                    text4 += text4 + "[Error Message] :" + msg + "\r\n";
                    text4 += text4 + "[Error Location] :" + trace + "\r\n";
                    text4 += text4 + "[Instance] :" + errSource + "\r\n";
                    text4 += "<New Error>\r\n";
                    streamWriter2.WriteLine(text4);
                    streamWriter2.Close();
                }
            }
		catch (Exception)
		{

		}

		
	}

        internal static string GetBasePath()
        {
		AppDomain currentDomain = AppDomain.CurrentDomain;
		if (currentDomain.BaseDirectory.Contains("\\bin\\Debug\\netcoreapp2.2"))
		{
			return currentDomain.BaseDirectory.Replace("\\bin\\Debug\\netcoreapp2.2\\","");
            }
            if (currentDomain.BaseDirectory.Contains("\\bin\\Release\\netcoreapp2.2"))
            {
                return currentDomain.BaseDirectory.Replace("\\bin\\Release\\netcoreapp2.2\\", "");
            }

		return currentDomain.BaseDirectory;
        }

        private static string GetLocalDate()
        {
		try
		{
			return DateTime.Now.ToUniversalTime().AddHours(1.0).ToString("yyyy/MM/dd");
		}
		catch (Exception)
		{
			return DateTime.Now.ToString("yyyy/MM/dd");
		}
        }

        private static string GetLocaTime()
        {
            try
            {
                return DateTime.Now.ToUniversalTime().AddHours(1.0).ToString("hh:mm:ss");
            }
            catch (Exception)
            {
                return DateTime.Now.ToString("hh:mm:ss");
            }
        }
    }

