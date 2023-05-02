using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public static class SystemParameterPath
{
    public static string AppPath = System.AppDomain.CurrentDomain.BaseDirectory;

    public static string iniFile = Path.Combine(AppPath, "DBSetup.ini");

    public static string ConfigFilePath = Path.Combine(AppPath, "test.dat");

    public static string TcpIniFile = Path.Combine(AppPath, "tcp.ini");

}
public class ExternalFile
{
    /// <summary>
    ///  external ini.File 
    /// </summary>

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]//window API讀取ini檔
    private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

    public static iniFileItem GetAllProfileString(string lpAppName)
    {
        iniFileItem iniFile = new iniFileItem();
      //  iniFile.GetType().GetProperties()[1].SetValue(iniFile, "test2");
        foreach (var prop in iniFile.GetType().GetProperties())
        {
            prop.SetValue(iniFile, GetProfileString(lpAppName,prop.Name,$"{prop.Name}=ErrorString",SystemParameterPath.iniFile));
        }        
        return iniFile;
    }
    public static String GetProfileString(string lpAppName, string lpKeyName, string lpDefault, string iniPath)
    {
        StringBuilder sb = new StringBuilder(200);//存放連線重要資料
        GetPrivateProfileString(lpAppName, lpKeyName, lpDefault, sb, (uint)sb.Capacity, iniPath);
        //.capacity個體配置的記憶體可包含的最大字元數    
        return sb.ToString();
    }

    public static bool WriteProfileString(string lpAppName, string lpKeyName, string lpString, string iniPath)
    {
        return WritePrivateProfileString(lpAppName, lpKeyName, lpString, iniPath);
    }

    /// <summary>
    /// external dat.File 
    /// </summary>
    /// <returns></returns>
    public static DatFileItem GetSetting()
    {
        DatFileItem Model;
        if (!File.Exists(SystemParameterPath.ConfigFilePath))
        {
            Model = new DatFileItem();
        }
        else
        {
            //Read dat檔 給 serialSetting資料型態
            Model = ReadFromBinaryFile<DatFileItem>(SystemParameterPath.ConfigFilePath) ?? new DatFileItem();
        }
        return Model;
    }
    public static T ReadFromBinaryFile<T>(string filePath)
    {
        try
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
        catch
        {
            return default(T);
        }
    }   
    public static void SetSetting(DatFileItem serialSetting)
    {
        //寫入 dat檔
        WriteToBinaryFile<DatFileItem>(SystemParameterPath.ConfigFilePath, serialSetting);
    }

    public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
    {
        using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }
    }
}
[Serializable]
public class DatFileItem
{ 
    public string datA { get; set; }
    public string datB { get; set; }
    public string datC { get; set; }
}
[Serializable]
public class iniFileItem
{
    public string iniA { get; set; }
    public string iniB { get; set; }
    public string iniC { get; set; }
}

