using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactBook
{
    public class ContactBookApp
    {
        private List<Contact> allContacts;
        private int page = 1;
        private int size = 10;

        public const string NEXT = "+";
        public const string PREV = "-";
        public const string EXIT = "Q";

        public const string YES = "Y";
        public const string NO = "N";

        public readonly string[] COMMANDS = { NEXT, PREV, EXIT };
        public readonly string[] YES_NO = { YES, NO };

        public ContactBookApp(List<Contact> contacts)
        {
            allContacts = contacts ?? new List<Contact>();
        }

        public void Start()
        {
            ShowWelcomeScreen();

            string input;

            do
            {
                Console.Clear();
                ShowContacts();

                do
                {
                    ShowInputOptions();
                    input = GetOptions("Select option: ", COMMANDS, EXIT);
                }
                while (!IsValidInput(input));

                ProcessInput(input);

            } while (!ConfirmExit());

            ShowExitScreen();
        }

        private void ShowWelcomeScreen()
        {
            Console.WriteLine("====================================");
            Console.WriteLine("     Hector Santos Contact Book     ");
            Console.WriteLine("====================================");
            PressEnterToContinue();
        }

        private void ShowContacts()
        {
            if (allContacts.Count == 0)
            {
                Console.WriteLine("No contacts found.");
                return;
            }

            int indexCol = allContacts.Count.ToString().Length + 2;
            int fnameCol = Math.Max("First Name".Length, allContacts.Max(c => c.GetFName()?.Length ?? 0)) + 2;
            int lnameCol = Math.Max("Last Name".Length, allContacts.Max(c => c.GetLName()?.Length ?? 0)) + 2;
            int phoneCol = Math.Max("Phone".Length, allContacts.Max(c => c.GetPhone()?.Length ?? 0)) + 2;
            int emailCol = Math.Max("Email".Length, allContacts.Max(c => c.GetEmail()?.Length ?? 0)) + 2;

            string format = $"{{0,-{indexCol}}}{{1,-{fnameCol}}}{{2,-{lnameCol}}}{{3,-{phoneCol}}}{{4,-{emailCol}}}";

            Console.WriteLine(format, "#", "First Name", "Last Name", "Phone", "Email");

            int totalWidth = indexCol + fnameCol + lnameCol + phoneCol + emailCol;
            Console.WriteLine(new string('-', totalWidth));

            int n = allContacts.Count;
            int pageCount = (int)Math.Max(1, Math.Ceiling(n / (double)size));
            int s = Math.Clamp((page - 1) * size, 0, n);
            int e = Math.Clamp(s + size, 0, n);

            for (int i = s; i < e; i++)
            {
                var c = allContacts[i];
                Console.WriteLine(format,
                    (i + 1),
                    c.GetFName(),
                    c.GetLName(),
                    c.GetPhone(),
                    c.GetEmail());
            }

            Console.WriteLine(new string('-', totalWidth));
            Console.WriteLine($"\nPage {page} of {pageCount} ({s + 1}-{e} of {n})");
        }

        private void ShowInputOptions()
        {
            Console.WriteLine("\nINPUT OPTIONS:");
            Console.WriteLine("[+] Next Page | [-] Prev Page | [Q] Exit");
        }

        private string GetOptions(string prompt, string[] validOptions, string defaultOption)
        {
            string options = string.Join('/', validOptions);

            Console.Write($"{prompt} [{options}] ({defaultOption}): ");

            string option = Console.ReadLine()!.ToUpper().Trim();

            if (string.IsNullOrWhiteSpace(option))
            {
                option = defaultOption;
            }

            while (!validOptions.Contains(option))
            {
                Console.WriteLine("ERROR: Invalid option. Please try again.");
                Console.Write($"{prompt} [{options}] ({defaultOption}): ");

                option = Console.ReadLine()!.ToUpper().Trim();

                if (string.IsNullOrWhiteSpace(option))
                {
                    option = defaultOption;
                }
            }

            return option;
        }

        private bool IsValidInput(string input)
        {
            if (!COMMANDS.Contains(input))
            {
                Console.WriteLine("ERROR: Invalid input.");
                return false;
            }
            return true;
        }

        private void ProcessInput(string input)
        {
            if (input == NEXT)
            {
                page++;
            }
            else if (input == PREV)
            {
                if (page > 1) page--;
            }
        }

        private bool ConfirmExit()
        {
            return Confirm("Do you want to exit? (Y/N): ", NO);
        }

        private bool Confirm(string prompt, string defaultOption)
        {
            string response = GetOptions(prompt, YES_NO, defaultOption);
            return response == YES;
        }

        private void PressEnterToContinue()
        {
            Console.WriteLine("\nPress Enter to continue...");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        }

        private void ShowExitScreen()
        {
            Console.WriteLine("\nGoodbye!");
        }
    }
}