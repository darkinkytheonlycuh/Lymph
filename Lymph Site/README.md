# Lymph Menu Site

Vercel-deployed update server + landing page for the Lymph Gorilla Tag modding suite.

## Repository
https://github.com/darkinkytheonlycuh/Lymph-Menu-Site

## Endpoints
- `GET /api/version?product=menu|injector|executor` — returns latest version, download URL, changelog, features.
- `GET /api/changelog` — full version history.

## Auto-Updater
The Lymph Menu DLL and Lymph Injector check these endpoints on startup and prompt the user to update.

## Deploy
```
vercel --prod
```

## Starting version
V 1.0.0
