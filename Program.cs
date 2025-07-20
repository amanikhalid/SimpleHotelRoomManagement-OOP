using System;
using System.Collections.Generic;

namespace SimpleHotelRoomManagement_OOP
{
    class Program
    {
        static void Main(string[] args)
        {
            
            HotelSystem system = new HotelSystem(); // Create an instance of the HotelSystem class
            system.LoadData(); // Load data before running the system
            system.Run();

            

        }
    }

    class HotelSystem
    {
        private const string FILE_PATH = "C:\\Users\\CodeLine\\source\\repos\\SimpleHotelRoomManagement-OOP\\hotel_data.txt"; 

        private List<Room> rooms = new List<Room>(); // List to store rooms

        public void Run() // Main method to run the hotel management system
        {
            while (true)
            {
                Console.Clear(); // Clear the console screen

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
                    case "8":
                        SaveData(); // Save data before exit
                        Console.WriteLine("Goodbye!");
                        return;

                 
                }
            }
        }



        public void LoadData() // Method to load room data from a file
        {
            if (!File.Exists(FILE_PATH))
            {
                Console.WriteLine("No saved data found.");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(FILE_PATH);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');

                    int roomNumber = int.Parse(parts[0]);
                    double dailyRate = double.Parse(parts[1]);
                    bool isReserved = bool.Parse(parts[2]);

                    Room room = new Room(roomNumber, dailyRate);

                    if (isReserved && parts.Length >= 7)
                    {
                        string guestName = parts[3];
                        int nights = int.Parse(parts[4]);
                        DateTime checkIn = DateTime.Parse(parts[5]);
                        DateTime checkOut = DateTime.Parse(parts[6]);

                        room.IsReserved = true;
                        room.Reservation = new Reservation(guestName, roomNumber, nights, checkIn, dailyRate);
                    }

                    rooms.Add(room);
                }

                Console.WriteLine("Data loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading data: " + ex.Message);
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

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
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

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void ReserveRoom() // Method to reserve a room
        {
            Console.Write("Enter guest name: "); // Prompt for guest name
            string guestName = Console.ReadLine();

            // Guest name validation
            if (string.IsNullOrWhiteSpace(guestName))
            {
                Console.WriteLine("Guest name cannot be empty.");
                return;
            }

            if (guestName.Length < 3 || guestName.Length > 50) // Validate length of guest name
            {
                Console.WriteLine("Guest name must be between 3 and 50 characters.");
                return;
            }
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

            Console.Write("Enter check-in date (yyyy-MM-dd): "); // Prompt for check-in date
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime checkIn))
            {
                Console.WriteLine("Invalid check-in date.");
                return;
            }
            if (checkIn < DateTime.Now.Date) // Check if check-in date is in the past
            {
                Console.WriteLine("Check-in date cannot be in the past.");
                return;
            }
            if (nights < 1 || nights > 30) // Check if the number of nights is valid
            {
                Console.WriteLine("Number of nights must be between 1 and 30.");
                return;
            }
            

            Console.Write("Enter check-out date (yyyy-MM-dd): "); // Prompt for check-out date
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime checkOut) || checkOut <= checkIn)
            {
                Console.WriteLine("Invalid check-out date.");
                return;
            }
            if ((checkOut - checkIn).Days < nights) // Check if the number of nights matches the check-in and check-out dates
            {
                Console.WriteLine("Check-out date must be at least " + nights + " nights after check-in date.");
                return;
            }
            if ((checkOut - checkIn).Days > 30) // Check if the reservation exceeds 30 days
            {
                Console.WriteLine("Reservation cannot exceed 30 days.");
                return;
            }


            int night = (checkOut - checkIn).Days; 

            room.Reservation = new Reservation(guestName, number, nights, checkIn, room.DailyRate);
            room.IsReserved = true;
            Console.WriteLine("Room reserved successfully.");

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
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

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
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
            Console.Write("Enter room number to cancel reservation: ");
            if (!int.TryParse(Console.ReadLine(), out int roomNumber)) // Validate room number input
            {
                Console.WriteLine("Invalid room number.");
                return;
            }

            var room = rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);

            if (room == null) // Check if the room exists
            {
                Console.WriteLine("Room does not exist.");
                return;
            }

            if (!room.IsReserved) // Check if the room is reserved
            {
                Console.WriteLine("Room is not reserved.");
                return;
            }

            room.IsReserved = false;
            room.Reservation = null;
            Console.WriteLine("Reservation cancelled.");
        }

        public void SaveData() // Method to save room data to a file
        {
            using (StreamWriter writer = new StreamWriter(FILE_PATH))
            {
                foreach (Room room in rooms)
                {
                    string line = $"{room.RoomNumber},{room.DailyRate},{room.IsReserved}";
                    if (room.IsReserved && room.Reservation != null)
                    {
                        line += $",{room.Reservation.GuestName},{room.Reservation.Nights},{room.Reservation.CheckInDate},{room.Reservation.CheckOutDate}";
                    }

                    writer.WriteLine(line);
                }
            }

            Console.WriteLine("Data saved successfully.");
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
    

