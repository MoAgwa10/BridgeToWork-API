# Bridge to Work (B2W) 🧩

An **Inclusive Social Media and Job Platform** designed specifically to empower **people with disabilities** and connect them with job opportunities, companies, and a supportive community.

---

## 🚀 Features

- 🧑‍💼 Job Posting & Application System  
- 🧑‍🤝‍🧑 Social Networking Features  
- 📄 User Profiles (Job Seekers, Companies, Supporters)  
- 🛠️ Accessibility-Focused Design  
- 🎯 Admin Panel for Moderation  
- 🔐 Secure Authentication 

---

## 📁 Project Structure

```bash
/B2W
├── B2W.sln                  # Solution file
├── B2W/                     # Main ASP.NET Core Project
│   ├── Controllers/
│   ├── Models/
│   ├── Views/
│   ├── wwwroot/
│   ├── appsettings.json
│   └── Program.cs


▶️ How to Run the Project Locally
Prerequisites:
.NET 8 SDK

SQL Server

Visual Studio 2022+ or VS Code

Steps:
Clone the repository:


git clone https://github.com/your-username/B2W.git
cd B2W
Set the database connection string:

Go to appsettings.json and update "DefaultConnection" with your SQL Server details.

Run the migrations:

dotnet ef database update
Run the app:

dotnet run
