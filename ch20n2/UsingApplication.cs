using System;
using System.Windows.Forms;

namespace ch20n2;

public class UsingApplication : Form
{
    public static void Main(string[] args)
    {
       UsingApplication application = new UsingApplication();

        application.Click += new EventHandler(
            (sender, eventArgs) =>
            {
                Console.WriteLine("Closing Window...");
                Application.Exit();
            });

        Console.WriteLine("Starting Window Application...");
        application.Run(application);
        Console.WriteLine("Exiting Window Application...");
    }
}