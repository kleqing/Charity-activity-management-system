# If you are using this UI for ASP.NET MVC, watch this guide first:

1. [How to add tailwind to ASP NET](https://github.com/angeldev96/tailwind-aspdotnet)
2. Then go to [daisy UI](https://daisyui.com/docs/install/), install using NPM as normal and follow the tutorial there
3. Note: If you style using tailwind WHILE the server is running, you must RESTART (not hot reload) the web server to see changes

# If you want to watch this from live server

## 1. Install tailwind and daisy UI using npm as normal

## 2. Run npm install again

## 2. Enter this line of code first before go live

`npx tailwindcss -i ./styles/input.css -o ./styles/output.css --watch`

- Change the path to where your actual css located (should be already the same if you are not moving anything)
- This make tailwind watch and rebuild the css (Yes, tailwind is not a simple css)
