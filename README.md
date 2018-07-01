# SEOAnalyser
A .NET web application that performs a simple SEO analysis of the text

## Installation & usage

#### System Dependencies & Configuration

To run this web application, you'll need:

* Visual studio 2015  
* Make sure you are connect to internet

1. download this zip folder and extract it.
2. Use Visual Studio to open this project `SEOAnalyser.sln`.
3. Make sure `SEOAnalyser.Web` is your startup project.
4. Run this application.

## Instruction command format

![alt text](https://github.com/Eliceeeeeee/SEOAnalyser/blob/master/SEOAnalyser/Content/SeoAnalyser.PNG)

There are 2 types of Inputs:
1. Raw text, eg: ```Hello the the world!```.
2. Url, eg: ```https://www.google.com```.

There are 3 types of Output Tables:
1. Table show number of occurrences on the page/text of each word

In order to get the number of occurrences on the page/text of each word, please select second option ```Get Number of Words from URL/Text```

The first table, you can select the first option ```Filter Stop Words``` to filter out the stop words, such as ```the```


2. Table show number of occurrences on the page of each word listed in meta tags

In order to get the number of occurrences on the page of each word listed in meta tags, please select third option ```Get Meta Tag info in URL```


3. Table show number of external links in the page

In order to get the number of external links in the page, please select fourth option ```Get All external links from URL/Text```

