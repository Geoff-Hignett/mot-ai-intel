# MOT AI Frontend

Frontend for the MOT AI application, providing:

- Vehicle search by registration
- AI-powered risk analysis display
- User authentication (login/register)
- Personalised recommendations via user profile
- Guest mode with limited functionality

---

## Project Structure

```id="4imw9x"
src/
  api/            Axios API layer (auth, user, vehicle requests)
  components/     Reusable UI components (AuthForm, Profile, Header)
  lib/            Utility functions (JWT handling, token storage)
  styles/         Global styles (App.css / index.css)
  App.tsx         Main application layout and routing logic
  main.tsx        App entry point

public/           Static assets

vite.config.ts    Vite configuration
tsconfig.json     TypeScript configuration
package.json      Dependencies and scripts
```

## Getting Started

### Install dependencies

`npm install`

### Run development server

`npm run dev`

### Configure backend API

`https://localhost:7001` (for example)

## Authentication

JWT-based authentication.

Features:

- Register / Login
- Token stored in localStorage
- Auto-authenticated API requests
- UI updates based on auth state

## User Experience

- Guest Mode (basic)
- Logged in user area for more personalised recommendations
- Search vehicle by registration
- View AI-generated risk + recommendations
