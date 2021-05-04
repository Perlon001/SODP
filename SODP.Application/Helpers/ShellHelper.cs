using System;
using System.Diagnostics;
using System.IO;

public static class ShellHelper
{
    private static string commandProcessor = getOsPlatform();
    public static string CommandProcessor
    {
        get
        {
            return commandProcessor;
        }
        set
        {
            commandProcessor = value;
        }
    }

    public static string RunShell(this string cmd)
    {
        Process p = new Process();
        
        p.StartInfo.FileName = commandProcessor;               // for linux  "/bin/bash", for windows  "cmd.exe"
        p.StartInfo.Arguments = cmd;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.Start();

        string output = commandProcessor+cmd+"\n "+p.StandardOutput.ReadToEnd() +"\n ";
        p.WaitForExit();

        return output;
    }

    private static string getOsPlatform()
    {
        string result;
        var os = Environment.OSVersion;
       
        switch (os.Platform)
        {
            case PlatformID.Win32NT:
                result = "cmd";
                break;
            case PlatformID.Unix:
                result = "/bin/bash";
                break;
            default:
                result = os.VersionString;
                break;
        }
        return result;

    }
}
