using System;

namespace UpdateManager_for_DBeaver_Portable
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager app = new Manager();
            //TESTING PURPUSE
            app.CheckDBeaverStatus();
            app.CheckJavaStatus();
            if(app.isUpdateAvailable())
            {
                // update
            }
            app.StartProgram();

            /*
             * 
             * TO-DO:
             *  UNMERGE CHECK JAVA AND DOWNLOAD JAVA - NEEDED FOR OFFLINE START OF APPLICATION
             *  DOWNLOAD-EXTRACT OF NEW DBEAVER ZIP (DOWNLOAD-EXTRACT METHODS ARE READY, JUST NEED PERFORM UPDATE METHOD)
             */
        }
    }
}
