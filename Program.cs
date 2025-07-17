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

        private void ViewRooms()
        {
            if (rooms.Count == 0)
            {
                Console.WriteLine("No rooms found.");
                return;
            }

            foreach (Room room in rooms)
            {
                Console.WriteLine(room.GetDetails());
            }
        }

        private void ReserveRoom()
        {
            Console.Write("Enter guest name: ");
            string guestName = Console.ReadLine();

            Console.Write("Enter room number to reserve: ");
            if (!int.TryParse(Console.ReadLine(), out int number))
            {
                Console.WriteLine("Invalid room number.");
                return;
            }

            Room room = rooms.Find(r => r.RoomNumber == number);
            if (room == null)
            {
                Console.WriteLine("Room not found.");
                return;
            }

            if (room.IsReserved)
            {
                Console.WriteLine("Room is already reserved.");
                return;
            }

            Console.Write("Enter number of nights: ");
            if (!int.TryParse(Console.ReadLine(), out int nights) || nights <= 0)
            {
                Console.WriteLine("Invalid number of nights.");
                return;
            }

            room.Reservation = new Reservation(guestName, number, nights, DateTime.Now, room.DailyRate);
            room.IsReserved = true;
            Console.WriteLine("Room reserved successfully.");
        }

        private void ViewReservations()
        {
            foreach (Room room in rooms)
            {
                if (room.IsReserved && room.Reservation != null)
                {
                    Console.WriteLine(room.Reservation.Display());
                }
            }
        }

        private void SearchReservation()
        {
            Console.Write("Enter guest name to search: ");
            string guestName = Console.ReadLine();

            foreach (Room room in rooms)
            {
                if (room.IsReserved && room.Reservation != null &&
                    room.Reservation.GuestName.Equals(guestName, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(room.Reservation.Display());
                    return;
                }
            }

            Console.WriteLine("Reservation not found.");
        }

        private void HighestPayingGuest()
        {
            double maxPayment = 0;
            Reservation highest = null;

            foreach (Room room in rooms)
            {
                if (room.IsReserved && room.Reservation != null)
                {
                    double cost = room.Reservation.TotalCost();
                    if (cost > maxPayment)
                    {
                        maxPayment = cost;
                        highest = room.Reservation;
                    }
                }
            }

            if (highest != null)
            {
                Console.WriteLine("Highest paying guest:");
                Console.WriteLine(highest.Display());
            }
            else
            {
                Console.WriteLine("No reservations found.");
            }
        }
        private void CancelReservation()
        {
            Console.Write("Enter room number to cancel reservation: ");
            if (!int.TryParse(Console.ReadLine(), out int number))
            {
                Console.WriteLine("Invalid number.");
                return;
            }

            Room room = rooms.Find(r => r.RoomNumber == number);
            if (room != null && room.IsReserved)
            {
                room.IsReserved = false;
                room.Reservation = null;
                Console.WriteLine("Reservation cancelled.");
            }
            else
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

        public Room(int roomNumber, double dailyRate)
        {
            RoomNumber = roomNumber;
            DailyRate = dailyRate;
            IsReserved = false;
            Reservation = null;
        }




    }
    

