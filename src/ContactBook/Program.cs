using System;
using ContactBook;

public class Program
{
    public static void Main(string[] args)
    {
        Contact c1 = new Contact();

        Contact c2 = new Contact("Hector");

        Contact c3 = new Contact("Hector", "Santos");

        Contact c4 = new Contact(fname: "Hector", email: "hector@email.com");

        Contact c5 = new Contact(lname: "Santos", fname: "Hector");

        Console.WriteLine(c4.ToString());
    }
}