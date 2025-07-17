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


            public void ViewAllRooms() // Method to view all rooms
            {
                Console.WriteLine("All Rooms:");
                if (!rooms.Any())
                {
                    Console.WriteLine("No rooms available.");
                    return;
                }

                foreach (var room in rooms)
                    Console.WriteLine(room.ToString());
            }

            public void ReserveRoom() // Method to reserve a room
            {
                Console.Write("Enter guest name: ");
                string guestName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(guestName)) // Validate guest name
                {
                    Console.WriteLine("Guest name cannot be empty.");
                    return;
                }

                Console.Write("Enter Room Number to Reserve: ");
                int number = int.Parse(Console.ReadLine());

                var room = rooms.FirstOrDefault(r => r.RoomNumber == number);
                if (room == null) // Check if room exists
                {
                    Console.WriteLine("Room does not exist.");
                    return;
                }
                if (room.IsReserved) // Check if room is already reserved
               
                {
                    Console.WriteLine("Room already reserved.");
                    return;
                }

                Console.Write("Enter number of nights: ");
                int nights;
                if (!int.TryParse(Console.ReadLine(), out nights) || nights <= 0) // Validate number of nights
                {
                    Console.WriteLine("Invalid number of nights.");
                    return;
                }

                room.IsReserved = true;
                room.Reservation = new Reservation(guestName, number, nights, DateTime.Now);
             
                reservations.Add(room.Reservation); // Add reservation to the list
                Console.WriteLine("Room reserved successfully.");

            }

            public void ViewAllReservations() // Method to view all reservations
            {
                Console.WriteLine("All Reservations:");
                if (!reservations.Any())
                {
                    Console.WriteLine("No reservations found.");
                    return;
                }

                foreach (var reservation in reservations) 
                {
                    var room = GetRoomByNumber(reservation.RoomNumber);
                    if (room != null)
                        Console.WriteLine(reservation.Display(room));
                }
            }

            public void SearchReservation() // Method to search reservation by guest name
            {
                Console.Write("Enter guest name to search: ");
                string guestName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(guestName)) // Validate guest name
                {
                    Console.WriteLine("Guest name cannot be empty.");
                    return;
                }
                var foundReservations = reservations.Where(r => r.GuestName.Equals(guestName, StringComparison.OrdinalIgnoreCase)).ToList();
                if (!foundReservations.Any()) // Check if any reservations found
                {
                    Console.WriteLine("No reservations found for this guest.");
                    return;
                }

                Console.WriteLine("Reservations found:"); // Display found reservations
                foreach (var reservation in foundReservations)
                {
                    var room = GetRoomByNumber(reservation.RoomNumber);
                    if (room != null)
                        Console.WriteLine(reservation.Display(room));
                }

            }

            public void HighestPayingGuest() // Method to find the highest paying guest
            {
                if (!reservations.Any()) // Check if there are any reservations
                {
                    Console.WriteLine("No reservations found.");
                    return;
                }
                var highestPaying = reservations.OrderByDescending(r => r.TotalCost()).First(); // Get the reservation with the highest total cost
                var room = GetRoomByNumber(highestPaying.RoomNumber);

                if (room != null)
                    Console.WriteLine($"Highest Paying Guest: {highestPaying.GuestName} | Room: {room.RoomNumber} | Total Cost: {highestPaying.TotalCost()}");
            }

            public void CancelReservation() // Method to cancel a reservation
            {
                Console.Write("Enter guest name to cancel reservation: ");
                string guestName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(guestName)) // Validate guest name
                {
                    Console.WriteLine("Guest name cannot be empty.");
                    return;
                }

                var reservation = reservations.FirstOrDefault(r => r.GuestName.Equals(guestName, StringComparison.OrdinalIgnoreCase));
                if (reservation == null) // Check if reservation exists
                {
                    Console.WriteLine("No reservation found for this guest.");
                    return;
                }

                var room = GetRoomByNumber(reservation.RoomNumber); 
                if (room != null)
                {
                    room.IsReserved = false; // Mark room as available
                    room.Reservation = null; // Clear reservation details

                    reservations.Remove(reservation); // Remove reservation from the list
                    Console.WriteLine("Reservation cancelled successfully.");
                }


            }
    } }
