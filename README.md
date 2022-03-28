# QuestGlobalInterview

Converter: The purpose of this app is to create a sqlite database using the provided JSON file. Just place the JSON file near the .sln, then build the solution. There is a post build command that will automatically copy the JSON file to bin folder. This will also be the place where you can find the sqlite database. This being said, just hit run, then wait one hour until its generated... or take the already generated sqlite file from this repo. Note: I used Newtonsoft to ease the reading of JSON file.

WebApplication: This is the part with REST API. I tried to consume the API here, but since this is my first time working with ASP NET and due to limited time, I failed miserably. In this solution I have implemented two GET requests... nothing too fancy. Data comes from sqlite, using Entity Framework. If you actually try to run the application, you might need this: https://stackoverflow.com/questions/32780315/could-not-find-a-part-of-the-path-bin-roslyn-csc-exe. Also, you need to copy the sqlite file to C:\Program Files\IIS Express 

Client: WPF Application that will call the REST API in order to display the desired data. If it's not working, it's probably because we have different ports and you will need to change this by yourself. You can do this in MainWindowsViewModel, methods: FetchMoviesFromYear, SearchMovieFromYearByName. 


![Animation](https://user-images.githubusercontent.com/25198837/160446718-59a515d0-b976-49e7-8d7c-3b763bb9b84a.gif)


I know that performance is important, this is why I spent some time creating the converter. 
The code is not complete, nor pretty and I'm not that proud of it... but I already spent two full days for writing what you can see. There is no search page or movie page because there is nothing fancy in implementing this, yet it's time consuming.
If you are interested to see something related to tasks, threads, async, await or anything else, just let me know and I will implement one of the missing tasks using such things. I won't do unit tests unless I'm paid!
