/*
    ##          ##      ##     ##           ##      ##      ##########      ##        ##        ##########          ################      ##        ##      ################        ###########     ##########      ##                ##        ################        ##                ##        ##########
    ##          ##      ##     ##           ##      ##          ##          ##        ##        ##                  ##                    ##        ##      ##            ##        ##              ##              ####              ##        ##            ##        ####              ##        ##        
    ##            ##  ##       ##             ##  ##            ##          ##        ##        ##                  ##                    ##        ##      ##            ##        ##              ##              ##  ##            ##        ##            ##        ##  ##            ##        ##        
    ##              ##         ##               ##              ##          ##        ##        ##                  ##                    ##        ##      ##            ##        ##              ##              ##    ##          ##        ##            ##        ##    ##          ##        ##        
    ##              ##         ##               ##              ##          ############        ########            ##                    ############      ##            ##        ###########     ########        ##      ##        ##        ##            ##        ##      ##        ##        ########  
    ##              ##         ##               ##              ##          ############        ########            ##                    ############      ##            ##        ###########     ########        ##        ##      ##        ##            ##        ##        ##      ##        ########  
    ##              ##         ##               ##              ##          ##        ##        ##                  ##                    ##        ##      ##            ##                 ##     ##              ##          ##    ##        ##            ##        ##          ##    ##        ##        
    ##              ##         ##               ##              ##          ##        ##        ##                  ##                    ##        ##      ##            ##                 ##     ##              ##            ##  ##        ##            ##        ##            ##  ##        ##        
    ##              ##         ##               ##              ##          ##        ##        ##                  ##                    ##        ##      ##            ##                 ##     ##              ##              ####        ##            ##        ##              ####        ##        
    ##########      ##         ##########       ##              ##          ##        ##        ##########          ################      ##        ##      ################        ###########     ##########      ##                ##        ################        ##                ##        ##########

    Script by lylythechosenone
*/

using System;
using System.IO;
using System.Management;
using Newtonsoft.Json;

class USBDrive{
    private static void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
    {

        Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));

        if (Directory.Exists(config.driveLetter + ":\\") && Directory.Exists(config.pathToCopy)) {
            if (config.clear) {
                Clear(config.driveLetter + ":\\");
            }
            Copy(config.pathToCopy, config.driveLetter + ":\\");
            Console.WriteLine("Done");
        }
    }          

    static void Main()
    {
        WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
        ManagementEventWatcher insertWatcher = new ManagementEventWatcher(insertQuery);
        insertWatcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
        insertWatcher.Start();

        Console.ReadKey();
    }

    public static void Copy(string sourceDirectory, string targetDirectory)
    {
        var diSource = new DirectoryInfo(sourceDirectory);
        var diTarget = new DirectoryInfo(targetDirectory);
 
        CopyAll(diSource, diTarget);
    }
 
    public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        Directory.CreateDirectory(target.FullName);
 
        // Copy each file into the new directory.
        foreach (FileInfo fi in source.GetFiles())
        {
            Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
            fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
        }
 
        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
            CopyAll(diSourceSubDir, nextTargetSubDir);
        }
    }

    public static void Clear(string path) {
        System.IO.DirectoryInfo di = new DirectoryInfo(path);

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete(); 
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true); 
        }
    }
}

public class Config
{
    public string driveLetter { get; set; }
    public string pathToCopy { get; set; }
    public bool clear { get; set; }
}