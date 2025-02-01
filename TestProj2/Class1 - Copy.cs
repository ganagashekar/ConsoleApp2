using System.IO;

namespace TestProj2  // Example namespace
{
    public class Address
    {
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public string City { get; set; }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }

        public void UpdateAddress(Address address)
        {
            this.Address = address;
        }
    }

    public class MyService
    {
        public void ProcessPerson(Person person)
        {
            // ... use the person object
        }

        public void UpdatePersonAddress(Person person)
        {
            person.UpdateAddress(new Address { Street = "New Street" });
        }

        public int[] GetEvenNumbers(int limit)
        {
            return Enumerable.Range(1, limit).Where(x => x % 2 == 0).ToArray();
        }
    }
}