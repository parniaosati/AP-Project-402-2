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

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/your-username/restaurant-management-system.git
   cd restaurant-management-system
