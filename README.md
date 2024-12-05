# Settlement Service

## Prerequisites
- Dotnet 9.0 SDK 
- nix | nix-darwin (Optional, if you want to run nix-shell)
- docker $ docker compose (Optional, if you want to run with SEQ)
- just (Optional, if you want to run automate task)

## Implementation Note: 

### Auth
I didn't implement Auth since we would use a library like Auth0 or Azure AD to make authenticate and authorize

### Caching
I didn't implement Caching which is a necessary in real-world application.
Since in this example we didn't focus on the retrieval of data in the application.