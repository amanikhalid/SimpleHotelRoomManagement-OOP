using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Security.Principal;
using System.Transactions;

namespace SimpleHotelRoomManagement_OOP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HotelSystem hotelSystem = new HotelSystem();
            int choice;

            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Hotel Room Management System");
                Console.WriteLine("1. Add New Room");
                Console.WriteLine("2. View All Rooms");
                Console.WriteLine("3. Reserve a Room");
                Console.WriteLine("4. View All Reservations");
                Console.WriteLine("5. Search Reservation by Guest Name");
                Console.WriteLine("6. Find Highest Paying Guest");
                Console.WriteLine("7. Cancel Reservation");
                Console.WriteLine("0. Exit");

                Console.Write("Please enter your choice: ");

            
        }
    }
}
