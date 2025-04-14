# 📚 E-Learning Platform Backend

The backend for our E-learning platform, developed in **.NET C#**, provides a RESTful API to support student learning activities, authentication, and content management. It connects to a **PostgreSQL** database hosted on **Azure**, and works in tandem with a **React Vite** frontend.

---

## 🔧 Technology Stack

- **Language**: C# (.NET 7/8)
- **Framework**: ASP.NET Core Web API
- **Database**: PostgreSQL (Azure-hosted)
- **ORM**: Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **Frontend**: React with Vite + TypeScript
- **Deployment**: Azure App Services

---

## 🗂 Project Structure

- **Controllers**: API endpoints for each feature
- **Models**: Entity definitions
- **DTOs**: Data Transfer Objects for communication
- **Data**: Database context and seeding logic
- **Repositories**: Data access logic
- **Services**: Core business logic
- **Middleware**: Error handling and request interception
- **Auth**: JWT generation and validation
- **Program.cs**: Application entry point

---

🔐 Authentication & Roles
JWT-Based Authentication: Secure endpoints with bearer tokens

User Roles:

Admin: Manages users and course assignments
Teacher: Creates modules, adds activities, grades submissions
Student: Views assigned courses and submits activities

---

## ⚙️ Setup Instructions

### Clone the Repository
```Bash
git clone https://github.com/payamaya/APL-Backend.git
cd APL-Backend
```
---

## 🤝 Contributors

- **Payamaya** – Backend logic, FrontEnd logic,  DB schema
- **mhdKhavari** – Authentication, security, deployment
- **ammarovehub** – API integration, admin tools, GitHub CI/CD

## 📝 License
- This project is licensed under the MIT License.

## 📫 Contact
- Have questions or suggestions? Email us at support@elearn-platform.com
