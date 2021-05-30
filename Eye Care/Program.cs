using System;
using System.Drawing;
using System.Windows.Forms;
using Eye_Care;
using Eye_Care.Properties;

public class Form1 : System.Windows.Forms.Form
{
    [STAThread]
    static void Main()
    {
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

        ContextMenu cm = new ContextMenu();
        cm.MenuItems.Add(new MenuItem("Pause", Control));
        cm.MenuItems.Add("-");
        cm.MenuItems.Add(new MenuItem("Exit", Exit));

        trayIcon.ContextMenu = cm;
        trayIcon.Visible = true;


        cTimer = new CustomTimer(trayIcon, 20*60);
    }

    void Exit(object sender, EventArgs e)
    {
        // Hide tray icon, otherwise it will remain shown until user mouses over it
        trayIcon.Visible = false;

        Application.Exit();
    }

    void Control(object sender, EventArgs e)
    {
        // Change text the menuItem text
        MenuItem menuItem = (MenuItem)sender;

        if (cTimer.timerIsCountingDown())
        {
            cTimer.Pause();
            menuItem.Text = "Resume";
            trayIcon.Text = "Eye Care. Paused";
        }
        else
        {
            cTimer.Resume();
            menuItem.Text = "Pause";
            trayIcon.Text = "Eye Care. " + cTimer.getRemainingMin() + " minutes remaining";
        }
    }
}
