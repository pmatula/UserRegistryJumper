using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace UserRegistryJumper
{
    public static class BusinessLogic
    {
        public static ObservableCollection<String> GetUsers()
        {

            // Enumerate all users
            string[] subdirs = Directory.GetDirectories(Path.GetPathRoot(Environment.SystemDirectory) + "Users")
                            .Select(Path.GetFileName)
                            .ToArray();

            //TODO: Better way to handle that? 
            //Reason: Two lists are necessary because I cant go through the list and remove items in the same list
            var usersListLoop = new ObservableCollection<String>(subdirs);
            var usersList = new ObservableCollection<String>(subdirs);

            //Go through the whole list and check if all users are convertable to SIDs.
            //If its not - remove the user. 
            foreach (string user in usersListLoop)
            {
                if(HelperMethods.getSid(user) == "notfound")
                {
                    usersList.Remove(user); 
                }
            }
            return usersList; 
        }

        public static ObservableCollection<String> GetRegistry()
        {
            //try to read all registry bookmarks from registryhives.txt (harcoded)
            var registryList = new ObservableCollection<String>();
            try
            {
                var lines = File.ReadLines("registryhives.txt");
                foreach (var line in lines)
                {
                    registryList.Add(line);
                }
            }
            catch (Exception e)
            {
                HelperMethods.DebugOut(e.Message);
                registryList.Add(".");
            }
            return registryList;
        }

        public static void DoJump(String username, String registry)
        {
            //retrieve the SID 
            var usernameSid = HelperMethods.getSid(username);

            //remove everything after "(" - at least try it
            try
            {
                registry = registry.Substring(0,registry.LastIndexOf('(') -1);
            }
            catch (Exception e)
            {
                HelperMethods.DebugOut(registry + "- registry string without comment: " + e.Message); 
            }

            //the actual jump phase
            //setting up everything
            string RegKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit";
            string RegValueName = "LastKey";
            string registryUsed = ""; 

            //deal with the special "."
            if (registry == ".")
            {
                registry = ""; 
            }
            else
            {
                //need to add the "\" - otherwise it won't work
                registryUsed += "\\" + registry; 
            }
            string RegValue = "HKEY_USERS\\" + usernameSid + registryUsed;

            //set the new value
            Microsoft.Win32.Registry.SetValue(RegKey, RegValueName, RegValue);

            //jump
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "regedit.exe",
                    Arguments = "/m"
                }
            };
            process.Start();
        }

    }
}
