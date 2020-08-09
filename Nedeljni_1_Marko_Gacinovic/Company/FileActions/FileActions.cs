using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.FileActions
{
    class FileActions
    {
        /// <summary>
        /// Singleton design pattern
        /// </summary>
        private static FileActions instance;

        public static FileActions Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FileActions();
                }
                return instance;
            }
        }

        // static fields for logging actions to the file
        public static string path = @"../../FileActions/Actions.txt";
        public static List<string> actions = new List<string>();


        private FileActions()
        {

        }

        /// <summary>
        /// method for writing action of adding user to the file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="list"></param>
        /// <param name="datetime"></param>
        /// <param name="user"></param>
        public void Adding(string path, List<string> list, string type, string a)
        {
            list.Add(DateTime.Now + " - created " + type + ": " + a);
            File.AppendAllLines(path, list);

            list.Clear();
        }

        /// <summary>
        /// method for writing action of deleting user to the file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="list"></param>
        /// <param name="datetime"></param>
        /// <param name="user"></param>
        public void Deleting(string path, List<string> list, string type, string a)
        {
            list.Add(DateTime.Now + " - deleted " + type + ": " + a);
            File.AppendAllLines(path, list);

            list.Clear();
        }

        /// <summary>
        /// method for writing action of editing user to the file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="list"></param>
        /// <param name="datetime"></param>
        /// <param name="user"></param>
        public void Editing(string path, List<string> list, string type, string a)
        {
            list.Add(DateTime.Now + " - edited " + type + ": " + a);
            File.AppendAllLines(path, list);

            list.Clear();
        }
    }
}
