using System;

namespace ContactBook
{
    public class Contact : IEquatable<Contact>
    {
        private string fname = default!;
        private string lname = default!;
        private string phone = default!;
        private string email = default!;

        public Contact(
            string fname = "", 
            string lname = "", 
            string phone = "", 
            string email = "")
        {
            SetFName(fname);
            SetLName(lname);
            SetPhone(phone);
            SetEmail(email);
        }

        public string GetFName()
        {
            return fname;
        }

        public string GetLName()
        {
            return lname;
        }

        public string GetPhone()
        {
            return phone;
        }

        public string GetEmail()
        {
            return email;
        }

        public void SetFName(string fname)
        {
            this.fname = fname;
        }

        public void SetLName(string lname)
        {
            this.lname = lname;
        }

        public void SetPhone(string phone)
        {
            this.phone = phone;
        }

        public void SetEmail(string email)
        {
            this.email = email;
        }

        public override string ToString()
        {
            return $"{fname} {lname} {phone} {email}";
        }

        public bool Equals(Contact? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(fname, other.fname)
                && string.Equals(lname, other.lname)
                && string.Equals(phone, other.phone)
                && string.Equals(email, other.email);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Contact);
        }

        public static bool operator ==(Contact? x, Contact? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null) return false;
            return x.Equals(y);
        }

        public static bool operator !=(Contact x, Contact y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(fname, lname, phone, email);
        }
    }
}