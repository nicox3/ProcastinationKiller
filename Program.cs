using System;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace ProcastinationKiller
{
    class Program
    {
        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        static private int targetTime = 0;
        static private int temptime = 0;
        static private string firstProcess;

        static void Main(string[] args)
        {
            GetUserInput();
        }

        static private void GetUserInput()
        {
            Console.WriteLine("Welcome to the ProcastinationKiller app, made by Nico, input the program that you want to block.\nRemember to Capital Letter the process and save all the work, because this program insta-kills the process\nIf you want to block more processes just open more instances of the program. \nThis is just an 0.1");
            Console.WriteLine();
            firstProcess = Console.ReadLine();
            Console.WriteLine($"You've selected {firstProcess}. \nFor how much time do you want to block {firstProcess}? (In hours)");
            string timeSelected = Console.ReadLine();

            if (timeSelected.Equals("")) { Console.WriteLine("You've selected nothing, please input a valid number."); }

            else
            {
                if (Int32.TryParse(timeSelected, out targetTime))
                {
                    targetTime = Int32.Parse(timeSelected);
                    targetTime *= 3_600_000;
                    Console.WriteLine($"You're going to block {firstProcess} for {timeSelected} hours, are you sure? (Yes) or (No)");
                }

                else { Console.WriteLine("That value is not valid, please enter a numeric one \nThe application will now die."); Thread.Sleep(2000); }

                string checkForUserResponse = Console.ReadLine();

                if (checkForUserResponse.Contains("y") || checkForUserResponse.Contains("Y"))
                {
                    Console.WriteLine($"Ok, {firstProcess} will be blocked for {timeSelected}, and the window will now hide,\nfor the purpose of being harder to stop, you must kill the process from the task manager");
                    Thread.Sleep(4500);
                    IntPtr hWnd = GetConsoleWindow();
                    if(hWnd != IntPtr.Zero)
                    {
                        ShowWindow(hWnd, 0);
                    }
                    ScanTheRam();
                }
                else if (checkForUserResponse.Contains("n") || checkForUserResponse.Contains("N"))
                {
                    Console.WriteLine("Ok, come here when you want to, If you don't care, I won't either, happy procastinating.");
                    Thread.Sleep(5000);
                }         
            }
        }

        static private void ScanTheRam()
        {
            if (targetTime >= temptime)
            {
                foreach (Process proc in Process.GetProcesses())
                {
                    if (proc.ProcessName.Contains(firstProcess))
                    {
                        try
                        {
                            proc.Kill();
                            Console.WriteLine(proc);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }             
                temptime += 5000;
                Thread.Sleep(5000);
                ScanTheRam();
            }
        }
    }
}
