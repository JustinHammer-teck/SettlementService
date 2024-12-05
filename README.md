# Settlement Service

## Requisite
- Dotnet 9.0

## Nix Shell for Dev environment

Nix allows for a reproductable development environment and declarative

## NOTE: 

### Strongly Type GUID & GUIDV7

### Auth
I didn't implement Auth since we would use a library like Auth0 or Azure AD to make authenticate and authorize

### Delta
I use this package to reduce IO to database by caching the response in the client side that reuse the response if the 
data is not change by measure the header 