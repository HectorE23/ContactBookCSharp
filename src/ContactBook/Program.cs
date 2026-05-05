using ContactBook;

class Program
{
    static void Main()
    {
        var app = new ContactBookApp(ContactSeed.GetContacts());
        app.Start();
    }
}