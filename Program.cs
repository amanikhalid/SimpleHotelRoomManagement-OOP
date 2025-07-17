using System;
using System.Collections.Generic;

namespace SimpleHotelRoomManagement_OOP
{
    class Program
    {
        static void Main(string[] args)
        {
            HotelSystem system = new HotelSystem(); // Create an instance of the HotelSystem class
            system.Run();
        }
    }

    class HotelSystem
    {
        private List<Room> rooms = new List<Room>(); // List to store rooms

        public void Run() // Main method to run the hotel management system
        {
            while (true)
            {
                Console.WriteLine("\nHotel Room Management System");
                Console.WriteLine("1. Add Room");
                Console.WriteLine("2. View All Rooms");
                Console.WriteLine("3. Reserve Room");
                Console.WriteLine("4. View Reservations");
                Console.WriteLine("5. Search Reservation by Guest Name");
                Console.WriteLine("6. Highest-Paying Guest");
                Console.WriteLine("7. Cancel Reservation");
                Console.WriteLine("8. Exit");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1": AddRoom(); break;
                    case "2": ViewRooms(); break;
                    case "3": ReserveRoom(); break;
                    case "4": ViewReservations(); break;
                    case "5": SearchReservation(); break;
                    case "6": HighestPayingGuest(); break;
                    case "7": CancelReservation(); break;
                    case "8": return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        private void AddRoom() // Method to add a new room
        {
            Console.Write("Enter room number: "); // Prompt for room number
            if (!int.TryParse(Console.ReadLine(), out int number))
            {
                Console.WriteLine("Invalid number.");
                return;
            }

            if (rooms.Exists(r => r.RoomNumber == number)) // Check if room already exists
            {
                Console.WriteLine("Room already exists.");
                return;
            }

            Console.Write("Enter daily rate: "); // Prompt for daily rate
            if (!double.TryParse(Console.ReadLine(), out double rate) || rate < 100)
            {
                Console.WriteLine("Invalid rate. Must be >= 100.");
                return;
            }

            rooms.Add(new Room(number, rate)); // Add the new room to the list
            Console.WriteLine("Room added successfully.");
        }

        private void ViewRooms() // Method to view all rooms
        {
            if (rooms.Count == 0)
            {
                Console.WriteLine("No rooms found."); // If no rooms exist, notify the user
                return;
            }

            foreach (Room room in rooms)
            {
                Console.WriteLine(room.GetDetails()); // Display details of each room
            }
        }

        private void ReserveRoom() // Method to reserve a room
        {
            Console.Write("Enter guest name: "); // Prompt for guest name
            string guestName = Console.ReadLine();

            Console.Write("Enter room number to reserve: "); // Prompt for room number
            if (!int.TryParse(Console.ReadLine(), out int number))
            {
                Console.WriteLine("Invalid room number.");
                return;
            }

            Room room = rooms.Find(r => r.RoomNumber == number); // Find the room by number
            if (room == null)
            {
                Console.WriteLine("Room not found."); // If room does not exist, notify the user
                return;
            }

            if (room.IsReserved)
            {
                Console.WriteLine("Room is already reserved."); // If room is already reserved, notify the user
                return;
            }

            Console.Write("Enter number of nights: "); // Prompt for number of nights
            if (!int.TryParse(Console.ReadLine(), out int nights) || nights <= 0)
            {
                Console.WriteLine("Invalid number of nights.");
                return;
            }

            room.Reservation = new Reservation(guestName, number, nights, DateTime.Now, room.DailyRate); // Create a new reservation
            room.IsReserved = true;
            Console.WriteLine("Room reserved successfully.");
        }

        private void ViewReservations() // Method to view all reservations
        {
            foreach (Room room in rooms)
            {
                if (room.IsReserved && room.Reservation != null) // Check if the room is reserved
                {
                    Console.WriteLine(room.Reservation.Display());
                }
            }
        }

        private void SearchReservation()
        {
            Console.Write("Enter guest name to search: "); // Prompt for guest name
            string guestName = Console.ReadLine();

            foreach (Room room in rooms) // Iterate through all rooms
            {
                if (room.IsReserved && room.Reservation != null &&
                    room.Reservation.GuestName.Equals(guestName, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(room.Reservation.Display()); // Display reservation details if found
                    return;
                }
            }

            Console.WriteLine("Reservation not found.");
        }

        private void HighestPayingGuest() // Method to find the highest paying guest
        {
            double maxPayment = 0;
            Reservation highest = null;

            foreach (Room room in rooms) // Iterate through all rooms
            {
                if (room.IsReserved && room.Reservation != null) // Check if the room is reserved
                {
                    double cost = room.Reservation.TotalCost(); // Calculate total cost of the reservation
                    if (cost > maxPayment)
                    {
                        maxPayment = cost;
                        highest = room.Reservation; // Update highest paying guest if current reservation is higher
                    }
                }
            }

            if (highest != null)
            {
                Console.WriteLine("Highest paying guest:"); // Display the highest paying guest
                Console.WriteLine(highest.Display());
            }
            else
            {
                Console.WriteLine("No reservations found."); // If no reservations exist, notify the user
            }
        }
        private void CancelReservation() // Method to cancel a reservation
        {
            Console.Write("Enter room number to cancel reservation: "); // Prompt for room number
            if (!int.TryParse(Console.ReadLine(), out int number)) 
            {
                Console.WriteLine("Invalid number.");
                return;
            }

            Room room = rooms.Find(r => r.RoomNumber == number); // Find the room by number
            if (room != null && room.IsReserved) // Check if the room exists and is reserved
            {
                room.IsReserved = false;
                room.Reservation = null;
                Console.WriteLine("Reservation cancelled.");
            }
            else // If room does not exist or is not reserved, notify the user
            {
                Console.WriteLine("No reservation found for that room.");
            }
        }

    }

    class Room
    {
        public int RoomNumber { get; set; }
        public double DailyRate { get; set; }
        public bool IsReserved { get; set; }
        public Reservation Reservation { get; set; }

        public Room(int roomNumber, double dailyRate) // Constructor to initialize a room
        {
            RoomNumber = roomNumber;
            DailyRate = dailyRate;
            IsReserved = false;
            Reservation = null;
        }

        public string GetDetails() // Method to get details of the room
        {
            if (IsReserved && Reservation != null) // Check if the room is reserved
            {
                return $"Room {RoomNumber} (Reserved) - Guest: {Reservation.GuestName}, Total: {Reservation.TotalCost()}";
            }
            else // If the room is available
            {
                return $"Room {RoomNumber} (Available) - Rate: {DailyRate}";
            }
        }

    }

    class Reservation
    {
        public string GuestName { get; set; }
        public int RoomNumber { get; set; }
        public int Nights { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public double DailyRate { get; set; }

        public Reservation(string guestName, int roomNumber, int nights, DateTime checkInDate, double dailyRate)
        {
            GuestName = guestName;
            RoomNumber = roomNumber;
            Nights = nights;
            CheckInDate = checkInDate;
            CheckOutDate = checkInDate.AddDays(nights);
            DailyRate = dailyRate;
        }

        public double TotalCost() // Method to calculate total cost of the reservation
        {
            return Nights * DailyRate; // Calculate total cost based on nights and daily rate
        }

        public string Display() // Method to display reservation details
        {
            return $"Reservation for {GuestName} | Room: {RoomNumber} | Nights: {Nights} | Check-in: {CheckInDate.ToShortDateString()} | Check-out: {CheckOutDate.ToShortDateString()} | Total Cost: {TotalCost()}";
        }
    }
}
    

