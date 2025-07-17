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
            int choice; // Variable to store user choice for menu options

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

                int.TryParse(Console.ReadLine(), out choice);

                switch (choice) // Switch case to handle user choicesl
                {
                    case 1: hotel.AddRoom(); break;
                    case 2: hotel.ViewAllRooms(); break;
                    case 3: hotel.ReserveRoom(); break;
                    case 4: hotel.ViewAllReservations(); break;
                    case 5: hotel.SearchReservation(); break;
                    case 6: hotel.HighestPayingGuest(); break;
                    case 7: hotel.CancelReservation(); break;
                    case 0: Console.WriteLine("Exiting..."); break;
                    default: Console.WriteLine("Invalid choice."); break;
                }


            } while
            (choice != 0);
        }

        class Room
        {
            public int RoomNumber { get; set; } // Unique identifier for the room
            public double DailyRate { get; set; } 
            public bool IsReserved { get; set; } = false; // Indicates if the room is reserved
            public Reservation Reservation { get; set; } = null; // Reservation details if the room is reserved

            public Room(int roomNumber, double dailyRate) // Constructor to initialize room properties
            {
                RoomNumber = roomNumber;
                DailyRate = dailyRate;
            }

            public override string ToString() 
            {
                if (!IsReserved)
                    return $"Room {RoomNumber} | Rate: {DailyRate} | Available";
                else
                    return $"Room {RoomNumber} | Reserved by {Reservation.GuestName} | Total: {Reservation.TotalCost()}";
            }
        }
    }
}
