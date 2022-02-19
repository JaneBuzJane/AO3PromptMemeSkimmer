# AO3 Prompt Meme Skimmer
A small WinForms app to scrape prompt meme data from the Archive of Our Own (AO3) and generate it into a comma-separated values (.csv) file.

## 🔔 What it is
THe Archive of Our Own (AO3) has no easy way to search a prompt meme collection. Several unofficial tools have been made to search exchanges, but prompt memes have been left out! This application attempts to correct that: given a prompt meme name, a username, and a password, it will scrape a prompt meme collection and generate a CSV file with all of the relevant data (summary, ship, tags, claim status, URL) for each prompt.

## 🔎 Who are you? 
I'm JaneBuzJane. I've been in lots of different fandoms over the years! The person who helped me write this project prefers to remain uncredited, and indeed he has no social media for me to even stealthily give him a shout-out. But let us all raise a glass to Bat, who wrote this entire thing in one evening when I came to him with a hypothetical. 🥂

## 💭 How do I install it?
1. You may have stumbled upon this project for personal use as a prompt meme admin. If you don't want to compile the code from Git, and just want the completed project that you can use, **[check the Wiki.](https://github.com/JaneBuzJane/AO3PromptMemeSkimmer/wiki/Download-pre-built-version)** Otherwise, keep reading! 

2. First, download Visual Studio. It is available for free [here.](https://visualstudio.microsoft.com/free-developer-offers/) You'll want the "Community" (purple) version.

3. Once you have installed Visual Studio, download the skimmer files by clicking the "Code" button, then choosing the "Download ZIP" option. 

![Step 1](https://user-images.githubusercontent.com/23597622/154336426-76086fe0-8fef-442d-912c-6f0ffc0c5cdb.png)

4. Extract the ZIP folder using WinRAR or some other program. When you have done so, open the folder. In the "Claim Skimmer" subfolder, open the C# project file "AO3 Prompt Meme Skimmer" in Visual Studio. 

5. Click the "Build" menu at the top of the window, then select "Build AO3 Prompt Meme Skimmer." 

![image](https://user-images.githubusercontent.com/23597622/154346800-851ff525-048e-4797-9355-bd91a79fc366.png)

6. Success! The application now lives at `\Claim Skimmer\bin\Debug\net5.0-windows`. Double-click to run. As this is a WinForms app, it does not need to be installed.

## 📢 How do I actually use it? 
When you first run the application, you'll be shown a window that prompts you for a collection ID, a username, and a password. 

![image](https://user-images.githubusercontent.com/23597622/154190834-db6d6c60-2393-4328-845a-c356078e8672.png)

The Collection ID is the name of the URL you want to scrape. You get this from the URL, NOT the title of the meme. For instance, in the example below, the collection ID I would enter would be "JBJ_Test_Prompt_Fest," not "Jane's Test Fest." 

![image](https://user-images.githubusercontent.com/23597622/154349772-ee07bd91-c6c3-428e-a9dc-4a724b5d7b31.png)

Then, enter a username and password. These fields are used to log into AO3 to actually scrape the site, and are critical to generating prompt URLs and claim statuses. You can use your own, or you can use an invite from your own account to create a new one.

🔴 Important: This data is **completely private** - I, the project owner, have no way of knowing what you put in this field. It is entirely local to your machine and I will never see it. 

Next, select a save location for the spreadsheet. Then, click "Start!" Once the skimmer has completed, you will find your spreadsheet at the save location you specified. 

## ✔️ I have more questions! 

I've tried to anticipate some FAQs and [put them in the Wiki](https://github.com/JaneBuzJane/AO3PromptMemeSkimmer/wiki). If you're interested in things like how to host this file so others can search it or why you get a "Invalid Tab Name" error, check it out! 
