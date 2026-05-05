using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactBook
{
    public class ContactBook
    {
        private List<Contact> allContacts;
        private int page = 1;
        private int size = 10;

        public const string NEXT_PAGE = "+";
        public const string PREV_PAGE = "-";
        public const string GOTO_PAGE = "G";
        public const string PAGE_SIZE = "S";
        public const string CREATE_CONTACT = "C";
        public const string REVIEW_CONTACT = "R";
        public const string UPDATE_CONTACT = "U";
        public const string DELETE_CONTACT = "D";
        public const string FIND_CONTACTS = "F";
        public const string ORDER_CONTACTS = "O";
        public const string DEDUPLICATE_CONTACTS = "M";
        public const string EXIT = "X";
        public const string YES = "Y";
        public const string NO = "N";

        public readonly string[] COMMANDS = { NEXT_PAGE, PREV_PAGE, GOTO_PAGE, PAGE_SIZE, CREATE_CONTACT, REVIEW_CONTACT, UPDATE_CONTACT, DELETE_CONTACT, FIND_CONTACTS, ORDER_CONTACTS, DEDUPLICATE_CONTACTS, EXIT };
        public readonly string[] YES_NO_OPTIONS = { YES, NO };

        public ContactBook(List<Contact> contacts = null!)
        {
            allContacts = contacts ?? new List<Contact>();
        }

        public void Start()
        {
            ShowWelcomeScreen();

            bool running = true;

            while (running)
            {
                Console.Clear();              

                ShowContacts();               
                ShowInputOptions();           

                string input = GetInput();    

                if (IsValidInput(input))
                {
                    if (input == EXIT)
                    {
                        if (ConfirmExit())
                            running = false;
                    }
                    else
                    {
                        ProcessInput(input);
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: Invalid input.");
                    PressEnterToContinue();
                }
            }

            ShowExitScreen();
        }

        private void CreateContact()
        {
            Console.Clear();

            Console.WriteLine("====================================");
            Console.WriteLine("     CREATE NEW CONTACT (HECTOR)    ");
            Console.WriteLine("====================================");

            string fName = ReadString("First Name");
            string lName = ReadString("Last Name");
            string phone = ReadPhone("Phone Number");
            string email = ReadEmail("Email Address");

            Contact newContact = new Contact(fName, lName, phone, email);
            allContacts.Add(newContact);

            Console.WriteLine("\n[Success] Contact added!");
            PressEnterToContinue();
        }

        private string ReadString(string label)
        {
            string result;

            do
            {
                Console.Write($"{label}: ");
                result = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrEmpty(result))
                {
                    Console.WriteLine("Error: Cannot be empty.");
                }

            } while (string.IsNullOrEmpty(result));

            return result;
        }

        private string ReadPhone(string label)
        {
            return ReadString(label);
        }

        private string ReadEmail(string label)
        {
            string email;
            bool isValid = false;

            do
            {
                email = ReadString(label);

                if (email.Contains("@") && email.Contains("."))
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("Error: Invalid email.");
                }

            } while (!isValid);

            return email;
        }

        private void ProcessInput(string input)
        {
            switch (input)
            {
                case NEXT_PAGE:
                    NextPage();
                    break;

                case PREV_PAGE:
                    PrevPage();
                    break;

                case CREATE_CONTACT:
                    CreateContact();
                    break;

                default:
                    Console.WriteLine($"Action '{input}' not implemented yet.");
                    PressEnterToContinue();
                    break;
            }
        }

        private void NextPage()
        {
            NextPage(allContacts, ref page, size);
        }

        private void NextPage(List<Contact> contacts, ref int page, int size)
        {
            page = Math.Clamp(page + 1, 1, PageCount(contacts, size));
        }

        private void PrevPage()
        {
            PrevPage(allContacts, ref page);
        }

        private void PrevPage(List<Contact> contacts, ref int page)
        {
            page = Math.Clamp(page - 1, 1, PageCount(contacts, size));
        }

        private void GotoPage()
        {
            Console.WriteLine("Goto Page");
        }

        private bool ConfirmExit()
        {
            return Confirm("Do you want to exit?", NO);
        }

        private bool Confirm(string prompt, string defaultOption)
        {
            string response = GetOption(prompt, YES_NO_OPTIONS, defaultOption);
            return response == YES;
        }

        private string GetOption(string prompt, string[] validOptions, string defaultOption)
        {
            string optionsStr = string.Join("/", validOptions);

            while (true)
            {
                Console.Write($"{prompt} [{optionsStr}] ({defaultOption}): ");

                string input = Console.ReadLine()?.ToUpper().Trim() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                    return defaultOption;

                if (validOptions.Contains(input))
                    return input;

                Console.WriteLine("Invalid option.");
            }
        }

        private void ShowContacts()
        {
            ShowContacts(allContacts, page, size);
        }

        private void ShowContacts(List<Contact> contacts, int page, int size)
        {
            if (contacts.Count <= 0)
            {
                Console.WriteLine("No contacts found.");
                return;
            }

            int indexCol = Math.Max("#".Length, contacts.Count.ToString().Length);
            int fnameCol = Math.Max("First Name".Length, contacts.Max(c => c.GetFName()?.Length ?? 0));
            int lnameCol = Math.Max("Last Name".Length, contacts.Max(c => c.GetLName()?.Length ?? 0));
            int phoneCol = Math.Max("Phone".Length, contacts.Max(c => c.GetPhone()?.Length ?? 0));
            int emailCol = Math.Max("Email".Length, contacts.Max(c => c.GetEmail()?.Length ?? 0));

            Console.WriteLine(""
                + "{0," + -indexCol + "} "
                + "{1," + -fnameCol + "} "
                + "{2," + -lnameCol + "} "
                + "{3," + -phoneCol + "} "
                + "{4," + -emailCol + "} ",
                "#", "First Name", "Last Name", "Phone", "Email");

            Console.WriteLine(new string('-', indexCol + fnameCol + lnameCol + phoneCol + emailCol + 4));

            int n = contacts.Count;
            int pageCount = PageCount(contacts, size);
            int s = Math.Clamp((page - 1) * size, 0, n);
            int e = Math.Clamp(s + size, 0, n);

            for (int i = s; i < e; i++)
            {
                Contact c = contacts[i];

                Console.WriteLine(""
                    + "{0," + -indexCol + "} "
                    + "{1," + -fnameCol + "} "
                    + "{2," + -lnameCol + "} "
                    + "{3," + -phoneCol + "} "
                    + "{4," + -emailCol + "} ",
                    (i + 1),
                    c.GetFName(),
                    c.GetLName(),
                    c.GetPhone(),
                    c.GetEmail());
            }

            Console.WriteLine();
            Console.WriteLine($"Page {page} of {pageCount} ({s + 1}-{e} of {n})");
        }

        private int PageCount(List<Contact> contacts, int size)
        {
            return (int)Math.Max(1, Math.Ceiling(contacts.Count / (double)size));
        }

        private static int NewMethod(int size, int n)
        {
            return (int)Math.Max(1, Math.Ceiling(n / (double)size));
        }

        private void ShowInputOptions()
        {
            Console.WriteLine();
            Console.WriteLine("[+] Next Page | [C] Create Contact | [D] Delete Contact | [M] Deduplicate Contacts");
            Console.WriteLine("[-] Prev Page | [R] Review Contact | [F] Find Contacts | [S] Change Page Size");
            Console.WriteLine("[G] Goto Page | [U] Update Contact | [O] Order Contacts | [X] Exit");
            Console.WriteLine();
        }
        private string GetInput()
        {
            Console.Write(">> ");
            return Console.ReadLine()?.ToUpper().Trim() ?? "";
        }

        private bool IsValidInput(string input)
        {
            return COMMANDS.Contains(input);
        }

        private void ShowWelcomeScreen()
        {
            Console.WriteLine("Welcome to Hector's Contact Book");
        }

        private void PressEnterToContinue()
        {
            Console.WriteLine("Press Enter...");
            Console.ReadLine();
        }

        private void ShowExitScreen()
        {
            Console.WriteLine("\nThanks for using Hector's Contact Book!");
        }
    }
}