using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactBook
{
    public class ContactBookApp
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

        public readonly string[] COMMANDS = new string[]
        {
            NEXT_PAGE, PREV_PAGE, GOTO_PAGE, PAGE_SIZE,
            CREATE_CONTACT, REVIEW_CONTACT, UPDATE_CONTACT,
            DELETE_CONTACT, FIND_CONTACTS, ORDER_CONTACTS,
            DEDUPLICATE_CONTACTS, EXIT
        };

        public ContactBookApp(List<Contact> contacts)
        {
            _allContacts = contacts ?? new List<Contact>();
        }

        public void Start()
{
    ShowWelcomeScreen();

    string input;

    do
    {
        ShowContacts();

        do
        {
            ShowInputOptions();
            input = GetInput();
        }
        while (!IsValidInput(input));

        ProcessInput(input);

    }
    while (!ConfirmExit());

    ShowExitScreen();
}

        private bool ConfirmExit()
        {
            throw new NotImplementedException();
        }

        private bool IsValidInput(string input)
        {
            throw new NotImplementedException();
        }

        private string GetInput()
        {
            throw new NotImplementedException();
        }

        private void ShowInputOptions()
        {
            throw new NotImplementedException();
        }

        private void ShowWelcomeScreen()
        {
            Console.WriteLine("====================================");
            Console.WriteLine(" Welcome to Hector's Contact Book ");
            Console.WriteLine("====================================");
            PressEnterToContinue();
        }

        private void ShowContacts()
        {
            if (_allContacts.Count == 0)
            {
                Console.WriteLine("No contacts found.");
                return;
            }

            int pageCount = (int)Math.Ceiling((double)_allContacts.Count / _size);
            int start = (_page - 1) * _size;
            int end = Math.Min(start + _size, _allContacts.Count);

            int indexWidth = _allContacts.Count.ToString().Length + 2;
            int nameWidth = Math.Max("First Name".Length, _allContacts.Max(c => c.GetFName().Length)) + 2;
            int lastNameWidth = Math.Max("Last Name".Length, _allContacts.Max(c => c.GetLName().Length)) + 2;
            int phoneWidth = Math.Max("Phone".Length, _allContacts.Max(c => c.GetPhone().Length)) + 2;
            int emailWidth = Math.Max("Email".Length, _allContacts.Max(c => c.GetEmail().Length)) + 2;

            string headerFormat = $"{{0,-{indexWidth}}} {{1,-{nameWidth}}} {{2,-{lastNameWidth}}} {{3,-{phoneWidth}}} {{4,-{emailWidth}}}";
            Console.WriteLine(headerFormat, "#", "First Name", "Last Name", "Phone", "Email");

            int totalWidth = indexWidth + nameWidth + lastNameWidth + phoneWidth + emailWidth;
            Console.WriteLine(new string('─', totalWidth));

            for (int i = start; i < end; i++)
            {
                var c = _allContacts[i];
                Console.WriteLine(headerFormat,
                    (i + 1),
                    c.GetFName(),
                    c.GetLName(),
                    c.GetPhone(),
                    c.GetEmail());
            }

            Console.WriteLine(new string('─', totalWidth));
            Console.WriteLine($"Page {_page} of {pageCount} (Showing {start + 1}-{end} of {_allContacts.Count} contacts)");
        }

        private string GetUserInput()
        {
            Console.WriteLine($"\nCommands: [{NEXT_PAGE}] Next | [{PREV_PAGE}] Prev | [{EXIT}] Exit");
            Console.Write("Select an option: ");
            return Console.ReadLine() ?? "";
        }

        private void ProcessInput(string input)
        {
            if (input == NEXT_PAGE)
            {
                int pageCount = (int)Math.Ceiling((double)_allContacts.Count / _size);
                if (_page < pageCount) _page++;
            }
            else if (input == PREV_PAGE)
            {
                if (_page > 1) _page--;
            }
        }

        private void PressEnterToContinue()
        {
            Console.WriteLine("\nPress Enter to continue...");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        }

        private void ShowExitScreen()
        {
            Console.WriteLine("\nThank you for using Contact Book. Goodbye!");
        }
    }
}