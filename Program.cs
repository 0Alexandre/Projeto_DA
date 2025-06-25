using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iTasks
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 1) Prepara a pasta de dados em %APPDATA%\iTasks
            var userData = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "iTasks"
            );
            Directory.CreateDirectory(userData);

            // 2) Diz ao .NET para usar essa pasta como |DataDirectory|
            AppDomain.CurrentDomain.SetData("DataDirectory", userData);

            // Depois, arranca a aplicação normalmente
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }
    }
}
