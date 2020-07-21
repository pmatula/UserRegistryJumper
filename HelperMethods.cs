using System;
using System.Diagnostics;
using System.Security.Principal;

namespace UserRegistryJumper
{
    public static class HelperMethods
    {
        /*
         * Print error message to OutputDebugString (which can be captured by Dbgview.exe)
         * copy & paste from: https://nedbatchelder.com/blog/200503/c_and_outputdebugstring.html
         */
        static public void DebugOut(string msg)
        {
            StackTrace st = new StackTrace(false);
            string caller = st.GetFrame(1).GetMethod().Name;
            Debug.WriteLine("UserRegistryJumper - " + caller + ": " + msg);
        }

        /*
         * Get SID from username
         */
        static public String getSid(string username) {
            try
            {
                var account = new NTAccount(username);
                return account.Translate(typeof(SecurityIdentifier)).ToString();
            }
            catch (Exception e)
            {
                DebugOut(username + ": " + e.Message);
                return "notfound";
            }
        }
    }
}
