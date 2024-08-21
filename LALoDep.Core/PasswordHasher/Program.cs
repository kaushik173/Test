using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Core.PasswordHasher
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Enter password and press ENTER");

            var password = Console.ReadLine();

            var hashed = PasswordHash.CreateHash(password);

            Console.WriteLine(hashed);

           // System.Windows.Forms.Clipboard.SetText(hashed);

            //Console.WriteLine("Copied to Clipboard. Press any key to close...");

            Console.Read();
        }
    }
}
