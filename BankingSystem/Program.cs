using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;

namespace BankingSystem
{
    class Program
    {
        String firstName;
        String lastName;
        String address;
        Int64 phone = 0;
        String email = "";
        string format = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        int initAccountNumber = 100000;
        decimal accountBalance = 0;
        String[] statement = new String[5];
        public Boolean loginStatus = false;

        //Interface of system portal
        public Boolean Portal() {
            int position = 15;
            int l_usrn = 5;
            int l_pwd = 6;
            String usr;
            String pwd;
            int i = 0;

            Console.WriteLine("╔═════════════════════════════════════════╗");
            Console.WriteLine("║    WELCOME TO SIMPLE BANKING SYSTEM     ║");
            Console.WriteLine("╠─────────────────────────────────────────╣");
            Console.WriteLine("║              LOGIN TO START             ║");
            Console.WriteLine("║                                         ║");
            Console.WriteLine("║     USERNAME:                           ║");
            Console.WriteLine("║     PASSWORD:                           ║");
            Console.WriteLine("╚═════════════════════════════════════════╝");
            Console.SetCursorPosition(position, l_usrn);
            usr = Console.ReadLine();
            Console.SetCursorPosition(position, l_pwd);
            
            StringBuilder password = new StringBuilder();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write('\0');
                    Console.CursorLeft--;
                    password.Remove(password.Length - 1, 1);
                }
                else
                {
                    password.Append(keyInfo.KeyChar);
                    Console.CursorLeft--;
                    Console.Write("*");
                }

            }

            pwd = password.ToString();

            //FileStream login = new FileStream("login.txt", FileMode.Open, FileAccess.Read);
            String[] account = File.ReadAllLines("login.txt");
            while(i <account.Length){
                if (account[i] == usr && account[i + 1] == pwd)
                {
                    loginStatus = true;
                    Console.WriteLine();
                    Console.WriteLine("Valid credentials!... Please Press Any Key");
                    Console.ReadKey();
                    break;
                }
                else {        
                    i += 2;
                }
            }

            if (i == account.Length) {
                Console.WriteLine();
                Console.WriteLine("Invalid credentials!... Please try again");
                Console.ReadKey();
                Console.Clear();
                Portal();
            }

            return loginStatus;
        }

        public void Menu() {
            Console.Clear();
            Console.WriteLine("╔═════════════════════════════════════════╗");
            Console.WriteLine("║    WELCOME TO SIMPLE BANKING SYSTEM     ║");
            Console.WriteLine("╠─────────────────────────────────────────╣");
            Console.WriteLine("║     1. Create a new account             ║");
            Console.WriteLine("║     2. Search for an account            ║");
            Console.WriteLine("║     3. Deposit                          ║");
            Console.WriteLine("║     4. Withdraw                         ║");
            Console.WriteLine("║     5. A/C statement                    ║");
            Console.WriteLine("║     6. Delete account                   ║");
            Console.WriteLine("║     7. Exit                             ║");
            Console.WriteLine("╠─────────────────────────────────────────╣");
            Console.WriteLine("║    Enter your choice(1-7):              ║");
            Console.WriteLine("╚═════════════════════════════════════════╝");
            Console.SetCursorPosition(28, 11);
            String choice = Console.ReadLine();

            switch(choice){
                case "1":
                    CreateAccount();
                    break;
                case "2":
                    SearchAccount();
                    break;
                case "3":
                    Deposit();
                    break;
                case "4":
                    Withdraw();
                    break;
                case "5":
                    Statement();
                    break;
                case "6":
                    DeleteAccount();
                    break;
                case "7":
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.Clear();
                    Menu();
                    break;
            }
            

        }
        //Interface of creating a new account
        public void CreateAccount() {

            Console.Clear();
            Console.WriteLine("╔═════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                   CREATE A NEW ACCOUNT                  ║");
            Console.WriteLine("╠─────────────────────────────────────────────────────────╣");
            Console.WriteLine("║                    ENTER THE DETAILS                    ║");
            Console.WriteLine("║                                                         ║");
            Console.WriteLine("║  First Name:                                            ║");
            Console.WriteLine("║  Last Name:                                             ║");
            Console.WriteLine("║  Address:                                               ║");
            Console.WriteLine("║  Phone:                                                 ║");
            Console.WriteLine("║  Email:                                                 ║");
            Console.WriteLine("╚═════════════════════════════════════════════════════════╝");

            Console.SetCursorPosition(14, 5);
            firstName = Console.ReadLine();
            Console.SetCursorPosition(13, 6);
            lastName = Console.ReadLine();
            Console.SetCursorPosition(11, 7);
            address = Console.ReadLine();
            Console.SetCursorPosition(9, 8);

            //Validate the Phone Number
            var check = Console.ReadLine();
            ValidNum(check, phone);

            Console.SetCursorPosition(9, 9);
            //Validate the Email
            var checkm = Console.ReadLine();
            ValidMail(checkm, format, email);

            //Ask user if the input is correct
            YesOrNo();

            //Create files and send email
            CreateFile(initAccountNumber,firstName,lastName,address,phone,email);
            Console.ReadKey();
            Menu();

        }

        //Interface of searching for an account
        public void SearchAccount() {
            Console.Clear();
            Console.WriteLine("╔═════════════════════════════════════════╗");
            Console.WriteLine("║          SEARCH FOR AN ACCOUNT          ║");
            Console.WriteLine("╠─────────────────────────────────────────╣");
            Console.WriteLine("║            ENTER THE DETAILS            ║");
            Console.WriteLine("║                                         ║");
            Console.WriteLine("║  Account Number:                        ║");
            Console.WriteLine("╚═════════════════════════════════════════╝");

            Console.SetCursorPosition(18,5);
            var check = Console.ReadLine();
            ValidAccount(check);
            Console.ReadKey();
            Menu();
        }

        //Interface for deposit
        public void Deposit() {
            Console.Clear();
            Console.WriteLine("╔═════════════════════════════════════════╗");
            Console.WriteLine("║                 Deposit                 ║");
            Console.WriteLine("╠─────────────────────────────────────────╣");
            Console.WriteLine("║            ENTER THE DETAILS            ║");
            Console.WriteLine("║                                         ║");
            Console.WriteLine("║  Account Number:                        ║");
            Console.WriteLine("║  Amount: $                              ║");
            Console.WriteLine("╚═════════════════════════════════════════╝");

            Console.SetCursorPosition(18, 5);
            var check = Console.ReadLine();
            //Loop for validating input
            while (true)
            {

                if (check.Length <= 10 && int.TryParse(check, out initAccountNumber) == true) {

                    String files = Environment.CurrentDirectory + "\\" + check + ".txt";
                    String[] dirList = Directory.GetFiles(Environment.CurrentDirectory);
                   
                    //Loop for adding tranaction statements
                    for (int i = 0; i < dirList.Length; i++) {

                        if (files == dirList[i]) {

                            String[] content = File.ReadAllLines(check + ".txt");
                            Console.SetCursorPosition(0, 9);
                            Console.WriteLine("Your account is found!");
                            Console.SetCursorPosition(12, 6);
                            String deposit = Console.ReadLine();
                            decimal _deposit = 0;
                            if (decimal.TryParse(deposit, out _deposit) == true && _deposit > 0) {
                                
                                decimal balance = _deposit + Convert.ToDecimal(content[0]);                             
                                String[] lines = File.ReadAllLines(check + ".txt");
                                lines[0] = Convert.ToString(balance);
                                String transaction = "deposit: $" + deposit;
                                
                                if (lines.Length < 7) {
                                    statement[4] = transaction;
                                    File.WriteAllLines(check + ".txt", lines);
                                    File.AppendAllLines(check + ".txt", statement);
                                } else {
                                    statement = Queue(transaction,lines);
                                    File.WriteAllLines(check + ".txt", statement);
                                }
                                    Console.SetCursorPosition(0, 9);
                                    Console.WriteLine("Deposit Successfull!!");
                                    Console.ReadKey();
                                    Menu();
                            } else {
                                Console.SetCursorPosition(0, 6);
                                Console.WriteLine("║  Amount: $                              ║");
                                Console.SetCursorPosition(0, 9);
                                Console.WriteLine("Invalid input amount! try again");
                                Console.ReadKey();
                                Deposit();
                            }

                        } else {

                            continue;
                        }
 
                    }

                    Console.SetCursorPosition(0, 8);
                    ReturnD("Your account is not found! ... Try again(y/n)?");

                } else {
                Console.SetCursorPosition(0, 8);
                Console.WriteLine("Invalid Input!... It must be integer within 10 characters");
                Console.ReadKey();
                Deposit();
                }
            }

        }

        //Interface of withdraw
        public void Withdraw() {
            Console.Clear();
            Console.WriteLine("╔═════════════════════════════════════════╗");
            Console.WriteLine("║                 WITHDRAW                ║");
            Console.WriteLine("╠─────────────────────────────────────────╣");
            Console.WriteLine("║            ENTER THE DETAILS            ║");
            Console.WriteLine("║                                         ║");
            Console.WriteLine("║  Account Number:                        ║");
            Console.WriteLine("║  Amount: $                              ║");
            Console.WriteLine("╚═════════════════════════════════════════╝");

            Console.SetCursorPosition(18, 5);
            var check = Console.ReadLine();
            //Loop for validating input
            while (true)
            {

                if (check.Length <= 10 && int.TryParse(check, out initAccountNumber) == true)
                {

                    String files = Environment.CurrentDirectory + "\\" + check + ".txt";
                    String[] dirList = Directory.GetFiles(Environment.CurrentDirectory);
                    
                    //Loop for adding transaction statements
                    for (int i = 0; i < dirList.Length; i++)
                    {

                        if (files == dirList[i])
                        {

                            String[] content = File.ReadAllLines(check + ".txt");
                            Console.SetCursorPosition(0, 9);
                            Console.WriteLine("Your account is found!");
                            Console.SetCursorPosition(12, 6);
                            String withdraw = Console.ReadLine();
                            decimal _withdraw = 0;
                            if (decimal.TryParse(withdraw, out _withdraw) == true && _withdraw > 0)
                            {
                                decimal balance = Convert.ToDecimal(content[0]) - _withdraw;
                                if(balance >= 0) { 
                                    String[] lines = File.ReadAllLines(check + ".txt");
                                    lines[0] = Convert.ToString(balance);
                                    string transaction = "Withdraw: $" + withdraw;
                                    if (lines.Length < 7)
                                    {
                                        statement[4] = transaction;
                                        File.WriteAllLines(check + ".txt", lines);
                                        File.AppendAllLines(check + ".txt", statement);
                                    }
                                    else
                                    {
                                        statement = Queue(transaction, lines);
                                        File.WriteAllLines(check + ".txt", statement);
                                    }
                                    File.WriteAllLines(check + ".txt", lines);
                                    Console.SetCursorPosition(0, 9);
                                    Console.WriteLine("Withdraw Successfull!");
                                    Console.ReadKey();
                                    Menu();
                                }else{
                                    Console.SetCursorPosition(0, 6);
                                    Console.WriteLine("║  Amount: $                              ║");
                                    Console.SetCursorPosition(0, 9);
                                    Console.WriteLine("Insufficient account!            ");
                                    Console.ReadKey();
                                    Withdraw();
                                }
                            }
                            else
                            {
                                Console.SetCursorPosition(0, 6);
                                Console.WriteLine("║  Amount: $                              ║");
                                Console.SetCursorPosition(0, 9);
                                Console.WriteLine("Invalid input amount! try again");
                                Console.ReadKey();
                                Withdraw();
                            }

                        }
                        else
                        {
                            continue;
                        }

                    }

                    Console.SetCursorPosition(0, 8);
                    ReturnW("Your account is not found! ... Try again(y/n)?");

                }
                else
                {
                    Console.SetCursorPosition(0, 8);
                    Console.WriteLine("Invalid Input!... It must be integer within 10 characters");
                    Console.ReadKey();
                    Withdraw();
                }
            }
        }

        //Interface for statements
        public void Statement() {
            Console.Clear();
            Console.WriteLine("╔═════════════════════════════════════════╗");
            Console.WriteLine("║                STATEMENT                ║");
            Console.WriteLine("╠─────────────────────────────────────────╣");
            Console.WriteLine("║            ENTER THE DETAILS            ║");
            Console.WriteLine("║                                         ║");
            Console.WriteLine("║  Account Number:                        ║");
            Console.WriteLine("╚═════════════════════════════════════════╝");

            Console.SetCursorPosition(18, 5);
            var check = Console.ReadLine();
            ShowStatement(check);
            Menu();
        }

        //Interface for deleting an account
        public void DeleteAccount() {
            Console.Clear();
            Console.WriteLine("╔═════════════════════════════════════════╗");
            Console.WriteLine("║            DELETE AN ACCOUNT            ║");
            Console.WriteLine("╠─────────────────────────────────────────╣");
            Console.WriteLine("║            ENTER THE DETAILS            ║");
            Console.WriteLine("║                                         ║");
            Console.WriteLine("║  Account Number:                        ║");
            Console.WriteLine("╚═════════════════════════════════════════╝");

            Console.SetCursorPosition(18, 5);
            var check = Console.ReadLine();
            DelCheck(check);
            Console.ReadKey();
            Menu();
        }

        //Mail function for creating account
        public void MailCreate(String check)
        {

            String[] information = File.ReadAllLines(check+".txt");
            String accountNo = check;
            String fName = information[1];
            String lName = information[2];
            String address = information[3];
            String phone = information[4];
            String email = information[5];

            MailAddress toAddress = new MailAddress(email);
            string body = "<head><title>Your Statement</title></head>" +
                          "<body><table><tr><td>AccountNo:</td><td>" + accountNo + "</td></tr>" +
                          "<tr><td>Name:</td><td>" + fName + " " + lName + "</td></tr>" +
                          "<tr><td>Address:</td><td>" + address + "</td></tr>" +
                          "<tr><td>Phone:</td><td>" + phone + "</td></tr></table></body>";
            var smtp = new SmtpClient
            {
                Host = "smtp-mail.outlook.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("yu.utsclass@outlook.com", "Forutsdemo0")
            };
            using (var message = new MailMessage("yu.utsclass@outlook.com", email)
            {
                Subject = "Simple Bank -- Statement of " + accountNo,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
                Menu();
            }
        }

        //Mail function for sending statements
        public void Mail(String check) {

            String[] information = File.ReadAllLines(check + ".txt");
            String accountNo = check;
            String accountBal = information[0];
            String fName = information[1];
            String lName = information[2];
            String address = information[3];
            String phone = information[4];
            String email = information[5];
            if (information.Length < 7) {
                Console.WriteLine("No transaction is Recorded");
                Console.ReadKey();
                Menu();
            }
            else { 
                String trans1 = information[6];
                String trans2 = information[7];
                String trans3 = information[8];
                String trans4 = information[9];
                String trans5 = information[10];

                MailAddress toAddress = new MailAddress(email);
                string body = "<head><title>Your Statement</title></head>" +
                                "<body><table><tr><td>AccountNo:</td><td>" + accountNo + "</td></tr>" +
                                "<tr><td>Name:</td><td>" + fName + " " + lName + "</td></tr>" +
                                "<tr><td>Address:</td><td>" + address + "</td></tr>" +
                                "<tr><td>Phone:</td><td>" + phone + "</td></tr>" +
                                "<tr><td>Balance:</td><td>" + accountBal + "</td></tr>" +
                                "<tr><td>Statement:</td><td></td></tr>" +
                                "<tr><td>#1</td><td>" + trans1 + "</td></tr>" +
                                "<tr><td>#2</td><td>" + trans2 + "</td></tr>" +
                                "<tr><td>#3</td><td>" + trans3 + "</td></tr>" +
                                "<tr><td>#4</td><td>" + trans4 + "</td></tr>" +
                                "<tr><td>#5</td><td>" + trans5 + "</td></tr></table></body>";

                var smtp = new SmtpClient
                {
                    Host = "smtp-mail.outlook.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("yu.utsclass@outlook.com", "Forutsdemo0")
                };
                using (var message = new MailMessage("yu.utsclass@outlook.com", email)
                {
                    Subject = "Simple Bank -- Statement of " + accountNo,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                    Menu();
                }
       
            }
        }



        //Switch in creating account
        public void YesOrNo() {

            Console.SetCursorPosition(0, 14);
            Console.WriteLine("Is information correct? Y/N");
            ConsoleKeyInfo keyinfo = Console.ReadKey();
            var key = keyinfo.Key;

            switch (key)
            {
                case ConsoleKey.Y:
                    Console.SetCursorPosition(0, 15);
                    Console.WriteLine("Account Created! details will be provided via email! This process may take a while...");
                    break;
                case ConsoleKey.N:
                    CreateAccount();
                    break;
                default:
                    YesOrNo();
                    break;
            }
        }

        //Switch in searching account
        public void Return(String notification) {
            Console.WriteLine(notification);
            ConsoleKeyInfo keyinfo = Console.ReadKey();
            var key = keyinfo.Key;

            switch (key)
            {
                case ConsoleKey.Y:
                    SearchAccount();
                    break;
                case ConsoleKey.N:
                    Menu();
                    break;
                default:
                    Return(notification);
                    break;
            }
        }

        public void ReturnS(String notification)
        {
            Console.WriteLine(notification);
            ConsoleKeyInfo keyinfo = Console.ReadKey();
            var key = keyinfo.Key;

            switch (key)
            {
                case ConsoleKey.Y:
                    Statement();
                    break;
                case ConsoleKey.N:
                    Menu();
                    break;
                default:
                    ReturnS(notification);
                    break;
            }
        }
        //Switch in deposit
        public void ReturnD(String notification)
        {
            Console.WriteLine(notification);
            ConsoleKeyInfo keyinfo = Console.ReadKey();
            var key = keyinfo.Key;

            switch (key)
            {
                case ConsoleKey.Y:
                    Deposit();
                    break;
                case ConsoleKey.N:
                    Menu();
                    break;
                default:
                    ReturnD(notification);
                    break;
            }
        }

        //Switch in withdraw
        public void ReturnW(String notification)
        {
            Console.WriteLine(notification);
            ConsoleKeyInfo keyinfo = Console.ReadKey();
            var key = keyinfo.Key;

            switch (key)
            {
                case ConsoleKey.Y:
                    Withdraw();
                    break;
                case ConsoleKey.N:
                    Menu();
                    break;
                default:
                    ReturnW(notification);
                    break;
            }
        }

        //Switch in deleting account
        public void ReturnR(String notification,String accName)
        {
            Console.WriteLine(notification);
            ConsoleKeyInfo keyinfo = Console.ReadKey();
            var key = keyinfo.Key;

            switch (key)
            {
                case ConsoleKey.Y:
                    File.Delete(accName + ".txt");
                    Console.WriteLine(" ");
                    Console.WriteLine("This account has been deleted!");
                    Console.ReadKey();
                    Menu();
                    break;
                case ConsoleKey.N:
                    Menu();
                    break;
                default:
                    ReturnR(notification,accName);
                    break;
            }
        }
        
        //Switch in deleting account when account is not found
        public void ReturnSR(String notification)
        {
            Console.WriteLine(notification);
            ConsoleKeyInfo keyinfo = Console.ReadKey();
            var key = keyinfo.Key;

            switch (key)
            {
                case ConsoleKey.Y:
                    DeleteAccount();
                    break;
                case ConsoleKey.N:
                    Menu();
                    break;
                default:
                    ReturnSR(notification);
                    break;
            }
        }

        //Switch in Mail function
        public void ReturnM(String notification,String Check)
        {
            Console.WriteLine(notification);
            ConsoleKeyInfo keyinfo = Console.ReadKey();
            var key = keyinfo.Key;

            switch (key)
            {
                case ConsoleKey.Y:
                    Mail(Check);
                    break;
                case ConsoleKey.N:
                    Menu();
                    break;
                default:
                    ReturnM(notification,Check);
                    break;
            }
        }

        //funtion for createfile&account
        public void CreateFile(int accountNumber, String fName, String lName, String address, Int64 pNum, String email) {
            int iteration = accountNumber;
            String currentFile = Convert.ToString(iteration) + ".txt";
            String balance = Convert.ToString(accountBalance);
            while (File.Exists(currentFile)) {
                iteration += 1;
                currentFile = Convert.ToString(iteration) + ".txt";
            }
            String phone = Convert.ToString(pNum);
            String[] content = { balance, fName, lName, address, phone, email };
            File.WriteAllLines(currentFile, content);
            Console.WriteLine("Account Number is: " + iteration);
            String account = Convert.ToString(iteration);
            MailCreate(account);
        }
        
        //function for validating number input
        public Int64 ValidNum(String check, Int64 data) {
            while (true)
            {
                if (check.Length <= 10 && Int64.TryParse(check, out data) == true)
                {
                    phone = Convert.ToInt64(check);
                    return phone;
                }
                else
                {
                    Console.SetCursorPosition(0, 8);
                    Console.WriteLine("║  Phone:                                                 ║");
                    Console.SetCursorPosition(0, 13);
                    Console.WriteLine("Invalid Input!... It must be integer within 10 characters");
                    Console.SetCursorPosition(9, 8);
                    check = Console.ReadLine();
                }
            }

        }

        //funtion for showing result in checking account module
        public void ValidAccount(String check) {
            while (true) {

                if (check.Length <= 10 && int.TryParse(check, out initAccountNumber) == true) {

                    String files = Environment.CurrentDirectory + "\\" + check + ".txt";
                    String[] dirList = Directory.GetFiles(Environment.CurrentDirectory);
                    for (int i = 0; i < dirList.Length; i++) {    
                        if (files == dirList[i]) {
                            String[] content = File.ReadAllLines(check + ".txt");

                            Console.SetCursorPosition(0, 8);
                            Console.WriteLine("Your account is found!");
                            Console.WriteLine("╔═════════════════════════════════════════╗");
                            Console.WriteLine("║             ACCOUNT DETAILS             ║");
                            Console.WriteLine("╠─────────────────────────────────────────╣");
                            Console.WriteLine("║  Account No: {0,-10}                 ║",check);
                            Console.WriteLine("║  Account Balance: ${0,-10}           ║",content[0]);
                            Console.WriteLine("║  First Name: {0,-15}            ║",content[1]);
                            Console.WriteLine("║  Last Name: {0,-15}             ║", content[2]);
                            Console.WriteLine("║  Address: {0,-30}║", content[3]);
                            Console.WriteLine("║  Phone: {0,-10}                      ║", content[4]);
                            Console.WriteLine("║  Email: {0,-30}  ║", content[5]);
                            Console.WriteLine("╚═════════════════════════════════════════╝");
                            Console.ReadKey();
                            Return("Check another account(y/n)?");
                            break;
                        } else {
                            continue;
                        }
                    }

                    Console.SetCursorPosition(0, 8);
                    Return("Your account is not found! ... Try again(y/n)?");
                    break;
                } else {
                        Console.SetCursorPosition(0, 8);
                        Console.WriteLine("Invalid Input!... It must be integer within 10 characters");
                        Console.ReadKey();
                        SearchAccount();
                }
            }
            
        }

        //funtion for showing statement in statement module
        public void ShowStatement(String check)
        {
            while (true)
            {

                if (check.Length <= 10 && int.TryParse(check, out initAccountNumber) == true)
                {

                    String files = Environment.CurrentDirectory + "\\" + check + ".txt";
                    String[] dirList = Directory.GetFiles(Environment.CurrentDirectory);
                    for (int i = 0; i < dirList.Length; i++)
                    {
                        if (files == dirList[i])
                        {
                            String[] content = File.ReadAllLines(check + ".txt");

                            Console.SetCursorPosition(0, 8);
                            Console.WriteLine("Your account is found!");
                            Console.WriteLine("╔═════════════════════════════════════════╗");
                            Console.WriteLine("║             ACCOUNT DETAILS             ║");
                            Console.WriteLine("╠─────────────────────────────────────────╣");
                            Console.WriteLine("║  Account Statement                      ║");
                            Console.WriteLine("║                                         ║");
                            if (content.Length > 7) { 
                                for (int j = 6; j < content.Length; j++) {
                            Console.WriteLine("║  {0,-17}                      ║", content[j]);
                                }
                            }
                            Console.WriteLine("║                                         ║");
                            Console.WriteLine("║  Account No: {0,-10}                 ║", check);
                            Console.WriteLine("║  Account Balance: ${0,-10}           ║", content[0]);
                            Console.WriteLine("║  First Name: {0,-15}            ║", content[1]);
                            Console.WriteLine("║  Last Name: {0,-15}             ║", content[2]);
                            Console.WriteLine("║  Address: {0,-30}║", content[3]);
                            Console.WriteLine("║  Phone: {0,-10}                      ║", content[4]);
                            Console.WriteLine("║  Email: {0,-30}  ║", content[5]);
                            Console.WriteLine("╚═════════════════════════════════════════╝");
                            Console.ReadKey();
                            ReturnM("Email Statement (y/n)?", check);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    Console.SetCursorPosition(0, 8);
                    ReturnS("Your account is not found! ... Try again(y/n)?");
                    //break;
                }
                else
                {
                    Console.SetCursorPosition(0, 8);
                    Console.WriteLine("Invalid Input!... It must be integer within 10 characters");
                    Console.ReadKey();
                    Statement();
                }
            }

        }

        //function for checking account in deleting account module
        public void DelCheck(String check)
        {
            while (true)
            {

                if (check.Length <= 10 && int.TryParse(check, out initAccountNumber) == true)
                {

                    String files = Environment.CurrentDirectory + "\\" + check + ".txt";
                    String[] dirList = Directory.GetFiles(Environment.CurrentDirectory);
                    for (int i = 0; i < dirList.Length; i++)
                    {
                        if (files == dirList[i])
                        {
                            String[] content = File.ReadAllLines(check + ".txt");

                            Console.SetCursorPosition(0, 8);
                            Console.WriteLine("Your account is found!");
                            Console.WriteLine("╔═════════════════════════════════════════╗");
                            Console.WriteLine("║             ACCOUNT DETAILS             ║");
                            Console.WriteLine("╠─────────────────────────────────────────╣");
                            Console.WriteLine("║                                         ║");
                            Console.WriteLine("║  Account No: {0,-10}                 ║", check);
                            Console.WriteLine("║  Account Balance: ${0,-10}           ║", content[0]);
                            Console.WriteLine("║  First Name: {0,-15}            ║", content[1]);
                            Console.WriteLine("║  Last Name: {0,-15}             ║", content[2]);
                            Console.WriteLine("║  Address: {0,-30}║", content[3]);
                            Console.WriteLine("║  Phone: {0,-10}                      ║", content[4]);
                            Console.WriteLine("║  Email: {0,-30}  ║", content[5]);
                            Console.WriteLine("╚═════════════════════════════════════════╝");
                            Console.ReadKey();
                            ReturnR("Sure to delete this account (y/n)?",check);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    Console.SetCursorPosition(0, 8);
                    ReturnSR("Your account is not found! ... Try again(y/n)?");
                    break;
                }
                else
                {
                    Console.SetCursorPosition(0, 8);
                    Console.WriteLine("Invalid Input!... It must be integer within 10 characters");
                    Console.ReadKey();
                    DeleteAccount();
                }
            }

        }

        //funtion for validating email address
        public String ValidMail(String checkm, String format, String data) {
            while (true)
            {
                if (Regex.IsMatch(checkm, format) == false)
                {
                    Console.SetCursorPosition(0, 9);
                    Console.WriteLine("║  Email:                                                 ║");
                    Console.SetCursorPosition(0, 13);
                    Console.WriteLine("Invalid Input!... It must be a valid Email address         ");
                    Console.SetCursorPosition(9, 9);
                    checkm = Console.ReadLine();
                }
                else
                {
                    email = checkm;
                    return email;
                }
            }
        }

        //function for rolling the record of last five transactions
        public String[] Queue(String transaction,String[] record) {
            
            if (record[10] == "") {
                statement[10] = transaction;
            } else {
                record[6] = record[7];
                record[7] = record[8];
                record[8] = record[9];
                record[9] = record[10];
                record[10] = transaction;
            }

            return record;
        }

        
    }


}
