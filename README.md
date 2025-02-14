# Restaurant Management and Food Ordering/Reservation System

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
  - [Customer Features](#customer-features)
  - [Restaurant Features](#restaurant-features)
  - [Admin Features](#admin-features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Usage](#usage)
  - [Customer Panel](#customer-panel)
  - [Restaurant Panel](#restaurant-panel)
  - [Admin Panel](#admin-panel)
- [Validation Rules](#validation-rules)
- [Project Structure](#project-structure)
- [Contributors](#contributors)
- [License](#license)

---

## Introduction

This project is a **Restaurant Management and Food Ordering/Reservation System**, developed as the final project for the **Advanced Programming** course. The system is designed to streamline the process of food ordering and table reservations for customers, while providing restaurant managers with tools to manage their operations efficiently.

The system supports three types of users:
1. **Customers**: Can order food, reserve tables, and provide feedback.
2. **Restaurants**: Can manage their menus, reservations, and orders.
3. **Admins**: Oversee the system, manage restaurants, and respond to complaints.

The project is implemented in **C#** using **WPF** for the user interface and **SQL Server** for database management. **Git** is used for version control.

---

## Features

### Customer Features
- **User Registration and Login**:
  - Customers can register with unique email, username, and phone numbers.
  - Email verification is required during registration.
- **Profile Management**:
  - Customers can view and edit their profiles.
- **Search and Filter Restaurants**:
  - Search restaurants by name, city, or type (e.g., dine-in or delivery).
  - Filter restaurants based on ratings and other criteria.
- **Food Ordering**:
  - Add items to a cart and place orders.
  - Online payment simulation via email confirmation.
- **Table Reservations**:
  - Reserve tables with options for different service tiers (e.g., Bronze, Silver, Gold).
  - Penalties for late arrivals or cancellations.
- **Feedback and Complaints**:
  - Rate and comment on food items.
  - Submit complaints against restaurants.
- **Order History**:
  - Track past orders and reservations.

### Restaurant Features
- **Menu Management**:
  - Add, edit, or delete food items.
  - Update food availability.
- **Reservation and Order Management**:
  - View and filter reservations/orders by date, customer, or other criteria.
  - Generate reports in CSV format.
- **Service Activation**:
  - Activate or deactivate reservation services based on customer ratings.
- **Feedback Management**:
  - Respond to customer comments.

### Admin Features
- **Restaurant Management**:
  - Add and manage restaurants in the system.
  - Set initial usernames and passwords for restaurant accounts.
- **Complaint Management**:
  - View and respond to customer complaints.
  - Update complaint statuses (e.g., under review, resolved).
- **Search and Filter**:
  - Search restaurants by name, city, or rating.
  - Search complaints by user, restaurant, or status.

---

## Technologies Used
- **C# and WPF**: For building the user interface.
- **SQL Server**: For database management.
- **Git**: For version control and collaboration.

---

## Installation

**Clone the Repository**:
   ```bash
   git clone https://github.com/your-username/restaurant-management-system.git
   cd restaurant-management-system
   
##Set Up the Database:

1. **Install SQL Server**  
   - Install **SQL Server** on your system.  
   - Restore the provided `.sql` file to set up the database. This file contains all the necessary tables and relationships for the project.  

2. **Configure the Connection String**  
   - Ensure the connection string in the project matches your SQL Server configuration.  

---

# Install Dependencies

1. **Open the Project**  
   - Open the project in **Visual Studio**.  

2. **Restore NuGet Packages**  
   - Restore NuGet packages to ensure all required dependencies are installed.  

---

# Run the Application

1. **Build and Run the Project**  
   - Build the project in **Visual Studio**.  
   - Run the application.  

2. **Login as a User**  
   - Once the application launches, you can log in as a **customer**, **restaurant**, or **admin** depending on the credentials provided.


## Usage

### For Customers:
- **Login or Register**: Customers must log in or register to access the system.
- **Order Food**: Browse restaurant menus, add items to the cart, and place orders.
- **Reserve Tables**: Choose a reservation time and service tier.
- **Provide Feedback**: Rate and comment on food items.
- **Track Orders**: View order history and reservation details.
- **Submit Complaints**: Customers can file complaints against restaurants and track their status.

### For Restaurants:
- **Manage Menu**: Add, edit, or delete food items and update availability.
- **View Reservations/Orders**: Filter and manage incoming orders and reservations.
- **Generate Reports**: Export reservation and order data as CSV files.
- **Activate/Deactivate Services**: Enable or disable table reservations based on ratings.

### For Admins:
- **Add Restaurants**: Register new restaurants and set initial credentials.
- **Manage Complaints**: View, filter, and respond to customer complaints.
- **Search and Filter**: Search for restaurants or complaints using various criteria.

---

## Technologies Used

The project uses the following technologies:

- **Programming Language**: C#  
- **Frontend Framework**: WPF (Windows Presentation Foundation)  
- **Database**: SQL Server  
- **Version Control**: Git  
- **Other Tools**: LINQ for data filtering and search functionality  

---

## Project Structure

The project is organized into the following main components:

- **Frontend**: Built using WPF and XAML for a clean and responsive user interface.
- **Backend**: Handles business logic and database interactions.
- **Database**: SQL Server is used to store user, restaurant, menu, order, and complaint data.

Directory structure:
- `/Models`: Contains data models for users, restaurants, orders, etc.
- `/Views`: WPF XAML files for the user interface.
- `/ViewModels`: Logic for binding data to views.
- `/Database`: SQL scripts for database setup.
- `/Services`: Business logic and backend services.

---

## Contributing

We welcome contributions to improve this project! To contribute:

1. **Fork the Repository**: Create a copy of the repository under your GitHub account.
2. **Create a Branch**: Create a new branch for your feature or bug fix.
3. **Make Changes**: Commit your changes to your branch.
4. **Submit a Pull Request**: Open a pull request to the main repository, and we will review it promptly.

Please ensure your code is clean and well-documented. Contributions that improve functionality, fix bugs, or enhance the user experience are highly appreciated.

---

## License

This project is licensed under the **MIT License**. See the `LICENSE` file for more details.

---
