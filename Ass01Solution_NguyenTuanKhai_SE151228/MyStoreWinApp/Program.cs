using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyStoreWinApp
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frmLogin loginForm = new frmLogin();
            Application.Run(loginForm);
            if (loginForm.UserSuccessfullyAuthenticated)
            {
                if (loginForm.isAdmin == true)
                {
                    Application.Run(new frmMemberManagement() { isAdmin = true });
                }
                else
                {
                    Application.Run(new frmMemberManagement() { isAdmin = false, id=loginForm.id });
                }
            }
        }
    }
}
