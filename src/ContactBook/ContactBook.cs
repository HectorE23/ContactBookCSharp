using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactBook
{
    public class ContactBookApp
    {
        private List<Contact> allContacts = new List<Contact>();
        private List<Contact> filteredContacts = new List<Contact>();
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

        public readonly string[] COMMANDS = {
            NEXT_PAGE, PREV_PAGE, GOTO_PAGE, PAGE_SIZE,
            CREATE_CONTACT, REVIEW_CONTACT, UPDATE_CONTACT,
            DELETE_CONTACT, FIND_CONTACTS, ORDER_CONTACTS,
            DEDUPLICATE_CONTACTS, EXIT
        };

        public readonly string[] YES_NO_OPTIONS = { YES, NO };

        public ContactBookApp(List<Contact> contacts = null!)
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
                        if (ConfirmExit()) running = false;
                    }
                    else
                    {
                        ProcessInput(input);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                    PressEnterToContinue();
                }
            }

            ShowExitScreen();
        }

        // ================= CREATE =================

        private void CreateContact()
        {
            Console.Clear();

            Console.WriteLine("Create Contact");
            Console.WriteLine(new string('#', 80));
            Console.WriteLine();

            Console.Write("Enter first name: ");
            string fname = Console.ReadLine()!;
            Console.Write("Enter last name: ");
            string lname = Console.ReadLine()!;
            Console.Write("Enter phone: ");
            string phone = Console.ReadLine()!;
            Console.Write("Enter email: ");
            string email = Console.ReadLine()!;

            if (Confirm("Do you want to create this contact?", YES))
            {
                CreateContact(fname, lname, phone, email);
                Console.WriteLine("Contact created.");
            }
            else
            {
                Console.WriteLine("Operation cancelled.");
            }

            PressEnterToContinue();
        }

        private void CreateContact(string fname, string lname, string phone, string email)
        {
            Contact c = new Contact(fname, lname, phone, email);
            allContacts.Add(c);
        }

        // ================= REVIEW =================

        private void ReviewContact(int index)
        {
            var list = filteredContacts.Count > 0 ? filteredContacts : allContacts;
            Contact c = list[index];

            Console.WriteLine(new string('#', 80));
            Console.WriteLine("Review Contact");
            Console.WriteLine(new string('#', 80));
            Console.WriteLine();

            Console.WriteLine($"First name: {c.GetFName()}");
            Console.WriteLine($"Last name: {c.GetLName()}");
            Console.WriteLine($"Phone: {c.GetPhone()}");
            Console.WriteLine($"Email: {c.GetEmail()}");

            Console.WriteLine();
        }

        private void ReviewContact()
        {
            var list = filteredContacts.Count > 0 ? filteredContacts : allContacts;

            if (list.Count == 0)
            {
                Console.WriteLine("No contacts found.");
                PressEnterToContinue();
                return;
            }

            int totalPages = (int)Math.Ceiling((double)list.Count / size);

            int start = (page - 1) * size;
            int end = Math.Min(start + size, list.Count);

            Console.Clear();

            Console.WriteLine("# First Name Last Name Phone Email");
            Console.WriteLine(new string('-', 80));

            for (int i = start; i < end; i++)
            {
                Contact c = list[i];

                Console.WriteLine(
                    $"{i + 1,-3} " +
                    $"{c.GetFName(),-10} " +
                    $"{c.GetLName(),-10} " +
                    $"{c.GetPhone(),-12} " +
                    $"{c.GetEmail(),-25}"
                );
            }

            Console.WriteLine();
            Console.WriteLine($"Page {page} of {totalPages}");
            Console.WriteLine();

            Console.WriteLine("[+] Next Page | [-] Prev Page | [C] Create Contact | [D] Delete Contact | [M] Deduplicate Contacts");
            Console.WriteLine("[R] Review Contact | [F] Find Contacts | [S] Change Page Size");
            Console.WriteLine("[G] Goto Page | [U] Update Contact | [O] Order Contacts | [X] Exit");
        }

        // ================= UPDATE =================

        private void UpdateContact()
        {
            if (allContacts.Count == 0)
            {
                Console.WriteLine("No contacts.");
                PressEnterToContinue();
                return;
            }

            int index = GetInt("Enter index", 1, allContacts.Count) - 1;

            Console.Clear();
            UpdateContact(index);
        }

        private void UpdateContact(int index)
        {
            Contact c = allContacts[index];

            string fname = c.GetFName();
            string lname = c.GetLName();
            string phone = c.GetPhone();
            string email = c.GetEmail();

            ReviewContact(index);

            Console.WriteLine(new string('#', 80));
            Console.WriteLine("Update Contact");
            Console.WriteLine(new string('#', 80));
            Console.WriteLine();

            if (Confirm("Do you want to edit the first name?", NO))
            {
                Console.Write("Enter first name: ");
                fname = Console.ReadLine()!;
            }

            if (Confirm("Do you want to edit the last name?", NO))
            {
                Console.Write("Enter last name: ");
                lname = Console.ReadLine()!;
            }

            if (Confirm("Do you want to edit the phone?", NO))
            {
                Console.Write("Enter phone: ");
                phone = Console.ReadLine()!;
            }

            if (Confirm("Do you want to edit the email?", NO))
            {
                Console.Write("Enter email: ");
                email = Console.ReadLine()!;
            }

            if (Confirm("Do you want to update this contact?", YES))
            {
                c.SetFName(fname);
                c.SetLName(lname);
                c.SetPhone(phone);
                c.SetEmail(email);

                Console.WriteLine("Operation successful: Contact updated.");
            }
            else
            {
                Console.WriteLine("Operation cancelled.");
            }

            PressEnterToContinue();
        }
        // ================= DELETE =================

        private void DeleteContact()
        {
            if (allContacts.Count == 0)
            {
                Console.WriteLine("No contacts.");
                PressEnterToContinue();
                return;
            }

            int index = GetInt("Enter index", 1, allContacts.Count) - 1;

            Console.Clear();
            DeleteContact(index);
            PressEnterToContinue();
        }

        private void DeleteContact(int index)
        {
            Contact c = allContacts[index];

            if (Confirm($"Do you want to delete {c.GetFName()} {c.GetLName()}?", NO))
            {
                allContacts.RemoveAt(index);
                Console.WriteLine("Contact deleted.");
            }
            else
            {
                Console.WriteLine("Operation cancelled.");
            }
        }

        // ================= FIND =================

        private void FindContacts()
        {
            Console.Write("Enter search term: ");
            string searchTerm = Console.ReadLine()!.ToLower();

            Console.WriteLine();

            if (Confirm("Do you want to search contacts?", YES))
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    filteredContacts.Clear();
                    page = 1;
                    Console.WriteLine("Search cleared.");
                    return;
                }

                filteredContacts = allContacts.FindAll(c =>
                    (c.GetFName() + c.GetLName() + c.GetPhone() + c.GetEmail())
                    .ToLower()
                    .Contains(searchTerm)
                );

                page = 1;
                Console.WriteLine("Operation successful: Contact searched.");
            }
            else
            {
                Console.WriteLine("Operation cancelled: Contact not searched.");
            }
        }

        // ================= ORDER =================

        private void OrderContacts()
        {
            allContacts = allContacts
                .OrderBy(c => c.GetFName())
                .ThenBy(c => c.GetLName())
                .ToList();

            Console.WriteLine("Contacts ordered.");
            PressEnterToContinue();
        }

        // ================= DEDUP =================

        private void DeduplicateContacts()
        {
            allContacts = allContacts
                .GroupBy(c => $"{c.GetFName()}-{c.GetLName()}-{c.GetPhone()}")
                .Select(g => g.First())
                .ToList();

            Console.WriteLine("Duplicates removed.");
            PressEnterToContinue();
        }

        // ================= NAV =================

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
            PrevPage(allContacts, ref page, size);
        }

        private void PrevPage(List<Contact> contacts, ref int page, int size)
        {
            page = Math.Clamp(page - 1, 1, PageCount(contacts, size));
        }

        private void GotoPage()
        {
            page = GetInt("Enter page", 1, PageCount(allContacts, size));
        }

        private void PageSize()
        {
            int max = Console.WindowHeight - 10;
            size = GetInt("Enter page size", 1, max);
            page = 1;
        }

        private int PageCount(List<Contact> contacts, int size)
        {
            return (int)Math.Max(1, Math.Ceiling(contacts.Count / (double)size));
        }

        // ================= UI =================

        private void ShowContacts()
        {
            var contacts = filteredContacts.Count > 0 ? filteredContacts : allContacts;
            ShowContacts(contacts, page, size);
        }

        private void ShowContacts(List<Contact> contacts, int page, int size)
        {
            if (contacts.Count == 0)
            {
                Console.WriteLine("No contacts.");
                return;
            }

            int start = (page - 1) * size;
            int end = Math.Min(start + size, contacts.Count);

            int indexCol = Math.Max("#".Length, contacts.Count.ToString().Length);
            int fnameCol = Math.Max("First Name".Length, contacts.Max(c => c.GetFName().Length));
            int lnameCol = Math.Max("Last Name".Length, contacts.Max(c => c.GetLName().Length));
            int phoneCol = Math.Max("Phone".Length, contacts.Max(c => c.GetPhone().Length));
            int emailCol = Math.Max("Email".Length, contacts.Max(c => c.GetEmail().Length));

            Console.WriteLine(
                $"{{0,-{indexCol}}} {{1,-{fnameCol}}} {{2,-{lnameCol}}} {{3,-{phoneCol}}} {{4,-{emailCol}}}",
                "#", "First Name", "Last Name", "Phone", "Email"
            );

            Console.WriteLine(new string('-', indexCol + fnameCol + lnameCol + phoneCol + emailCol + 4));

            for (int i = start; i < end; i++)
            {
                var c = contacts[i];

                Console.WriteLine(
                    $"{{0,-{indexCol}}} {{1,-{fnameCol}}} {{2,-{lnameCol}}} {{3,-{phoneCol}}} {{4,-{emailCol}}}",
                    i + 1,
                    c.GetFName(),
                    c.GetLName(),
                    c.GetPhone(),
                    c.GetEmail()
                );
            }

            Console.WriteLine($"\nPage {page} of {PageCount(contacts, size)}");
        }

        private void ShowInputOptions()
        {
            Console.WriteLine();
            Console.WriteLine("[+] Next Page | [C] Create Contact | [D] Delete Contact | [M] Deduplicate Contacts");
            Console.WriteLine("[-] Prev Page | [R] Review Contact | [F] Find Contacts | [S] Change Page Size");
            Console.WriteLine("[G] Goto Page | [U] Update Contact | [O] Order Contacts | [X] Exit");
            Console.WriteLine();
        }

        // ================= INPUT =================

        private string GetInput()
        {
            Console.Write(">> ");
            return Console.ReadLine()!.ToUpper();
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
                case PREV_PAGE: PrevPage(); break;
                case GOTO_PAGE: GotoPage(); break;
                case PAGE_SIZE: PageSize(); break;
                case CREATE_CONTACT: CreateContact(); break;
                case REVIEW_CONTACT: ReviewContact(); break;
                case UPDATE_CONTACT: UpdateContact(); break;
                case DELETE_CONTACT: DeleteContact(); break;
                case FIND_CONTACTS: FindContacts(); break;
                case ORDER_CONTACTS: OrderContacts(); break;
                case DEDUPLICATE_CONTACTS: DeduplicateContacts(); break;
            }
        }

        // ================= HELPERS =================

        private int GetInt(string prompt, int min, int max)
        {
            int value;

            Console.Write($"{prompt} [{min}-{max}]: ");

            while (!int.TryParse(Console.ReadLine(), out value) || value < min || value > max)
            {
                Console.WriteLine("Invalid number.");
                Console.Write($"{prompt} [{min}-{max}]: ");
            }

            return value;
        }

        private bool Confirm(string prompt, string defaultOption)
        {
            Console.Write($"{prompt} [Y/N] ({defaultOption}): ");
            string input = Console.ReadLine()!.ToUpper();

            if (string.IsNullOrWhiteSpace(input))
                return defaultOption == "Y";

            return input == "Y";
        }

        private bool ConfirmExit()
        {
            return Confirm("Do you want to exit?", NO);
        }

        private void ShowWelcomeScreen()
        {
            Console.WriteLine("Welcome to Hector's Contact Book");
            PressEnterToContinue();
        }

        private void ShowExitScreen()
        {
            Console.WriteLine("\nThanks for using Hector's Contact Book!");
        }

        private void PressEnterToContinue()
        {
            Console.WriteLine("\nPress Enter...");
            Console.ReadLine();
        }
    }
}