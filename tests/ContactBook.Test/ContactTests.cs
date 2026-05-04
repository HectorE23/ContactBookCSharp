using Xunit;
using ContactBook;

namespace ContactBook.Tests
{
    public class ContactTests
    {
        [Fact]
        public void DefaultConstructor_ShouldInitializeEmpty()
        {
            var contact = new Contact();

            Assert.Equal("", contact.GetFName());
            Assert.Equal("", contact.GetLName());
            Assert.Equal("", contact.GetPhone());
            Assert.Equal("", contact.GetEmail());
        }

        [Fact]
        public void Constructor_WithValues_ShouldAssignCorrectly()
        {
            var contact = new Contact("Hector", "Santos", "7875551234", "hector@email.com");

            Assert.Equal("Hector", contact.GetFName());
            Assert.Equal("Santos", contact.GetLName());
            Assert.Equal("7875551234", contact.GetPhone());
            Assert.Equal("hector@email.com", contact.GetEmail());
        }

        [Fact]
        public void Setters_ShouldUpdateValues()
        {
            var contact = new Contact();

            contact.SetFName("Hector");
            contact.SetLName("Santos");
            contact.SetPhone("7875551234");
            contact.SetEmail("hector@email.com");

            Assert.Equal("Hector", contact.GetFName());
            Assert.Equal("Santos", contact.GetLName());
            Assert.Equal("7875551234", contact.GetPhone());
            Assert.Equal("hector@email.com", contact.GetEmail());
        }

        [Fact]
        public void Equals_SameObject_ShouldBeTrue()
        {
            var contact = new Contact("Hector", "Santos");

            Assert.True(contact.Equals(contact));
        }

        [Fact]
        public void Equals_SameValues_ShouldBeTrue()
        {
            var c1 = new Contact("Hector", "Santos");
            var c2 = new Contact("Hector", "Santos");

            Assert.True(c1.Equals(c2));
            Assert.True(c1 == c2);
        }

        [Fact]
        public void Equals_DifferentValues_ShouldBeFalse()
        {
            var c1 = new Contact("Hector", "Santos");
            var c2 = new Contact("Juan", "Perez");

            Assert.False(c1.Equals(c2));
            Assert.True(c1 != c2);
        }

        [Fact]
        public void Equals_Null_ShouldBeFalse()
        {
            var contact = new Contact("Hector", "Santos");

            Assert.False(contact.Equals(null));
        }

        [Fact]
        public void GetHashCode_SameValues_ShouldBeEqual()
        {
            var c1 = new Contact("Hector", "Santos");
            var c2 = new Contact("Hector", "Santos");

            Assert.Equal(c1.GetHashCode(), c2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_DifferentValues_ShouldNotBeEqual()
        {
            var c1 = new Contact("Hector", "Santos");
            var c2 = new Contact("Juan", "Perez");

            Assert.NotEqual(c1.GetHashCode(), c2.GetHashCode());
        }

        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            var contact = new Contact("Hector", "Santos", "7875551234", "hector@email.com");

            var result = contact.ToString();

            Assert.Contains("Hector", result);
            Assert.Contains("Santos", result);
            Assert.Contains("7875551234", result);
            Assert.Contains("hector@email.com", result);
        }
    }
}