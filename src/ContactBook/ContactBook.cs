using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactBook
{
    public class ContactBook
    {
        private List<Contact> _allContacts;
        private int _page = 1;
        private int _size = 10;

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

        public readonly string[] COMMANDS = new string[]
        {
            NEXT_PAGE, PREV_PAGE, GOTO_PAGE, PAGE_SIZE,
            CREATE_CONTACT, REVIEW_CONTACT, UPDATE_CONTACT,
            DELETE_CONTACT, FIND_CONTACTS, ORDER_CONTACTS,
            DEDUPLICATE_CONTACTS, EXIT
        };

        public readonly string[] YES_NO_OPTIONS = new string[] { YES, NO };

        public ContactBook(List<Contact> contacts = null!)
        {
            _allContacts = contacts ?? new List<Contact>();
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
                        if (ConfirmExit()) running = false;
                    }
                    else
                    {
                        ProcessInput(input);
                    }
                }
                else
                {
                    Console.WriteLine("\nError: Invalid input. Please try again.");
                    PressEnterToContinue();
                }
            }
            ShowExitScreen();
        }

        private void ShowInputOptions()
        {
            Console.WriteLine("\nINPUT OPTIONS:");
            Console.WriteLine($"{NEXT_PAGE}: Next Page | {PREV_PAGE}: Prev Page | {GOTO_PAGE}: Go To Page | {PAGE_SIZE}: Change Page Size");
            Console.WriteLine($"{CREATE_CONTACT}: Create | {REVIEW_CONTACT}: Review | {UPDATE_CONTACT}: Update | {DELETE_CONTACT}: Delete");
            Console.WriteLine($"{FIND_CONTACTS}: Find | {ORDER_CONTACTS}: Order | {DEDUPLICATE_CONTACTS}: De-duplicate | {EXIT}: Exit");
        }

        private string GetInput()
        {
            Console.Write("\nSelect option: ");
            return Console.ReadLine()?.ToUpper().Trim() ?? "";
        }

        private bool IsValidInput(string input)
        {
            return COMMANDS.Contains(input);
        }

        private void ProcessInput(string input)
        {
            switch (input)
            {
                case NEXT_PAGE: NextPage(); break;
                case PREV_PAGE: PreviousPage(); break;
                case GOTO_PAGE: GoToPage(); break;
                case PAGE_SIZE: ChangePageSize(); break;
                case CREATE_CONTACT: CreateContact(); break;
                case REVIEW_CONTACT: ReviewContact(); break;
                case UPDATE_CONTACT: UpdateContact(); break;
                case DELETE_CONTACT: DeleteContact(); break;
                case FIND_CONTACTS: FindContacts(); break;
                case ORDER_CONTACTS: OrderContacts(); break;
                case DEDUPLICATE_CONTACTS: DeDuplicateContacts(); break;
            }
        }

        private void NextPage()
        {
            int pageCount = (int)Math.Max(1, Math.Ceiling(_allContacts.Count / (double)_size));
            if (_page < pageCount) _page++;
        }

        private void PreviousPage()
        {
            if (_page > 1) _page--;
        }

        private void GoToPage() { Console.WriteLine("Action: Go To Page"); PressEnterToContinue(); }
        private void ChangePageSize() { Console.WriteLine("Action: Change Page Size"); PressEnterToContinue(); }
        private void CreateContact() { Console.WriteLine("Action: Create Contact"); PressEnterToContinue(); }
        private void ReviewContact() { Console.WriteLine("Action: Review Contact"); PressEnterToContinue(); }
        private void UpdateContact() { Console.WriteLine("Action: Update Contact"); PressEnterToContinue(); }
        private void DeleteContact() { Console.WriteLine("Action: Delete Contact"); PressEnterToContinue(); }
        private void FindContacts() { Console.WriteLine("Action: Find Contacts"); PressEnterToContinue(); }
        private void OrderContacts() { Console.WriteLine("Action: Order Contacts"); PressEnterToContinue(); }
        private void DeDuplicateContacts() { Console.WriteLine("Action: De-duplicate Contacts"); PressEnterToContinue(); }

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
            Console.Write($"{prompt} [{optionsStr}] ({defaultOption}): ");
            string input = Console.ReadLine()?.ToUpper().Trim() ?? "";

            if (string.IsNullOrWhiteSpace(input)) input = defaultOption;

            while (!validOptions.Contains(input))
            {
                Console.WriteLine("Invalid option. Please try again.");
                Console.Write($"{prompt} [{optionsStr}] ({defaultOption}): ");
                input = Console.ReadLine()?.ToUpper().Trim() ?? "";
                if (string.IsNullOrWhiteSpace(input)) input = defaultOption;
            }

            return input;
        }

        private void ShowContacts()
        {
            if (_allContacts.Count == 0)
            {
                Console.WriteLine("No contacts found.");
                return;
            }

            int indexCol = _allContacts.Count.ToString().Length;
            int fnameCol = Math.Max("First Name".Length, _allContacts.Max(c => c.GetFName()?.Length ?? 0));
            int lnameCol = Math.Max("Last Name".Length, _allContacts.Max(c => c.GetLName()?.Length ?? 0));
            int phoneCol = Math.Max("Phone".Length, _allContacts.Max(c => c.GetPhone()?.Length ?? 0));
            int emailCol = Math.Max("Email".Length, _allContacts.Max(c => c.GetEmail()?.Length ?? 0));

            string format = $"{{0, -{indexCol}}} {{1, -{fnameCol}}} {{2, -{lnameCol}}} {{3, -{phoneCol}}} {{4, -{emailCol}}}";

            Console.WriteLine(format, "#", "First Name", "Last Name", "Phone", "Email");
            Console.WriteLine(new string('-', indexCol + fnameCol + lnameCol + phoneCol + emailCol + 8));

            int n = _allContacts.Count;
            int pageCount = (int)Math.Max(1, Math.Ceiling(n / (double)_size));
            int s = Math.Clamp((_page - 1) * _size, 0, n);
            int e = Math.Clamp(s + _size, 0, n);

            for (int i = s; i < e; i++)
            {
                Contact c = _allContacts[i];
                Console.WriteLine(format, (i + 1), c.GetFName(), c.GetLName(), c.GetPhone(), c.GetEmail());
            }

            Console.WriteLine($"\nPage {_page} of {pageCount} ({s + 1}-{e} of {n})");
        }

        private void ShowWelcomeScreen()
        {
            Console.WriteLine("====================================");
            Console.WriteLine(" Welcome to Hector Santos Contact Book ");
            Console.WriteLine("====================================");
            PressEnterToContinue();
        }

        private void PressEnterToContinue()
        {
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        private void ShowExitScreen()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("Thank you for using Hector Santos Contact Book");
            Console.WriteLine("========================================");
            Console.WriteLine("Goodbye!");
        }
    }
}