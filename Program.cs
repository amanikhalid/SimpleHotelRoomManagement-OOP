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
                    default: Console.WriteLine("Invalid choice."); break; // Default case for invalid input
                }


            } while
            (choice != 0);
        }

        class

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
                    return $"Room {RoomNumber} | Rate: {DailyRate} | Available"; // Display room details if not reserved
                else
                    return $"Room {RoomNumber} | Reserved by {Reservation.GuestName} | Total: {Reservation.TotalCost()}"; // Display room details if reserved
            }
        }



        class Reservation
        {
            public string GuestName { get; set; }
            public int RoomNumber { get; set; }
            public int Nights { get; set; } // Number of nights reserved
            public DateTime CheckInDate { get; set; }
            public DateTime CheckOutDate { get; set; }

            public Reservation(string guestName, int roomNumber, int nights, DateTime checkInDate) // Constructor to initialize reservation properties
            {
                GuestName = guestName;
                RoomNumber = roomNumber;
                Nights = nights;
                CheckInDate = checkInDate;
                CheckOutDate = checkInDate.AddDays(nights); // Calculate check-out date based on nights reserved
            }

            public double TotalCost() // Calculate total cost of the reservation
            {
                return Nights * HotelSystem.GetRoomByNumber(RoomNumber).DailyRate; // Get room rate and calculate total cost
            }

            public string Display(Room room) // Display reservation details in a formatted string
            {
                return $"Reservation for {GuestName} | Room: {room.RoomNumber} | Nights: {Nights} | Check-in: {CheckInDate.ToShortDateString()} | Check-out: {CheckOutDate.ToShortDateString()} | Total Cost: {TotalCost()}";
            }

        }

        class HotelSystem
        {
            private List<Room> rooms = new List<Room>(); // List to store rooms
            private List<Reservation> reservations = new List<Reservation>(); // List to store reservations
            private const double MIN_RATE = 100.0;

            public void AddRoom() // Method to add a new room
            {
                Console.Write("Enter room number: ");
                int roomNumber = int.Parse(Console.ReadLine());
                if (rooms.Exists(r => r.RoomNumber == roomNumber)) // Check if room already exists
                {
                    Console.WriteLine("Room number already exists.");
                    return;
                }

                Console.Write("Enter daily rate: ");
                double dailyRate = double.Parse(Console.ReadLine());

                if (dailyRate < MIN_RATE) // Validate minimum rate
                {
                    Console.WriteLine($"Daily rate must be at least {MIN_RATE}.");
                    return;
                }
                rooms.Add(new Room(roomNumber, dailyRate)); // Add new room to the list
                Console.WriteLine("Room added successfully.");
            }

        }
    } }
