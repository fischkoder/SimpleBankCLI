using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BankingSystem
{
    class MainEntrance
    {
        static void Main(String[] args) {
            Program sys = new Program();
            sys.Portal();
            
            if(sys.loginStatus == true){
                sys.Menu();
            }
        }
        

    }
}
