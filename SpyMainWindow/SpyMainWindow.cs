using System;
using System.Windows;

namespace spymainwindow
{
    public static class SpyMainWindow
    {
        public static bool CloseWindow(Window window)
        {
            try
            {
                window.Hide();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public static bool ShowWindow(Window window)
        {
            try
            {
                window.Show();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}