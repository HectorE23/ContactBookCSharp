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

        public ContactBook(List<Contact> contacts = null!)
        {
            allContacts = (contacts == null) ? new List<Contact>() : contacts;
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
                    input = GetInput();
                }
                while (!IsValidInput(input));

                ProcessInput(input);

            } while (!ConfirmExit());

            ShowExitScreen();
        }

        private void ShowWelcomeScreen()
        {
            Console.WriteLine("====================================");
            Console.WriteLine(" Welcome to Contact Book ");
            Console.WriteLine("====================================");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private void ShowContacts()
        {
            if (allContacts.Count == 0)
            {
                Console.WriteLine("No contacts found.");
                return;
            }

            int indexCol = allContacts.Count.ToString().Length;
            int fnameCol = Math.Max("First Name".Length, allContacts.Max(c => c.GetFName()?.Length ?? 0));
            int lnameCol = Math.Max("Last Name".Length, allContacts.Max(c => c.GetLName()?.Length ?? 0));
            int phoneCol = Math.Max("Phone".Length, allContacts.Max(c => c.GetPhone()?.Length ?? 0));
            int emailCol = Math.Max("Email".Length, allContacts.Max(c => c.GetEmail()?.Length ?? 0));

            Console.WriteLine(""
                + "{0, " + -indexCol + "} "
                + "{1, " + -fnameCol + "} "
                + "{2, " + -lnameCol + "} "
                + "{3, " + -phoneCol + "} "
                + "{4, " + -emailCol + "} ",
                "#", "First Name", "Last Name", "Phone", "Email");

            Console.WriteLine(new string('-', indexCol + 2 + fnameCol + 2 + lnameCol + 2 + phoneCol + 2 + emailCol));

            int n = allContacts.Count;
            int pageCount = (int)Math.Max(1, Math.Ceiling(n / (double)size));

            int s = Math.Clamp((page - 1) * size, 0, n);
            int e = Math.Clamp(s + size, 0, n);

            for (int i = s; i < e; i++)
            {
                Contact c = allContacts[i];

                Console.WriteLine(""
                    + "{0, " + -indexCol + "} "
                    + "{1, " + -fnameCol + "} "
                    + "{2, " + -lnameCol + "} "
                    + "{3, " + -phoneCol + "} "
                    + "{4, " + -emailCol + "} ",
                    (i + 1),c.GetFName(),c.GetLName(),c.GetPhone(),c.GetEmail());
            }

            Console.WriteLine();
            Console.WriteLine($"Page {page} of {pageCount} ({s + 1}-{e} of {n})");
        }

        private void ShowInputOptions()
        {
            Console.WriteLine("\nCommands: [+] Next | [-] Prev | [Q] Quit");
        }

        private string GetInput()
        {
            Console.Write("Select option: ");
            return Console.ReadLine() ?? "";
        }

        private bool IsValidInput(string input)
        {
            return input == "+" || input == "-" || input.ToUpper() == "Q";
        }

        private void ProcessInput(string input)
        {
            int n = allContacts.Count;
            int pageCount = (int)Math.Max(1, Math.Ceiling(n / (double)size));

            if (input == "+")
            {
                if (page < pageCount) page++;
            }
            else if (input == "-")
            {
                if (page > 1) page--;
            }
        }

        private bool ConfirmExit()
        {
            Console.Write("Exit? (Q to confirm): ");
            return Console.ReadLine()?.ToUpper() == "Q";
        }

        private void ShowExitScreen()
        {
            Console.WriteLine("Goodbye!");
        }
    }
}