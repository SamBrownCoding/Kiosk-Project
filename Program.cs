using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;

namespace Kiosk_Project {
    
    internal class Program {
        static void Main(string[] args) {

            //The program should keep track of how many of each denomination of bill and coin the kiosk currently has. Use a user defined datatype to do this.

            //The program should allow any number of items costs to be input until no cost has been supplied.

            //Once all items have been input the kiosk should display a total and ask the user to insert money.
            //The user should be able to insert any denomination of bills or coins until their total amount exceeds the cost of all the items.  

            //The kiosk should calculate how much change should be dispensed and dispense the change. (Research and use a greedy algorithm to dispense the change)

            //If the kiosk does not have enough physical money to supply the change then the transaction will be cancelled, and the user should be offered another way to make
            //payments(you do not have to develop other methods of payment)
            //decimal owed = 0;

            //KIOSK START
                CashInventory vault = new CashInventory();
                bool doneShop = true;
            while (true)
            {
                //vault.DisplayVault();
                decimal owedAmount = ItemCosts();
                //owed = owedAmount;
                decimal vaultAmount = vault.CalculateTotalVault();

                //TESTING
                //Console.WriteLine($"before: {vaultAmount:c}");//TESTING

                vault.AlternatePayment(owedAmount);
                decimal refund = vault.CashRefund;
                //DISPENSE THE CHANGE
                List<decimal> ToDispense = vault.Dispenser(vault.Change);

                if (ToDispense.Count > 0)
                {
                    Console.WriteLine($"\t\t\tProcessing To Dispense: {vault.Change:c}\n");
                    Thread.Sleep(1500);
                    ColorText("\t\t\tDispense", ConsoleColor.Red, true);
                    ColorText("\t\t\t==========================", ConsoleColor.DarkRed, true);
                    foreach (var denom in ToDispense)
                    {
                        Console.WriteLine($"\t\t\t{denom:c}");
                    }//end for each
                } 
                else if (refund > vaultAmount) {
                    Console.WriteLine("\t\t\tIncoming Refund due to insufficient fund");
                    Thread.Sleep(1500);
                    Console.Write($"\t\t\tRefund: {refund:c}\n");
                    Console.WriteLine($"\t\t\tProcessing To Refund: {refund:c}\n");
                    Thread.Sleep(1500);
                    ColorText("\t\t\tRefund", ConsoleColor.Red, true);
                    ColorText("\t\t\t==========================", ConsoleColor.DarkRed, true);
                    Console.WriteLine($"\t\t\t{refund:c}");
                }
                
                Console.WriteLine();

                //TESTING
                
                //Console.WriteLine($"{vault.AcctNumber} and {vault.CardType}");
                //Console.WriteLine($"AFTER VAULT COUNT : {vault.CalculateTotalVault():c}");

                //TO START A NEW TRANSACTION OR STOP THE TRANSACTION
                if (NewTransaction(vault.CardType, vault.AcctNumber, owedAmount, vault.Change, vault.CashBack, vault.PersonPay, vault.CashRefund, vault.Success).ToLower() == "n")
                {
                    //TO DONE WITH SHOPPING
                    break;            

                } else {
                    //CONTINUE THE GAME
                    doneShop = false;
                    

                }//end else
            }//end while         

        }//end main
          
        //=========================================================================================================

        #region ItemCost
        static decimal ItemCosts() {
            //LOADING
            Console.WriteLine("\t\t\t\t\t\tLoading Kiosk...\n");
            // Simulate loading progress
            for (int i = 0; i <= 100; i++) {
                UpdateLoadingProgress(i);
                Thread.Sleep(10); // Adjust the delay as needed                
            }//end for
            Thread.Sleep(500);
            Console.WriteLine();
            ColorText("\n\t\t\t\t\t\tKiosk complete!", ConsoleColor.Green, true);
            Thread.Sleep(1250);
            Console.Clear();
            Header("Welcome to Self-Checkout Kiosk");
            Console.WriteLine();
            //COUNTINNG THE ITEM
            int count = 1;
            decimal total = 0;
            bool shopMore = true;
            //ASK THE USER TO INPUT OF WHAT ITEM COST IT IS
                while (shopMore) {
                decimal item = PromptForNumber($"\t\t\tItem {count} cost: $");
                Console.WriteLine();
                //IF USER IS DONE WITH ITEM PROCESS (TO PRESS ENTER TO STOP)     
                    if (item == 0) {
                        count--;
                        break;
                    //IF USER INPUT 0 OR LESS WILL GET INVALID MESSAGE
                    } else if (item < 0) {  
                        ColorText("\t\t\tInvalid Number!\n", ConsoleColor.DarkRed, true);
                        continue;
                    } else {
                    decimal itemCost = item;
                        total += itemCost;
                    }//end else

                    //Increment the count of Item and add up the total cost
                    count++;

                }//end while

            Console.WriteLine($"\t\t\tYou have {count} items.");
                Console.WriteLine();
            Console.Write($"\t\t\tTotal: ");
            ColorText($"{total:c}", ConsoleColor.Green);
                Console.WriteLine();
            return total;
        }//end ItemCosts Function

        #endregion        

        #region New Transaction

        static string NewTransaction(string cardType, string acct, decimal owed, decimal moneyReturn, decimal cashRequest, decimal personPay, decimal refund, decimal suss) {
            string newTransaction;
            string lowerEmAll;
                    //RECEIPT LOG INFO
            Random random = new Random();
            int rng = random.Next(1000, 9999);
            string transactionNumber = rng.ToString();
            var transactionDate = DateTime.Now.ToString("MM-dd-yyyy");
            var transactionTime = DateTime.Now.ToString("h:mmtt");
            decimal totalCashAmount = owed;
            string cardNumber = acct;
            decimal cardCashback = cashRequest;
            string paymentCardVendor = cardType;
            decimal changeGiven = moneyReturn;
            decimal personPaid = personPay;
            decimal refundPaid = refund;
            decimal sussPaid = suss;

            if (changeGiven >= 0 || sussPaid == 1) {
                //SET UP ARGUMENT        
                string argument = $"{transactionNumber} {transactionDate} {transactionTime} {totalCashAmount} {cardNumber} {cardCashback} {paymentCardVendor} {changeGiven} {personPaid}";

                //PROCESS START INFO TO FILE
                ProcessStartInfo receipt = new ProcessStartInfo();
                receipt.FileName = @"C:\Users\MCA-26\Documents\GitHub\Test-C-\TransactionLoggingPackage\bin\Debug\net8.0\TransactionLoggingPackage.exe";
                receipt.Arguments = argument;
                Process.Start(receipt);
                //end LOG
            } else {
                ColorText("\t\t\tTransaction Cancelled", ConsoleColor.Red, true);
            }


            while (true){

                newTransaction = Prompt("\t\t\tDo you want to start a new transaction? (---Y---/---N---): ");
                Console.WriteLine();
                lowerEmAll = newTransaction.ToLower();
                if (lowerEmAll == "y") {
                    Console.Clear();
                    //return true;
                } else if (lowerEmAll == "n") {
                    Header("   Thank you for Shopping!\t");
                    Thread.Sleep(2000);

                } else {
                    ColorText("\t\t\tInvalid response! Please type 'Y' or 'N'\n", ConsoleColor.Red, true);
                }//end if
                while (lowerEmAll != "n" && lowerEmAll != "y") ;
                return lowerEmAll;
            }//end while loop

        }//end function
        #endregion

        #region LoadingProgressFun
        static void UpdateLoadingProgress(int percentage)
        {           
            Console.Write($"\r[{new string('#', percentage)}{new string(' ', 100 - percentage)}] {percentage}%");
        }//end Update function
        #endregion

        #region COLOR TEXT AND NUMBER

        static void ColorText(string message, ConsoleColor color, bool isWriteLine = true) {
            Console.ForegroundColor = color;
            if (isWriteLine) {
                Console.WriteLine(message);
            } else {
                Console.Write(message);
            }//end if
            Console.ResetColor();
        }//end ColorText function

        static void ColorNum(int number, ConsoleColor color, bool isWriteLine = true) {
            Console.ForegroundColor = color;
            if (isWriteLine) {
                Console.WriteLine(number);
            } else {
                Console.Write(number);
            }//end if
            Console.ResetColor();
        }//end ColorText function

        #endregion

        #region   TITLE AND FRAME
        static void Header(string title) {
            Console.WriteLine("\t\t\t╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("\t\t\t║                                                               ║");
            Console.WriteLine($"\t\t\t║\t\t   {title} \t\t║");
            Console.WriteLine("\t\t\t║                                                               ║");
            Console.WriteLine("\t\t\t╚═══════════════════════════════════════════════════════════════╝");
        }//end Header Function        
        #endregion

        #region PROMPT FUNCTIONS


        static string Prompt(string dataRequest) {
            //VARIABLES
            string userInput = "";

            //REQUEST INFORMATION FROM USER
            Console.Write(dataRequest);

            //RECEIVE RESPONSE
            userInput = Console.ReadLine();

            return userInput;

        }// end Prompt Function

        static int PromptTryInt(string dataRequest) {
            //VARIABLES
            int userInput = 0;
            bool isValid = false;

            //INPUT VALIDATION LOOP
            do {
                Console.Write(dataRequest);
                isValid = int.TryParse(Console.ReadLine(), out userInput);
            } while (isValid == false);

            return userInput;
        }//end PromptTryInt Function

        static decimal PromptForNumber(string prompt) {
            decimal result;
            bool isValidInput = false;
            //WANT THE NUMBER INPUT AND PREVENT A STRING
            do {
                Console.Write(prompt);
                string userInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(userInput)) {
                    //BREAK OUT OF LOOP if empty
                    return 0;
                }//end if

                isValidInput = decimal.TryParse(userInput, out result);

                if (!isValidInput) {
                    Console.WriteLine("Invalid Input. Please enter a valid number.");
                }//end if 
            } while (!isValidInput);
            return result;
        }//end PromptForNumber

        static double PromptTryDouble(string dataRequest) {
            //VARIABLES
            double userInput = 0.0;
            bool isValid = false;

            //INPUT VALIDATION LOOP
            do {
                Console.Write(dataRequest);
                isValid = double.TryParse(Console.ReadLine(), out userInput);
            } while (isValid == false);
            return userInput;
        }//end PromptTryDouble function

        static decimal PromptTryDecimal(string dataRequest) {
            //VARIABLES
            decimal userInput = 0;
            bool isValid = false;

            //INPUT VALIDATION LOOP
            do {
                Console.Write(dataRequest);
                isValid = decimal.TryParse(Console.ReadLine(), out userInput);
            } while (isValid == false);
            return userInput;
        }//end PromptTryDouble function

        #endregion

    }//end class
}//end namespace