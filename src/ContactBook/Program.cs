using System;

public class Program
{
    public static void Main()
    {
        var app = new ContactBook.ContactBook(ContactBook.ContactSeeds.Contacts);
        app.Start();
    }
}