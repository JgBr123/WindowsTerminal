using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Timers;

namespace Terminal
{
    class Program
    {
        public static string Path = @$"C:\Users\{Environment.UserName}";
        static void Main()
        {
            Console.Clear();
            Console.Title = "Windows Terminal [v1.0]";

            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string color;

            try { color = File.ReadAllText(appdataPath + "\\WindowsTerminal\\color.txt"); }
            catch { color = ""; }

            if (color == "black") Console.ForegroundColor = ConsoleColor.Black;
            else if (color == "blue") Console.ForegroundColor = ConsoleColor.Blue;
            else if (color == "cyan") Console.ForegroundColor = ConsoleColor.Cyan;
            else if (color == "darkblue") Console.ForegroundColor = ConsoleColor.DarkBlue;
            else if (color == "darkcyan") Console.ForegroundColor = ConsoleColor.DarkCyan;
            else if (color == "darkgray") Console.ForegroundColor = ConsoleColor.DarkGray;
            else if (color == "darkgreen") Console.ForegroundColor = ConsoleColor.DarkGreen;
            else if (color == "darkmagenta") Console.ForegroundColor = ConsoleColor.DarkMagenta;
            else if (color == "darkred") Console.ForegroundColor = ConsoleColor.DarkRed;
            else if (color == "darkyellow") Console.ForegroundColor = ConsoleColor.DarkYellow;
            else if (color == "gray") Console.ForegroundColor = ConsoleColor.Gray;
            else if (color == "green") Console.ForegroundColor = ConsoleColor.Green;
            else if (color == "magenta") Console.ForegroundColor = ConsoleColor.Magenta;
            else if (color == "red") Console.ForegroundColor = ConsoleColor.Red;
            else if (color == "white") Console.ForegroundColor = ConsoleColor.White;
            else if (color == "yellow") Console.ForegroundColor = ConsoleColor.Yellow;
            else Console.ForegroundColor = ConsoleColor.DarkCyan;
            Terminal();
        }
        static void Terminal()
        {
            Console.WriteLine("Windows Terminal [version 1.0]");
            Console.WriteLine("For more information and usage guide, check github.com/JgBr123");
            while (true)
            {
                Console.WriteLine();
                Console.Write($"{Path}>");
                Commands(Console.ReadLine());
            }
        }
        static void Commands(string command)
        {
            var args = command.Split(" ").ToList();

            try { args[1].ToString(); } catch { args.Add(""); }
            try { args[2].ToString(); } catch { args.Add(""); }

            //CMD
            if (args[0] == "cmd")
            {
                string commandCmd = "";
                int i = 1;

                foreach (string str in args)
                {
                    if (str != "cmd") commandCmd += str + " ";
                    i++;
                }
                RunCmd(commandCmd);
            }
            //CLS - CLEAR
            else if (args[0] == "cls" || args[0] == "clear")
            {
                Console.Clear();
            }
            //LS - LIST
            else if (args[0] == "ls" || args[0] == "list")
            {
                DirectoryInfo folder = new DirectoryInfo(Path);
                DriveInfo drive = new DriveInfo(Path.Substring(0, 2));

                Console.WriteLine();

                int folders = 0, files = 0;
                double bused = 0, bfree = drive.AvailableFreeSpace;

                foreach (var file in folder.GetDirectories())
                {
                    Console.WriteLine($"{file.LastWriteTime}   <DIR>   {file.Name}");
                    folders++;
                }

                foreach (var file in folder.GetFiles())
                {
                    Console.WriteLine($"{file.LastWriteTime}           {file.Name} ({file.Length}B)");
                    bused += file.Length;
                    files++;
                }

                Console.WriteLine($"\n              {folders} folders, {files} files.");
                Console.WriteLine($"              {bused} bytes used, {bfree} bytes free.");
            }
            //CD
            else if (args[0] == "cd")
            {
                if (args[1] == "..")
                {
                    if (Path.Length != 3)
                    {
                        string newPath = "";

                        var folders = Path.Split(@"\").ToList();
                        folders.RemoveAt(folders.Count - 1);

                        foreach (string folder in folders)
                        {
                            newPath += folder + @"\";
                        }
                        newPath = newPath.Substring(0, newPath.Count() - 1);

                        if (newPath.Length >= 3) Path = newPath;
                        else Path = newPath + @"\";
                    }
                }
                else if (Directory.Exists(args[1]))
                {
                    if (args[1] != "." && args[1] != @"\" && args[1] != "/")
                    {
                        string newPath = args[1].Replace(@"\\", @"\");
                        if (newPath.Length < 3) newPath += @"\";

                        newPath = newPath[0].ToString().ToUpper() + newPath.Substring(1);
                        Path = newPath;

                        while (Path.Contains(@"\\")) Path = Path.Replace(@"\\", @"\");

                        if (Path.EndsWith(@"\")) Path = Path.Remove(Path.Length - 1);
                    }
                    else if (args[1] == @"\" || args[1] == "/")
                    {
                        Path = Path.Substring(0, 3);
                    }
                }
                else if (Directory.Exists(Path + @"\" + args[1]))
                {
                    string newPath = Path + @"\" + args[1];

                    Path = newPath.Replace(@"\\", @"\");

                    while (Path.Contains(@"\\")) Path = Path.Replace(@"\\", @"\");

                    if (Path.EndsWith(@"\")) Path = Path.Remove(Path.Length - 1);
                }
                else Console.WriteLine("This is not a valid path.");
            }
            //OPEN
            else if (args[0] == "open")
            {
                string filePath = "";

                if (File.Exists(args[1]))
                {
                    if (args[1] != "." && args[1] != @"\" && args[1] != "/" && args[1] != "..")
                    {
                        string newPath = args[1].Replace(@"\\", @"\");
                        if (newPath.Length < 3) newPath += @"\";

                        newPath = newPath[0].ToString().ToUpper() + newPath.Substring(1);
                        filePath = newPath;

                        while (filePath.Contains(@"\\")) filePath = filePath.Replace(@"\\", @"\");

                        if (filePath.EndsWith(@"\")) filePath = filePath.Remove(filePath.Length - 1);

                        try { RunCmd(filePath); }
                        catch { Console.WriteLine("Couldn't open this file."); }
                    }
                }
                else if (File.Exists(Path + @"\" + args[1]))
                {
                    string newPath = Path + @"\" + args[1];

                    filePath = newPath.Replace(@"\\", @"\");

                    while (filePath.Contains(@"\\")) filePath = filePath.Replace(@"\\", @"\");

                    if (filePath.EndsWith(@"\")) filePath = filePath.Remove(filePath.Length - 1);

                    try { RunCmd(filePath); }
                    catch { Console.WriteLine("Couldn't open this file."); }
                }
                else Console.WriteLine("This is not a valid file path.");
            }
            //RMDIR
            else if (args[0] == "rmdir" || args[0] == "removedir" || args[0] == "removedirectory")
            {
                string filePath = "";

                if (Directory.Exists(args[1]))
                {
                    if (args[1] != "." && args[1] != @"\" && args[1] != "/" && args[1] != "..")
                    {
                        if (args[1] != "")
                        {
                            string newPath = args[1].Replace(@"\\", @"\");
                            if (newPath.Length < 3) newPath += @"\";

                            newPath = newPath[0].ToString().ToUpper() + newPath.Substring(1);
                            filePath = newPath;

                            while (filePath.Contains(@"\\")) filePath = filePath.Replace(@"\\", @"\");

                            if (filePath.EndsWith(@"\")) filePath = filePath.Remove(filePath.Length - 1);

                            Console.WriteLine($"Are you sure? This action will delete {filePath} and all of the files and subfolders inside. (Y/N)");

                            switch (Console.ReadLine().ToUpper())
                            {
                                case "Y":
                                    try { Directory.Delete(filePath, true); }
                                    catch { Console.WriteLine("Couldn't delete this folder."); }
                                    break;
                                default:
                                    return;
                            }
                        }
                        else Console.WriteLine("You need to provide the path of the folder you want to delete.");
                    }
                }
                else if (Directory.Exists(Path + @"\" + args[1]))
                {
                    if (args[1] != "")
                    {
                        string newPath = Path + @"\" + args[1];

                        filePath = newPath.Replace(@"\\", @"\");

                        while (filePath.Contains(@"\\")) filePath = filePath.Replace(@"\\", @"\");

                        if (filePath.EndsWith(@"\")) filePath = filePath.Remove(filePath.Length - 1);

                        Console.WriteLine($"Are you sure? This action will delete {filePath} and all of the files and subfolders inside. (Y/N)");

                        switch (Console.ReadLine().ToUpper())
                        {
                            case "Y":
                                try { Directory.Delete(filePath, true); }
                                catch { Console.WriteLine("Couldn't delete this folder."); }
                                break;
                            default:
                                return;
                        }
                    }
                    else Console.WriteLine("You need to provide the path of the folder you want to delete.");
                }
                else Console.WriteLine("This is not a valid folder path.");
            }
            //MKDIR
            else if (args[0] == "mkdir" || args[0] == "makedir" || args[0] == "makedirectory")
            {
                string pathToWrite = "";

                if (!args[1].Contains(@"\") && !args[1].Contains("/") && !args[1].Contains(":") && !args[1].Contains("*") && !args[1].Contains("*") && !args[1].Contains("\"") && !args[1].Contains("<") && !args[1].Contains(">") && !args[1].Contains("|"))
                {
                    if (args[1] != "")
                    {
                        pathToWrite = Path + @"\" + args[1];

                        while (pathToWrite.Contains(@"\\")) pathToWrite = pathToWrite.Replace(@"\\", @"\");

                        try
                        {
                            Directory.CreateDirectory(pathToWrite);
                            Console.WriteLine("Folder created successfully.");
                        }
                        catch { Console.WriteLine("Couldn't create this folder."); }
                    }
                    else Console.WriteLine("You need to provide the name of the folder you want to create.");
                }
                else Console.WriteLine("Folder name can't contain the following characters: \\/:*\"?<>|");
            }
            //CAT
            else if (args[0] == "cat")
            {
                string filePath;

                string mode = "read";
                try { if (args[1][0].ToString() == ">") mode = "write"; } catch { mode = "read"; }
                if (mode == "write")
                {
                    //Write mode

                    string pathToWrite = "";
                    string textFile = "";

                    if (!args[1].Contains(@"\") && !args[1].Contains("/") && !args[1].Contains(":") && !args[1].Contains("*") && !args[1].Contains("*") && !args[1].Contains("\"") && !args[1].Contains("<") && !args[1].Substring(1).Contains(">") && !args[1].Contains("|"))
                    {
                        pathToWrite = Path + @"\" + args[1].Substring(1);

                        if (pathToWrite.EndsWith(".txt"))
                        {
                            while (pathToWrite.Contains(@"\\")) pathToWrite = pathToWrite.Replace(@"\\", @"\");

                            Console.WriteLine($"\nYou are editing \"{pathToWrite}\".");
                            Console.WriteLine("Use /s to save and close the file, or /c to cancel.\n");

                            while (true)
                            {
                                string line = Console.ReadLine();
                                if (line == "/s")
                                {
                                    try
                                    {
                                        File.WriteAllText(pathToWrite, textFile);
                                        Console.WriteLine("File saved and closed successfully.");
                                    }
                                    catch { Console.WriteLine("Couldn't create this file."); }
                                    break;
                                }
                                if (line == "/c") { Console.WriteLine("File was canceled successfully."); break; }
                                textFile += line + "\n";
                            }
                        }
                        else Console.WriteLine("Text files must finish with \".txt\" extension.");
                    }
                    else Console.WriteLine("File name can't contain the following characters: \\/:*\"?<>|");
                }
                else
                {
                    //Read mode
                    if (File.Exists(args[1]))
                    {
                        if (args[1] != "." && args[1] != @"\" && args[1] != "/" && args[1] != "..")
                        {
                            string newPath = args[1].Replace(@"\\", @"\");
                            if (newPath.Length < 3) newPath += @"\";

                            newPath = newPath[0].ToString().ToUpper() + newPath.Substring(1);
                            filePath = newPath;

                            while (filePath.Contains(@"\\")) filePath = filePath.Replace(@"\\", @"\");

                            if (filePath.EndsWith(@"\")) filePath = filePath.Remove(filePath.Length - 1);

                            try { Console.WriteLine("\n" + File.ReadAllText(filePath)); }
                            catch { Console.WriteLine("Couldn't read this file."); }
                        }
                    }
                    else if (File.Exists(Path + @"\" + args[1]))
                    {
                        string newPath = Path + @"\" + args[1];

                        filePath = newPath.Replace(@"\\", @"\");

                        while (filePath.Contains(@"\\")) filePath = filePath.Replace(@"\\", @"\");

                        if (filePath.EndsWith(@"\")) filePath = filePath.Remove(filePath.Length - 1);

                        try { Console.WriteLine("\n" + File.ReadAllText(filePath)); }
                        catch { Console.WriteLine("Couldn't read this file."); }
                    }
                    else Console.WriteLine("This is not a valid text file path.");
                }
            }
            //RM
            else if (args[0] == "rm" || args[0] == "remove" || args[0] == "del" || args[0] == "delete")
            {
                string filePath = "";

                if (File.Exists(args[1]))
                {
                    if (args[1] != "." && args[1] != @"\" && args[1] != "/" && args[1] != "..")
                    {
                        if (args[1] != "")
                        {
                            string newPath = args[1].Replace(@"\\", @"\");
                            if (newPath.Length < 3) newPath += @"\";

                            newPath = newPath[0].ToString().ToUpper() + newPath.Substring(1);
                            filePath = newPath;

                            while (filePath.Contains(@"\\")) filePath = filePath.Replace(@"\\", @"\");

                            if (filePath.EndsWith(@"\")) filePath = filePath.Remove(filePath.Length - 1);

                            Console.WriteLine($"Are you sure? This action will delete {filePath} and can't be reversed. (Y/N)");

                            switch (Console.ReadLine().ToUpper())
                            {
                                case "Y":
                                    try { File.Delete(filePath); }
                                    catch { Console.WriteLine("Couldn't delete this file."); }
                                    break;
                                default:
                                    return;
                            }
                        }
                        else Console.WriteLine("You need to provide the path of the file you want to delete.");
                    }
                }
                else if (File.Exists(Path + @"\" + args[1]))
                {
                    if (args[1] != "")
                    {
                        string newPath = Path + @"\" + args[1];

                        filePath = newPath.Replace(@"\\", @"\");

                        while (filePath.Contains(@"\\")) filePath = filePath.Replace(@"\\", @"\");

                        if (filePath.EndsWith(@"\")) filePath = filePath.Remove(filePath.Length - 1);

                        Console.WriteLine($"Are you sure? This action will delete {filePath} and can't be reversed. (Y/N)");

                        switch (Console.ReadLine().ToUpper())
                        {
                            case "Y":
                                try { File.Delete(filePath); }
                                catch { Console.WriteLine("Couldn't delete this file."); }
                                break;
                            default:
                                return;
                        }
                    }
                    else Console.WriteLine("You need to provide the path of the file you want to delete.");
                }
                else Console.WriteLine("This is not a valid file path.");
            }
            //GET
            else if (args[0] == "get")
            {
                WebClient wc = new WebClient();

                string pathDownload = "";
                double fileSize;

                if (!args[2].Contains(@"\") && !args[2].Contains("/") && !args[2].Contains(":") && !args[2].Contains("*") && !args[2].Contains("*") && !args[2].Contains("\"") && !args[2].Contains("<") && !args[2].Contains(">") && !args[2].Contains("|"))
                {
                    if (args[1] != "")
                    {
                        if (args[2] != "")
                        {
                            pathDownload = Path + @"\" + args[2];

                            Stopwatch sw = new Stopwatch();

                            while (pathDownload.Contains(@"\\")) pathDownload = pathDownload.Replace(@"\\", @"\");

                            try
                            {
                                Console.WriteLine("Downloading file...");
                                sw.Start();
                                wc.DownloadFile(args[1], args[2]);
                                sw.Stop();
                                fileSize = new System.IO.FileInfo(args[2]).Length;
                                File.Move(args[2], pathDownload);
                                Console.WriteLine("File downloaded successfully.");
                                Console.WriteLine($"Size: {Math.Round(Decimal.Divide((decimal)fileSize, 1024))} KB | Time: {Math.Round(Decimal.Divide(sw.ElapsedMilliseconds, 1000), 2)} seconds");
                            }
                            catch { Console.WriteLine("Couldn't download this file. Check if URL link is not misspelled."); }
                        }
                        else Console.WriteLine("You need to provide a name to the file.");
                    }
                    else Console.WriteLine("Type a URL link to download the file.");
                }
                else Console.WriteLine("File name can't contain the following characters: \\/:*\"?<>|");
            }
            //MV
            else if (args[0] == "mv" || args[0] == "move")
            {
                string newFilePath = args[2];

                while (newFilePath.Contains(@"\\")) newFilePath = newFilePath.Replace(@"\\", @"\");
                if (newFilePath.EndsWith(@"\")) newFilePath = newFilePath.Remove(newFilePath.Length - 1);

                if (File.Exists(newFilePath)) { Console.WriteLine("File output already exist."); return; }
                else
                {
                    newFilePath = Path + @"\" + args[2];

                    while (newFilePath.Contains(@"\\")) newFilePath = newFilePath.Replace(@"\\", @"\");
                    if (newFilePath.EndsWith(@"\")) newFilePath = newFilePath.Remove(newFilePath.Length - 1);

                    if (File.Exists(newFilePath)) { Console.WriteLine("File output already exist."); return; }
                }

                RunCmd($"move {args[1]} {args[2]}",true);

                newFilePath = args[2];

                while (newFilePath.Contains(@"\\")) newFilePath = newFilePath.Replace(@"\\", @"\");
                if (newFilePath.EndsWith(@"\")) newFilePath = newFilePath.Remove(newFilePath.Length - 1);

                if (File.Exists(newFilePath)) { Console.WriteLine("File moved successfully."); }
                else
                {
                    newFilePath = Path + @"\" + args[2];

                    while (newFilePath.Contains(@"\\")) newFilePath = newFilePath.Replace(@"\\", @"\");
                    if (newFilePath.EndsWith(@"\")) newFilePath = newFilePath.Remove(newFilePath.Length - 1);

                    if (File.Exists(newFilePath)) { Console.WriteLine("File moved successfully."); }
                    else { Console.WriteLine("Couldn't move file. Check if paths are not misspelled."); }
                }
            }
            //CP
            else if (args[0] == "cp" || args[0] == "copy")
            {
                string newFilePath = args[2];

                while (newFilePath.Contains(@"\\")) newFilePath = newFilePath.Replace(@"\\", @"\");
                if (newFilePath.EndsWith(@"\")) newFilePath = newFilePath.Remove(newFilePath.Length - 1);

                if (File.Exists(newFilePath)) { Console.WriteLine("File output already exist."); return; }
                else
                {
                    newFilePath = Path + @"\" + args[2];

                    while (newFilePath.Contains(@"\\")) newFilePath = newFilePath.Replace(@"\\", @"\");
                    if (newFilePath.EndsWith(@"\")) newFilePath = newFilePath.Remove(newFilePath.Length - 1);

                    if (File.Exists(newFilePath)) { Console.WriteLine("File output already exist."); return; }
                }

                RunCmd($"copy {args[1]} {args[2]}", true);

                newFilePath = args[2];

                while (newFilePath.Contains(@"\\")) newFilePath = newFilePath.Replace(@"\\", @"\");
                if (newFilePath.EndsWith(@"\")) newFilePath = newFilePath.Remove(newFilePath.Length - 1);

                if (File.Exists(newFilePath)) { Console.WriteLine("File copied successfully."); }
                else
                {
                    newFilePath = Path + @"\" + args[2];

                    while (newFilePath.Contains(@"\\")) newFilePath = newFilePath.Replace(@"\\", @"\");
                    if (newFilePath.EndsWith(@"\")) newFilePath = newFilePath.Remove(newFilePath.Length - 1);

                    if (File.Exists(newFilePath)) { Console.WriteLine("File copied successfully."); }
                    else { Console.WriteLine("Couldn't copy file. Check if paths are not misspelled."); }
                }
            }
            //COLORSET
            else if (args[0] == "colorset")
            {
                string color = args[1].ToLower();

                if (color == "default") Console.ForegroundColor = ConsoleColor.DarkCyan;
                else if (color == "black") Console.ForegroundColor = ConsoleColor.Black;
                else if (color == "blue") Console.ForegroundColor = ConsoleColor.Blue;
                else if (color == "cyan") Console.ForegroundColor = ConsoleColor.Cyan;
                else if (color == "darkblue") Console.ForegroundColor = ConsoleColor.DarkBlue;
                else if (color == "darkcyan") Console.ForegroundColor = ConsoleColor.DarkCyan;
                else if (color == "darkgray") Console.ForegroundColor = ConsoleColor.DarkGray;
                else if (color == "darkgreen") Console.ForegroundColor = ConsoleColor.DarkGreen;
                else if (color == "darkmagenta") Console.ForegroundColor = ConsoleColor.DarkMagenta;
                else if (color == "darkred") Console.ForegroundColor = ConsoleColor.DarkRed;
                else if (color == "darkyellow") Console.ForegroundColor = ConsoleColor.DarkYellow;
                else if (color == "gray") Console.ForegroundColor = ConsoleColor.Gray;
                else if (color == "green") Console.ForegroundColor = ConsoleColor.Green;
                else if (color == "magenta") Console.ForegroundColor = ConsoleColor.Magenta;
                else if (color == "red") Console.ForegroundColor = ConsoleColor.Red;
                else if (color == "white") Console.ForegroundColor = ConsoleColor.White;
                else if (color == "yellow") Console.ForegroundColor = ConsoleColor.Yellow;
                else { Console.WriteLine("Color is invalid, try again with supported colors."); return; }

                string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                Directory.CreateDirectory(appdataPath + "\\WindowsTerminal");
                File.WriteAllText(appdataPath + "\\WindowsTerminal\\color.txt", color);
            }
            //SHUTDOWN
            else if (args[0] == "shutdown")
            {
                RunCmd("shutdown /s");
            }
            //RESTART
            else if (args[0] == "restart")
            {
                RunCmd("shutdown /r");
            }
            //LOGOFF
            else if (args[0] == "logoff")
            {
                RunCmd("shutdown /l");
            }
            //SLEEP
            else if (args[0] == "sleep")
            {
                RunCmd("shutdown /h");
            }
            //EXIT
            else if (args[0] == "exit")
            {
                System.Environment.Exit(0);
            }
            else //Fail command
            {
                if (args[0] != "") Console.WriteLine($"\"{args[0]}\" was not recognized. Check if you didn't misspell anything, or check the usage guide for more info.");
            }
        }
        static void RunCmd (string command, bool invisible=false)
        {
            //Execute cmd command
            var cmd = new Process();

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine($"cd {Path}");
            cmd.StandardInput.WriteLine($"{Path.Substring(0,2)}");
            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();

            List<string> lines = cmd.StandardOutput.ReadToEnd().Split("\n").ToList();

            for (int x = 0; x < 8; x++) try { lines.RemoveAt(0); } catch { }
            for (int x = 0; x < 2; x++) try { lines.RemoveAt(lines.Count - 1); } catch { }

            if (!invisible) lines.ForEach(Console.WriteLine);
        }
    }
}
