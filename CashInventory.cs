using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Net;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.XPath;

namespace Kiosk_Project {
    internal class CashInventory {
        #region FIELDS
            decimal _hundredDollar   = 0;
            decimal _fiftyDollar     = 0;
            decimal _twentyDollar    = 0;
            decimal _tenDollar       = 0;
            decimal _fiveDollar      = 0;
            decimal _oneDollar       = 1;
                                       
             decimal _quarter        = 0;     
            decimal _dime            = 0;
            decimal _nickel          = 0;
            decimal _penny           = 0;
            decimal _change          = 0;
            int    _count            = 0;

            decimal _DispenseAmount  = 0;
            decimal _refundMoney     = 0;
            string _cardType         = "n/a";
            string _acctNumber       = "n/a";
            decimal _cashBack        = 0;
            decimal _personPay       = 0;
            decimal _cardSuccess     = 0;

        #endregion
               
        #region PROPERTIES

        public int Count {get { return _count; } set { _count = value; } }//end Count

            public decimal Hundred  {get { return _hundredDollar; } }//end hundred
            public decimal Fifty    {get { return _fiftyDollar; } }//end fifty
            public decimal Twenty   {get { return _twentyDollar; }}//end twenty
            public decimal Ten      {get { return _tenDollar; } }//end ten
            public decimal Five     {get { return _fiveDollar; } }//end five
            public decimal Dollar   {get { return _oneDollar; } }//end dollar

            public decimal Quarter  {get { return _quarter; } }//end 25 cent
            public decimal Dime     {get { return _dime; } }//end dime
            public decimal Nickel   {get { return _nickel; } }//end nickel
            public decimal Penny    {get { return _penny; } }//end penny

            public decimal Change   { get { return _change; } }//end change

            public decimal Dispense { get { return _DispenseAmount; } }//end Dispense
            public decimal CashRefund { get {  return _refundMoney; } }//end CashInsert
            public string CardType { get { return _cardType; } }//end CardType
            public string AcctNumber { get { return _acctNumber; } }//end AcctNumber
            public decimal CashBack { get { return _cashBack; } }//end CashBack
            public decimal PersonPay { get { return _personPay; } }//end PersonPay
            public decimal Success { get { return _cardSuccess; } }//end success

        #endregion


        #region CALCULATE VAULT TOTAL
        public decimal CalculateTotalVault() {
            //SHOW HOW MUCH TOTAL IN THE VAULT
            decimal totalVault = (Hundred * 100) + 
                                 (Fifty * 50) + 
                                 (Twenty * 20) +
                                 (Ten * 10) + 
                                 (Five * 5) + 
                                 (Dollar) + 
                                 (Quarter * 0.25m) + 
                                 (Dime * 0.10m) +
                                 (Nickel * 0.05m) + 
                                 (Penny * 0.01m);
            
            return totalVault;
        }//end Calculate method
        #endregion

        public void RefundMoneyInserted(decimal refund) {
            decimal amount = refund;
        }//end Refund Money Method
        #region DISPLAY VAULT
        public void DisplayVault() {
            //DISPLAY THE MONEY VAULT
            Console.WriteLine("Kiosk Vault Inventory: \n");
            Console.WriteLine($"Hundred Bill:\t {Hundred}");
            Console.WriteLine($"Fifty Bill:\t {Fifty}");
            Console.WriteLine($"Twenty Bill:\t {Twenty}");
            Console.WriteLine($"Ten Bill:\t {Ten}");
            Console.WriteLine($"Five Bill:\t {Five}");
            Console.WriteLine($"Dollar Bill:\t {Dollar}");
            Console.WriteLine($"Quarter:\t {Quarter}");
            Console.WriteLine($"Dime:\t\t {Dime}");
            Console.WriteLine($"Nickel:\t\t {Nickel}");
            Console.WriteLine($"Penny:\t\t {Penny}\n");
                        
        }//end Display Method
        #endregion
        
        #region LUHN ALGORITHM
        //RETURN TRUE IF GIVEN
        //CARD NUMBER IS VALID
        public bool ValidateCardNumber(string acct) {
            string cardNumber = acct;
            if (cardNumber.Length !=15 && cardNumber.Length != 16) {
                //Console.WriteLine("Invalid Card number length.");
                return false;
            }//end if 

            int nSum = 0;
            bool isSecond = false;


            for (int i = cardNumber.Length - 1; i >= 0; i--) {
                int checkNumber = cardNumber[i] - '0';
                if (isSecond) {
                    //ADD TWO DIGITS TO HANDLE # CASES THAT MAKE 
                    //TWO DIGITS AFTER DOUBLE
                    checkNumber *= 2;

                if (checkNumber > 9) {
                        checkNumber -= 9; 
                    }//end if 
            
                }//end if 

                nSum += checkNumber;
                isSecond = !isSecond;            
            }//end for 

            if (nSum % 10 != 0) {
                Console.WriteLine("\t\t\tInvaild Number (Luhn failed).");
                return false;            
            }//end if 
            return true;
        }//end Validate Card Method 
        #endregion

        #region CARD TYPES
        public string GetCardType(string number) {
            //EXTRACT THE FIRST DIGIT             
            char oneDigit = number[0];
            decimal maxDigit = number.Length;
            //DETERMINE WHICH CARD VENDOR BASED ON THE FIRST DIGIT
            string vendor;
            switch (oneDigit) {
                case '3':
                    vendor = "AMEX";
                    break;
                case '4':
                    vendor = "Visa";
                    break;
                case '5':
                    vendor = "MasterCard";
                    break;
                case '6':
                    vendor = "Discover";
                    break;
                default:
                    vendor = "Unknown";
                    break;

            }//end switch
            return vendor;
        }//end Get Card Number method
        #endregion

        #region     PAYMENT OPTIONS
        public void AlternatePayment(decimal amount) {
            decimal totalAmount = amount;
            //ASK USER FOR OPTIONS TO PAY FOR ITEMS.
            Console.WriteLine("\t\t\tWould you like to change your payment method or pay the remaining balance in cash?\n");
            Console.WriteLine("\t\t\t\t1. Pay Card method\t2. Pay remaining balance in cash\n");
            int choice = PromptTryInt("\t\t\tChoose an Option: ");
            Console.WriteLine();

            if (choice <= 2) {
                switch (choice) {

                    case 1:
                        //ASKING USER TO WANT CASHBACK
                        //HANDLE CHANGING CARD PAYMENT METHOD
                        Console.Clear();
                        ProcessCardPayment(totalAmount);
                        break;

                    case 2:
                        //HANDLE PAY REMAINING IN CASH
                        Console.Clear();
                        Console.Write($"\t\t\tPlease pay the remaining balance in cash: ");
                        ColorText($"{totalAmount:c}", ConsoleColor.Green);
                        Console.WriteLine();
                        InsertCash(totalAmount);
                        break;

                    default:
                        ColorText("\t\t\tInvalid Choice. Please pick 1 or 2.", ConsoleColor.DarkRed, true);
                        AlternatePayment(amount);//CALL PROMPT AGAIN IF IT INVALID CHOICE
                        break;
                }//end switch
            }else {
                ColorText("\t\t\tInvalid input. Please enter a valid choice.", ConsoleColor.DarkRed, true);
                AlternatePayment(amount);//CALL PROMPT AGAIN IF IT INVALID CHOICE
            }//end if and else
        }//end Alternate payment method
        #endregion


        #region PROCESS CARD PAYMENT
        public void ProcessCardPayment(decimal amount) {
            string cardNumber = "";
            string wantMoney;
            string lowerCase;
            decimal cashBack = 0;
            decimal cashAmount = 0;
            bool isProcessed = false;

            while (!isProcessed) {

                //USER INPUT THEIR CARD NUMBER TO WETHER IT APPROVE OR NOT
                cardNumber = Prompt("\t\t\tEnter a Card Number: ");

                if (ValidateCardNumber(cardNumber)) {
                    ColorText("\t\t\tCard Accepted", ConsoleColor.DarkGreen, true);
                    Thread.Sleep(1000);
                    string card = GetCardType(cardNumber);
                    Console.WriteLine($"\t\t\tCard vendor: {card}");
                    isProcessed = true;
                    string last4 = "**";
                    for (int i = cardNumber.Length - 4; i < cardNumber.Length; i++) { 
                        last4 += cardNumber[i];
                }//end for 
                    //Console.WriteLine(last4);
                    _acctNumber = last4;
                    _cardType = card;
                } else {

                    ColorText("\t\t\tInvalid Card", ConsoleColor.DarkRed, true);
                }//end else
            }//end while
                
                bool isSelect = false;

                while (!isSelect) {

                    wantMoney = Prompt("\t\t\tWould you like a cashback? <Y>/<N>: ");
                    lowerCase = wantMoney.ToLower();

                    if (lowerCase == "y") {//GET CASHBACK OF AMOUNT YOU REQUEST
                            isSelect = true;
                        cashBack = PromptTryDecimal("\t\t\tHow much money would you like to have: $");
                        cashAmount = cashBack + amount;
                        _cashBack = cashBack;

                        string[] cardCheck = MoneyRequest(cardNumber, cashAmount);

                        if (cardCheck[1] == "declined") {

                            ColorText("\t\t\tCredit Card Declined.", ConsoleColor.DarkRed, true);
                                AlternatePayment(amount);
                        
                        } else {
                            decimal paid = decimal.Parse(cardCheck[1]);

                        if (paid < cashAmount) {

                            decimal remainingBalance = cashAmount - paid;
                            Console.WriteLine("\t\t\tCredit Card Payment successful");
                            Console.WriteLine($"\t\t\tPartial payment received: {paid:c}");
                            Console.WriteLine($"\t\t\tRemaining: {remainingBalance:c}");
                            Console.WriteLine();
                            AlternatePayment(remainingBalance);
                        } else {
                            
                            Console.WriteLine("\t\t\tCredit Card Payment successful");
                        }//else
                            _change = cashBack;
                        }//end else

                    } else if (lowerCase == "n") {
                        //NO CASHBACK AND STRAIGHT TO CARD PAYMENT
                        isSelect= true;

                        string[] cardCheck = MoneyRequest(cardNumber, amount);

                        if (cardCheck[1] == "declined") {

                            ColorText("\t\t\tCredit Card Declined.", ConsoleColor.DarkRed, true);
                        AlternatePayment(amount);

                        } else {
                            decimal paid = decimal.Parse(cardCheck[1]);

                        if (paid < amount) {
                            decimal remainingBalance = amount - paid;
                            Console.WriteLine("\t\t\tCredit Card Payment successful");
                            Console.WriteLine($"\t\t\tPartial payment received: {paid:c}");
                            Console.WriteLine($"\t\t\tRemaining: {remainingBalance:c}");
                            AlternatePayment(remainingBalance);
                        }
                        else {
                            //new "update"
                            Console.WriteLine("\t\t\tCredit Card Payment successful");
                        }//end else Credit card pay success
                           _cardSuccess = 1;                            

                        }//end else decimal paid
                    } else {
                        ColorText("\t\t\tInvalid response! Please 'Y' or 'N'", ConsoleColor.DarkRed, true);
                    continue;
                    }//end else
                }//end while                

        }//end ProcessCard Method
        #endregion

        #region INSERT CASH
        public void InsertCash(decimal owed) {
            decimal amountPaid          = 0;
            decimal change              = 0;
            decimal youPay              = 0;
            //ASK THE USER TO INSERT THE MONEY
            while (amountPaid < owed) {
                string insert = Prompt("\t\t\tPlease insert money: $");
                //IF USER WANT TO CANCEL THE TRANSACTION (PRESS ENTER TO STOP)
                if (insert == "") {
                    ColorText("\t\t\tTransaction failed", ConsoleColor.DarkRed, true);
                    return;
                }//end if
                    //USER INPUT NEGATIVE NUMBER WILL GET THE MESSAGE THAT IS INVALID.
                if (!decimal.TryParse(insert, out decimal payment) || payment < 0) {
                    Console.WriteLine("\t\t\tInvalid Number! Please enter a positive number value.");
                    continue;
                }//end if
                //CORRECT CURRENCY MONEY TO INSERT
                if (payment == 100 || payment == 50 || payment == 20 || payment == 10 || payment == 5 || payment == 2 || payment == 1 || payment == 0.50m || payment == 0.25m || payment == 0.10m || payment == 0.05m || payment == 0.01m) {
                    amountPaid += payment;
                    youPay += payment;
                    _refundMoney = youPay;
                    _personPay = youPay;
                    //Console.WriteLine($"You paid amount of cash: {_personPay}");
                    Console.WriteLine($"\t\t\tRemaining: {owed - amountPaid:c}\n");  
                } else {
                    ColorText("\t\t\tPlease insert correct USD currency.\n", ConsoleColor.DarkRed, true);
                
                }//end else 
            }//end while
            //CALCULATE THE CHANGE TO BE DISPENSED TO THE USER
            if (amountPaid > CalculateTotalVault()) {
                _refundMoney = youPay;
            } 
           
            change = amountPaid - owed;
            _change = change;

            
            //Console.WriteLine($"Your change: {change:c}");
            

        }//end InsertCash 
        #endregion

        #region DISPENSER
        public List<decimal> Dispenser(decimal amountKioskOwed) {
            decimal hun     = _hundredDollar;
            decimal fif     = _fiftyDollar;
            decimal twen    = _twentyDollar;
            decimal ten     = _tenDollar;
            decimal five    = _fiveDollar;
            decimal one     = _oneDollar;
            decimal quart   = _quarter;
            decimal dime    = _dime;
            decimal nick    = _nickel;
            decimal pen     = _penny;


            decimal totalVault = CalculateTotalVault();
            List<decimal> result = new List<decimal>();

            decimal[] denominations = new decimal[] {
                100, 50, 20, 10, 5, 1, 0.25m, 0.10m, 0.05m, 0.01m
            }; //sorted in decreasing order

            //if change needed > totalVal . Refund
            if (amountKioskOwed > totalVault) {
                //REFUND MONEY BACK!!
                _change = 0;
                return result;
            }//end if 

            foreach (decimal denom in denominations) {
                while (amountKioskOwed >= denom) {
                    bool success = false;
                    if (denom == 100 && hun > 0) {
                        success = true;
                        hun -= 1;
                    } else if (denom == 50 && fif > 0) {
                        fif -= 1;
                        success = true;
                    } else if (denom == 20 && twen > 0) {
                        twen -= 1;
                        success = true;
                    } else if (denom == 10 && ten > 0) {
                        ten -= 1;
                        success = true;
                    } else if (denom == 5 && five > 0) {
                        five -= 1;
                        success = true;
                    } else if (denom == 1 && one > 0) {
                        one -= 1;
                        success = true;
                    } else if (denom == 0.25m && quart > 0) {
                        quart -= 1;
                        success = true;
                    } else if (denom == 0.10m && dime > 0) {
                        dime -= 1;
                        success = true;
                    } else if (denom == 0.05m && nick > 0) {
                        nick -= 1;
                        success = true;
                    } else if (denom == 0.01m && pen > 0) {
                        pen -= 1;
                        success = true;
                    }//end if 
                    if (success) {
                        result.Add(denom);
                        amountKioskOwed -= denom;
                        totalVault -= denom;
                    } else {
                        break;
                    }//end method
                        
                }//end while
            }//end for each

            if (amountKioskOwed > 0) {
                //NOT ENOUGH MONEY
                result.Clear();
                _change = 0;
                return result;
            }//end if

            //TESTING AFTER DISPENSE
            //Console.WriteLine($"After Dispense = {totalVault}");//Testing after money away from vault
            _hundredDollar  = hun;
            _fiftyDollar    = fif;
            _twentyDollar   = twen;
            _tenDollar      = ten;
            _fiveDollar     = five;
            _oneDollar      = one;
            _quarter        = quart;
            _dime           = dime;
            _nickel         = nick;
            _penny          = pen;
            return result;
        }//end Dispenser method
        #endregion
        
        #region MONEY REQUEST "DO NOT CHANGE THIS FUNCTION"

        public string[] MoneyRequest(string account_number, decimal amount) {
            Random rnd = new Random();
            //50% CHANCE TRANSACTION PASSES OR FAILS
            bool pass = rnd.Next(100) < 50;
            //bool pass = false;
            //50% CHANCE THAT A FAILED TRANSACTION IS DECLINED
            bool declined = rnd.Next(100) < 50;

            if (pass) {
                return new string[] { account_number, amount.ToString()};
            } else {
                if (!declined) {
                    return new string[] { account_number, (amount / rnd.Next(2, 6)).ToString()};
                } else {
                    return new string[] { account_number, "declined" };
                }//end if 
            }//end if 
        }//end Money Request Function 
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
                if (userInput < 0) {
                    isValid = false;
                    Console.WriteLine("Please enter a valid number.");
                }//end if 
            } while (isValid == false);
            return userInput;
        }//end PromptTryDouble function

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

        #endregion
    }//end CashInventory

        #region CARD IDENTIFY
       /* public void CreditCardVendor() {
            string cardNumber = Prompt("Enter a credit card number: ");

            // Extract the first digit from the card number
            char firstDigit = cardNumber[0];

            // Determine the card vendor based on the first digit
            string vendor;
            switch (firstDigit) {
                case '3':
                    vendor = "American Express";
                    break;
                case '4':
                    vendor = "Visa";
                    break;
                case '5':
                    vendor = "MasterCard";
                    break;
                case '6':
                    vendor = "Discover";
                    break;
                default:
                    vendor = "Unknown";
                    break;
            }

            Console.WriteLine($"Card vendor: {vendor}");
        }
       */
    #endregion

   /* #region TAKE Away Money (Not Using it)
    public void TakeAwayMoney(decimal amountToDeduct) {
        //DEDUCT MONEY FROM VAULT BASED ON THE GIVEN AMOUNT
        while (amountToDeduct > 0) {
            if (amountToDeduct >= 100 && _hundredDollar > 0) {
                _hundredDollar--;
                amountToDeduct -= 100;
            }else if (amountToDeduct >= 50 && _fiftyDollar > 0) {
                _fiftyDollar--;
                amountToDeduct -= 50;
                Console.WriteLine("50 x 1");
            } else if (amountToDeduct >= 20  && _twentyDollar > 0) {
                _twentyDollar--;
                amountToDeduct -= 20;
            }else if (amountToDeduct >= 10  && _tenDollar > 0) {
                _tenDollar--;
                amountToDeduct -= 10;
            }else if (amountToDeduct >= 5 && _fiveDollar > 0) {
                _fiveDollar--;
                amountToDeduct -= 5;
            } else if (amountToDeduct >= 1 && _oneDollar > 0) {
                _oneDollar--;
                amountToDeduct -= 1;
            }else if (amountToDeduct >= 0.25m && _quarter > 0) {
                _quarter--;
                amountToDeduct -= 0.25m;
            }else if (amountToDeduct >= 0.10m && _dime > 0) {
                _dime--;
                amountToDeduct -= 0.10m;
            }else if (amountToDeduct >= 0.05m && _nickel > 0) {
                _nickel--;
                amountToDeduct -= 0.05m;
            }else if (amountToDeduct >= 0.01m && _penny > 0) {
                _penny--;
                amountToDeduct -= 0.01m;
            }else {
                //NOT ENOUGH MONEY TO DEDUCT THE REMAINING VAULT TOTAL VALUE
                throw new InvalidOperationException("Insufficient Money in the vault.");
            }//end else
        }//end while
    }//end Take away money method
    #endregion */
    

    #region INCREMENT AND DECREMENT
    /*
      #region Increments
    public void AddHundred(int count) {
        _hundredDollar += count;
    }//end method

    public void AddFifty(int count) {
        _fiftyDollar += count;
    }//end method 

    public void AddTwenty(int Count) {
        _twentyDollar += Count;
    }//end method

    public void AddTen(int Count) {
        _tenDollar += Count;
    }//end method

    public void AddFive(int Count) {
        _fiveDollar += Count;
    }//end method

    public void AddDollar(int Count) {
        _oneDollar += Count;
    }//end method

    public void AddQuarter(int Count) {
        _quarter += Count;
    }//end method

    public void AddDime(int Count) {
        _dime += Count;
    }//end method

    public void AddNickel(int Count) {
        _nickel += Count;
    }//end method

    public void AddPenny(int Count) {
        _penny += Count;
    }//end method
    #endregion

    #region Decrements
    public void SubHundred(int count) {
        _hundredDollar -= count;
    }//end method

    public void SubFifty(int count) {
        _fiftyDollar -= count;
    }//end method 

    public void SubTwenty(int Count) {
        _twentyDollar -= Count;
    }//end method

    public void SubTen(int Count) {
        _tenDollar -= Count;
    }//end method

    public void SubFive(int Count) {
        _fiveDollar -= Count;
    }//end method

    public void SubDollar(int Count) {
        _oneDollar -= Count;
    }//end method

    public void SubQuarter(int Count) {
        _quarter -= Count;
    }//end method

    public void SubDime(int Count) {
        _dime -= Count;
    }//end method

    public void SubNickel(int Count) {
        _nickel -= Count;
    }//end method

    public void SubPenny(int Count) {
        _penny -= Count;
    }//end method
    #endregion
    *///COMMENTED OUT
    #endregion
    //Example of Return
    /*
    public void Test (int x) {

        if (x > 10) {
            Console.WriteLine("Bigger than 10");
            return; // early return
        }

        while (x < 10) {
            Console.WriteLine("X is not greater than 10!!!");
            x++;
        }//end method


        return; // returns at end of method
    }//end method
    */

}//namespace
