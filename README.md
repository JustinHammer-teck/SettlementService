# Settlement Service

## Prerequisites

- Dotnet 9.0 SDK 
- nix | nix-darwin (Optional, if you want to run nix-shell)
- just (Optional, if you want to run automate task)

## Demo Testing Note

I already create a hack to mimic InMemoryDatabase behavior that clear everything on shutdown.

Sample Request:

> POST http://localhost:5209/api/Settlement

Sample Request Body

> { "bookingTime": "09:00", "name": "John Doe" }

Name value: is enforce to fullname with firstname and lastname.

## Implementation Note: 

My implementation focusing on showcase Domain Driven Design, application architecture
design pattern and my prefer coding style.
I like to design rich model with enforce business domain constraint instead of 
creating anemic model that would do every on fat service layer. 
I do left some comment on Domain model class where I explain some of my
thought process.

> I hope you have a great time exploring the project, I'm surely enjoy creating this project from scratch !

### SQL

I implement ApplicationDbContext with SQLite since I want to build a complex
EF core entity configuration. Since InMemoryDatabase does not support migration.

### Auth

I didn't implement Auth since we would use a library like Auth0 or Azure AD to make authenticate and authorize.

### Caching

I didn't implement Caching which is a necessary in real-world application.
Since in this example we didn't focus on the retrieval of data in the application.

### Future Work

#### Docker support

#### Testcontainer

In a real project I would love to test out Testcontainer for reliable testing.
Since in that pass I face an error on production that does not occur in testing
with InMemoryDatabase 

