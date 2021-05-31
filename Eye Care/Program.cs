using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using Eye_Care;
using Eye_Care.Properties;

public class Form1 : System.Windows.Forms.Form
{
    public static string path = @"c:\temp\lockfile";
    [STAThread]
    static void Main()
    {
        // check if there's already an instance running
        if (File.Exists(path)) // if the lockfile exists, show message
        {
            MessageBox.Show("An instance of Eye care is already running");
            return;
        }
            
        File.Create(path).Close();
        Application.Run(new MyCustomApplicationContext());
    }

}


public class MyCustomApplicationContext : ApplicationContext
{
    private NotifyIcon trayIcon;
    private CustomTimer cTimer;

    public MyCustomApplicationContext()
    {
        // Initialize Tray Icon
        trayIcon = new NotifyIcon();
        trayIcon.Icon = Resources.eye;

        // create contextmenu and add items
        ContextMenu cm = new ContextMenu();
        cm.MenuItems.Add(new MenuItem("Pause", Control));
        cm.MenuItems.Add("-");
        cm.MenuItems.Add(new MenuItem("Exit", Exit));

        // add contextmenu to trayIcon and show it
        trayIcon.ContextMenu = cm;
        trayIcon.Visible = true;

        // start the timer
        cTimer = new CustomTimer(trayIcon, 20*60);

        // add event handler when the application exits
        Application.ApplicationExit += new EventHandler(this.onApplicationExit);
    }
    void onApplicationExit(object sender, EventArgs e)
    {
        // delete the lockfile
        File.Delete(Form1.path);
    }

    void Exit(object sender, EventArgs e)
    {
        // Hide tray icon, otherwise it will remain shown until user mouses over it
        trayIcon.Visible = false;

        Application.Exit();
    }

    void Control(object sender, EventArgs e)
    {
        // get the menuItem sender
        MenuItem menuItem = (MenuItem)sender;

        if (cTimer.timerIsCountingDown())
        {
            // pause timer
            cTimer.Pause();
            // update text
            menuItem.Text = "Resume";
            trayIcon.Text = "Eye Care. Paused";
        }
        else
        {
            // resume timer
            cTimer.Resume();
            // update text
            menuItem.Text = "Pause";
            trayIcon.Text = "Eye Care. " + cTimer.getRemainingMin() + " minutes remaining";
        }
    }
}
