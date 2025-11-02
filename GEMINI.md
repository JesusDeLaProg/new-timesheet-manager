# Timesheet Manager

The Timesheet Manager Project is a full-stack Web Application for timesheet management for small and medium sized businesses. The Software is tailored specifically for Engineering firms, centered around clients, projects, project phases and activities.

# Project Structure

## client

The Client part of the Project is an Angular Application. The Angular App is divided into multiple dashboards that allow interacting with different parts of the Database:

- Home page: list Timesheets visible to the current user. For example, regular users can only see their own Timesheets, while administrators can see Timesheets for users below their own user privilege level.
- Timesheet Edit: This dashboard allows creating and editing Timesheets. This page presents a single Timesheet in table format with editable cells, and a table header that allow modifying the Timesheet's header information (e.g. start and end date, owner of the Timesheet, etc.).
- Admin Panel: This dashbord is only accessible to administrators. It contains these sub-dashboards:
  - Project and Client management: Allows creating and editing Clients and Projects.
  - Activity and Phase management: Allows creating and editing activities and phases, and allows linking activities to phases.
  - Employee management: Allows creating and editing Employees.
  - User management: Allows creating and editing users of the Application.

## server

The Server part of the Project is an ASP.Net Core Application. This Application is a classic gPRC Service exposing endpoints for different entity types managed through the frontend application. The Server performs validation on protobufs using a mix of protovalidate and custom validation logic for complex validation.

## proto

Communication between the client and the server is based on gRPC. As such, protos used for communication between both parts of the Application are placed in the `proto` directory.

## Database

All data is stored in a Firestore Native database. This database contains a top-level collection called `timesheet-manager`, which itself contains one document for each environment of the application:

- development
- staging
- production

Each of these documents contain collections, one for each type of entity, listed in a later section of this document.


# Entities

These are the entities modeled by the application:

- Timesheet: Main entity of the application. This entity contains information about the amount of work done on particular projects on different dates by a single employee.
- Client: A Client is a real client for the Business using the Timesheet Manager Software. It is used to group Projects given by the same client to the Business.
- Project: A Project is an Engineering project to which employees work on.
- Phase: A Project is divided into multiple Phases, which itself is divided into multiple Activities.
- Activity: An Activity is a specific task category done by an employee during a Project Phase. An Activity can be tied to different Phases.
- User: A User is the representation of a person's account in the System. It is used to control authentication and authorization into the Application.
- Employee: An Employee is the representation of a person working on a Project. Entities like Timesheets are tied to Employees, not Users. Employee entities are used to keep information about an employee's name and role as an employee, which itself keeps information about billable rate and other information. An Employee can be owned by a User, or it can be unowned, in which case Administrators are responsible to manage.

## Timesheet

### Timesheet Header

The Header contains this information:

- Employee ID: ID of the Employee this Timesheet is attached to.
- Start and End date: Start and end date of the Timesheet.

### Lines

The content of the Timesheet is divided into lines where Employees enter their time worked. Each line has this information:

- Project
- Phase
- Activity
- Misc: Free-form text input for Employees to add information about the task
- Entries: For each day of the Timesheet, Employees can enter a fractional number of hours

### Travels

Work-related Travels can be reimbursed to the Employee. Travels are usually car travel, so they are reimbursed based on distance traveled. Each travel contains this information:

- Project
- Start and destination
- Distance traveled
- Expenses: Extra expenses can be attached to Travels, like parking
  - Description
  - Amount ($)

## Client

A Client is created in the Database to link related projects for the same client together. It contains this information:

- Name
- Default Project Type: See Project Type in the next section

## Project

A Project is used as a way to group work done by all employees on a single project for billing purposes. It contains this information:

- Code: Numerical code used to quickly keep track of ongoing project. For example, the Project with code 25-32 is the 32nd Project started in the year 2025.
- Name
- Client
- Projet Type: Projects are either Publicly funded or Privately funded. This is used to select the right billing rate for each employee.
- Active status: Keeps track of Projects that are ongoing and which are finished or archived.

## Phase

A Phase divides Projects into smaller units of work. It contains this information:

- Code: 2 letter code used to quickly select Phases in the Application.
- Name: Complete name of the Phase
- Activities in Phase: List of Activities that can be entered on the same Timesheet Line as this Phase.

## Activity

An Activity is a small group of tasks that are billed together to clients. It contains this information:

- Code: 2 letter code used to quickly select Activities in the Application.
- Name: Complete name of the Activity.

## User

A User is the representation of a person's account in the System. It contains this information:

- Username
- User Role: A User Role defines the privileges of the User.
  - User
  - Super User
  - Admin
  - Super Admin 
- Email

### User Roles

#### User

Regular User. They can:

- View Timesheets attached to their Employee entity
- Create and Edit Timesheets attached to their Employee entity

#### Super User

Special kind of User. They can:

- View Timesheets attached to their Employee entity
- View Timesheets attached to all unowned Employees and Employees owned by other Users with `role: User`
- Create and Edit Timesheets attached to their Employee entity
- Access the Admin Panel:
  - Create and Edit Project and Clients
  - Create and Edit Phases and Activities
  - Edit Employees' Roles (See Employee Roles section below). However, they cannot edit their own Employee Roles.

#### Administrator

Administrator of the Application. They can:

- View Timesheets for all Employees, owned and unowned
- Create and Edit Timesheets for any Employee
- Access the Admin Panel:
  - Create and Edit Projects and Clients
  - Create and Edit Phases and Activities
  - Create and Edit Employees (all information).
    - Administrators can only create and edit Employees that are unowned, or owned by other Users with `role: User`, `role: Super User` or `role: Admin`. However, they cannot modify their own Employee's information.
  - Create and Edit Users with roles User, Super User, or Admin. These caveats apply to role changes:
    - Administrators can upgrade User to Super User or Admin
    - Administrators can upgrade Super User to Admin, or downgrade Super User to User
    - Administrators cannot upgrade or downgrade Admin to any level
    - Administrators cannot upgrade or downgrade Super Admin to any level

#### Super Administrator

Super Administrator of the Application. They can do everything:

- View Timesheets for all Employees, owned and unowned
- Create and Edit Timesheets for any Employee
- Access the Admin Panel:
  - Create and Edit Projects and Clients
  - Create and Edit Phases and Activities
  - Create and Edit Employees (all information), unowned and owned by any User with any Role
  - Create and Edit Users with any role

## Employee

An Employee represents a person working for the Business using the Timesheet Manager application on the Business's Projects. It contains this information:

- First and Last name
- Owning User
- Roles

### Employee Roles

An Employee Role keeps information about the Role an Employee had at a given date. This is mainly used to select the right billing rate. Each Role contains this information:

- Title: Name of the role (e.g. Principal Engineer)
- Start and End Date
- Project Type
- Hourly Rate

# Validation

## Timesheet

A Timesheet must be attached to an Employee.

A Timesheet must have a Start and End date. The Start date has to be a Sunday, and the End date has to be a Saturday. A Timesheet must span 2 full weeks.

A Timesheet must have at least one TimesheetLine, but can have no Travel.

A TimesheetLine must be attached to a Project, a Phase, and an Activity. Misc is optional.

The Activity attached to a TimesheetLine must be allowed for the Phase based on the Phase's `activities_in_phase`.

A TimesheetLine must have 14 entries. Each entry must have a unique Date between the Timesheet's Start and End Date inclusively.

A TimesheetEntry has a duration of 0, or a positive amount of time.

A Travel must be attached to a Project.

A Travel must have a Date between the Timesheet's Start and End Date inclusively.

A Travel must have a Start and Destination, which are both free-form text.

A Travel must have a Distance of 0, or a positive amount.

A Travel can have 0 or multiple Expenses attached.

A Travel cannot have a Distance of 0 and no Expenses attached. It must have at least one of those.

An Expense must have a description, which is free-form text.

An Expense must have a strictly positive amount.

## Client

A Client must have a unique Name.

A Client must have a Default Project Type.

## Project

A Project must have a unique numerical code. The Numerical code is made up of 2 or 4 numbers for the year the project started (e.g. projects started in 2025 start with 25 or 2025), a hyphen, and 2 or more numbers incrementing for each new project started in the year. For example, the third project started in 2025 would be 25-03 or 2025-03, the 56th would be 25-56 or 2025-56, the 102nd would be 25-102 or 2025-102, etc.

A Project must have a unique name.

A Project must be attached to a Client.

A Project must have a Project Type.

A Project must have an Active status.

## Phase

A Phase must have a unique 2 letter code.

A Phase must have a unique name.

A Phase must have a non-empty list of Activities that can be used during this Phase on a Timesheet.

## Activity

An Activity must have a unique 2 letter code.

An Activity must have a unique name.

## User

A User must have a unique username.

A User must have a role.

A User must have a unique email.

## Employee

An Employee can be unowned, or owned by a User by having an existing User ID in `owning_user_id`.

An Employee must have a First name.

An Employee must have a Last name.

An Employee must have exactly one active Employee Role for each Project Type. An active Employee Role is one without an End date.

An Employee can have previous Employee Roles for each Project Type.

An Employee can only have a single Employee Role for any given Date for a single Project Type.

An Employee must have a single Employee Role for each Project Type without a Start Date. This is used as "the first" Employee Role an Employee has had.

An Employee Role must have a Title.

An Employee Role must have a Start date, unless it is "the first" Employee Role for a Project Type.

An Employee Role must have an End date, unless it is the Active Employee Role for a Project Type.

An Employee Role must have a Project Type.

An Employee Role must have an Hourly Rate that is strictly positive.