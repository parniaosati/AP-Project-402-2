# 🍽️ Restaurant Management System

A comprehensive desktop application developed using WPF (Windows Presentation Foundation) and C#, designed to streamline restaurant operations, including food ordering, reservations, and user management. The system caters to three types of users: Customers, Restaurant Managers, and Administrators.

## 📋 Table of Contents

- [Project Overview](#project-overview)
- [Objectives](#objectives)
- [Technologies Used](#technologies-used)
- [System Features](#system-features)
  - [Customer Panel](#customer-panel)
  - [Restaurant Panel](#restaurant-panel)
  - [Admin Panel](#admin-panel)
- [Special Services](#special-services)
- [Validation Rules](#validation-rules)
- [Git Workflow](#git-workflow)
- [Setup Instructions](#setup-instructions)
- [Team Members](#team-members)
- [Contact Information](#contact-information)

---

## 📖 Project Overview

This project aims to develop a robust and user-friendly restaurant management system that facilitates:

- Efficient food ordering and reservation processes.
- Comprehensive user management for customers, restaurants, and administrators.
- Real-time tracking and management of orders and reservations.
- Enhanced customer experience through special services and support features.

---

## 🎯 Objectives

- Implement advanced programming concepts using C# and WPF.
- Utilize SQL Server for persistent data storage.
- Employ Git for version control and collaborative development.
- Ensure high code readability, proper exception handling, and modular design.
- Provide a seamless user interface with intuitive navigation and responsiveness.

---

## 🛠️ Technologies Used

- **Programming Language:** C#
- **Framework:** WPF (Windows Presentation Foundation)
- **Database:** SQL Server
- **Version Control:** Git
- **Design Pattern:** MVVM (Model-View-ViewModel)

---

## 🔧 System Features

### 👤 Customer Panel

- **Authentication:**
  - Login and registration with unique username and mobile number.
  - Email verification during registration.

- **Profile Management:**
  - View and edit personal information.
  - Upgrade to special services (Bronze, Silver, Gold).

- **Restaurant Search:**
  - Search restaurants by city, name, or type (Delivery/Dine-in).
  - Apply filters based on average ratings.

- **Menu Interaction:**
  - View restaurant menus categorized by food type.
  - Place orders and make reservations.
  - Rate and comment on food items.

- **Order & Reservation History:**
  - View past orders and reservations.
  - Submit complaints regarding services.

- **Live Chat Support:**
  - Real-time chat with administrators for support (session-based, non-persistent).

### 🏢 Restaurant Panel

- **Menu Management:**
  - Add, edit, or remove food items.
  - Categorize menu items appropriately.

- **Inventory Management:**
  - Update stock quantities.
  - Mark items as unavailable when out of stock.

- **Reservation Service:**
  - Enable or disable reservation services based on average ratings.

- **Order & Reservation Tracking:**
  - View history with filtering options (e.g., by customer, date, price range).
  - Export data to CSV format.

- **Complaint Handling:**
  - Respond to customer complaints.
  - View complaint history.

### 🛡️ Admin Panel

- **Restaurant Registration:**
  - Add new restaurants with initial credentials.

- **User & Restaurant Management:**
  - Search and filter users and restaurants based on various criteria.

- **Complaint Management:**
  - View, respond to, and update the status of complaints.

- **Monitoring:**
  - Track overall system performance and user activities.

---

## 🌟 Special Services

Customers can subscribe to special services to access enhanced features:

- **Bronze Service:**
  - Price: 100,000 IRR
  - Max Reservations: 2 per month
  - Reservation Duration: 1 hour
  - Cancellation Penalty Threshold: 30 minutes

- **Silver Service:**
  - Price: 150,000 IRR
  - Max Reservations: 5 per month
  - Reservation Duration: 1.5 hours
  - Cancellation Penalty Threshold: 30 minutes

- **Gold Service:**
  - Price: 300,000 IRR
  - Max Reservations: 15 per month
  - Reservation Duration: 3 hours
  - Cancellation Penalty Threshold: 15 minutes

*Note: Only restaurants with an average rating of 4.5 or higher can offer reservation services.*

---

## ✅ Validation Rules

- **Name & Surname:**
  - Only letters, minimum 3 and maximum 32 characters.

- **Email:**
  - Format: `example@domain.com`
  - Domain and extension must be valid.

- **Mobile Number:**
  - Exactly 11 digits, starting with '09'.

- **Username:**
  - Minimum 3 characters, can include letters and numbers.

- **Password:**
  - Minimum 8 characters, maximum 32.
  - Must include at least one uppercase letter, one lowercase letter, and one digit.

- **Initial Restaurant Password:**
  - 8-digit random number.

---
## 🖥️ Setup Instructions

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/parniaosati/AP-Project-402-2.git
   ```
   ## 🗄️ Database Setup

### 1. Install Requirements

- Install **SQL Server** and **SQL Server Management Studio (SSMS)**.

### 2. Execute SQL Scripts

- Open **SSMS**.
- Use the SQL scripts located in the `/Database` directory to:
  - ✅ Create all necessary tables.
  - ✅ Seed initial data (e.g., admin accounts, sample restaurants).
- Make sure your **SQL Server instance is running** properly.

---

## ⚙️ Configure the Application

### 3. Update Connection String

- Open the solution in **Visual Studio**.
- Navigate to `App.config` (or your data access configuration file).
- Replace the default connection string with the one that matches your local SQL Server instance. Example:

```xml
<connectionStrings>
  <add name="RestaurantDbContext"
       connectionString="Data Source=localhost;Initial Catalog=RestaurantDb;Integrated Security=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```
## 🧱 Build and Run

### 4. Build the Application

- Open the solution in **Visual Studio**.
- Restore any missing packages by navigating to: Tools > NuGet Package Manager > Restore NuGet Packages
- Build the solution to ensure everything compiles correctly.

### 5. Run the Application

- Press `F5` or click the **Start** button in Visual Studio to launch the application.

---

## ✅ Test Your Setup

Once the application launches:

- You will see a **Login screen** as the first interface.
- Use one of the **pre-seeded admin accounts** or register as a **new customer**.
- After login, the system will direct you to the appropriate dashboard based on your role:
- **Customer:** Browse restaurants, order food, reserve tables, and more.
- **Restaurant Owner:** Manage menu items, view orders, track reservations, etc.
- **Administrator:** Add restaurants, respond to complaints, and manage users.

Explore each feature to ensure full functionality and understand the system flow.
